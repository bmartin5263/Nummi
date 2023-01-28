using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;
using NLog;
using Nummi.Core.Util;

namespace Nummi.Core.External; 

public class NummiHttpClient {
    
    private HttpClient Client { get; }
    private string BaseUrl { get; }
    private Dictionary<HttpStatusCode, Action<HttpResponseReader>> DefaultStatusCodeActions { get; } = new();
    private List<string> DefaultLogHeaders { get; } = new();

    public NummiHttpClient(HttpClient httpClient, string baseUrl) {
        Client = httpClient;
        BaseUrl = baseUrl;
    }

    public NummiHttpClient OnStatusCode(HttpStatusCode code, Action<HttpResponseReader> action) {
        DefaultStatusCodeActions[code] = action;
        return this;
    }
    
    public NummiHttpClient LogHeaders(params string[] keys) {
        DefaultLogHeaders.AddRange(keys);
        return this;
    }

    public HttpRequestBuilder Post(string suffix = "", object? body = default) {
        return new HttpRequestBuilder(
            client: Client,
            method: HttpMethod.Post,
            baseUrl: BaseUrl,
            suffix: suffix,
            body: body,
            defaultStatusCodeActions: DefaultStatusCodeActions,
            defaultLogHeaders: DefaultLogHeaders
        );
    }

    public HttpRequestBuilder Get(string suffix = "") {
        return new HttpRequestBuilder(
            client: Client,
            method: HttpMethod.Get,
            baseUrl: BaseUrl,
            suffix: suffix,
            body: null,
            defaultStatusCodeActions: DefaultStatusCodeActions,
            defaultLogHeaders: DefaultLogHeaders
        );
    }
    
}

public class HttpRequestBuilder {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    
    private HttpClient Client { get; }
    private string BaseUrl { get; }
    private string Suffix { get; }
    private HttpMethod Method { get; }
    private object? Body { get; }
    private IDictionary<HttpStatusCode, Action<HttpResponseReader>> DefaultStatusCodeActions { get; }
    private IList<string> DefaultLogHeaders { get; }
    private IList<string>? PathArgs { get; set; }
    private IDictionary<string, string>? Parameters { get; set; }

    public HttpRequestBuilder(
        HttpClient client, 
        HttpMethod method, 
        string baseUrl, 
        string suffix, 
        object? body, 
        IDictionary<HttpStatusCode, Action<HttpResponseReader>> defaultStatusCodeActions, 
        IList<string> defaultLogHeaders
    ) {
        Client = client;
        BaseUrl = baseUrl;
        Suffix = suffix;
        Body = body;
        DefaultStatusCodeActions = defaultStatusCodeActions;
        DefaultLogHeaders = defaultLogHeaders;
        Method = method;
    }

    public HttpRequestBuilder PathArg(string value) {
        PathArgs ??= new List<string>();
        PathArgs.Add(value);
        return this;
    }

    public HttpRequestBuilder Parameter(string key, string value) {
        Parameters ??= new Dictionary<string, string>();
        Parameters[key] = value;
        return this;
    }

    public HttpRequestBuilder Parameter(string key, IEnumerable<string> list) {
        Parameters ??= new Dictionary<string, string>();
        var str = '[' + string.Join(",", list.Select(s => $"\"{s}\"")) + ']';
        Parameters[key] = str;
        return this;
    }

    public HttpResponseReader Execute() {
        HttpContent? content = CreateContent();
        string uri = BuildUri();

        Log.Info(content == null
            ? $"{Method.ToString().Yellow()} {uri.Blue()}"
            : $"{Method.ToString().Yellow()} {uri.Blue()} with body [{content}]"
        );

        var tick = DateTime.Now;
        var response = Client.SendAsync(new HttpRequestMessage(method: Method, requestUri: uri) {
            Content = content
        }).Result;
        var tock = DateTime.Now;

        return new HttpResponseReader(response, tock - tick, DefaultStatusCodeActions, DefaultLogHeaders);
    }

    private HttpContent? CreateContent() {
        return Body != null ? JsonContent.Create(Body) : null;
    }

    private string BuildUri() {
        var minLength = BaseUrl.Length + Suffix.Length;
        var uri = new StringBuilder(BaseUrl, minLength);

        AppendSuffix(ref uri);
        AppendParameters(ref uri);

        return uri.ToString();
    }

    private void AppendSuffix(ref StringBuilder uri) {
        if (Suffix.Length == 0) {
            return;
        }

        if (uri[^1] != '/' && Suffix[0] != '/') {
            uri.Append('/');
        }
        
        int pathArgIndex = 0;
        bool inOpenBracket = false;
        foreach (var c in Suffix) {
            if (inOpenBracket) {
                if (c == '}') {
                    if (pathArgIndex >= (PathArgs?.Count ?? 0)) {
                        throw new ArgumentException($"Suffix '{Suffix}' does not have enough Path argument placeholders for {PathArgs}");
                    }
                    uri.Append(PathArgs![pathArgIndex++]);
                    inOpenBracket = false;
                }
                else {
                    throw new ArgumentException($"Suffix '{Suffix}' has an invalid format");
                }
            }
            else {
                if (c == '{') {
                    inOpenBracket = true;
                }
                else {
                    uri.Append(c);
                }
            }
        }
    }

    private void AppendParameters(ref StringBuilder uri) {
        if (Parameters == null) {
            return;
        }

        using var iter = Parameters.GetEnumerator();
        if (iter.MoveNext()) {
            var entry = iter.Current;
            AppendParameter(ref uri, ref entry, '?');
        }
        while (iter.MoveNext()) {
            var entry = iter.Current;
            AppendParameter(ref uri, ref entry, '&');
        }
    }

    private void AppendParameter(ref StringBuilder uri, ref KeyValuePair<string, string> entry, char delimiter) {
        uri.Append(delimiter);
        uri.Append(entry.Key).Append('=').Append(HttpUtility.UrlEncode(entry.Value).Replace("%2c", ","));
    }
}

public class HttpResponseReader {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    
    private HttpResponseMessage Response { get; }
    private TimeSpan Time { get; }
    private IDictionary<HttpStatusCode, Action<HttpResponseReader>> DefaultStatusCodeActions { get; }
    private IList<string> DefaultLogHeaders { get; }
    private string Json { get; }

    public HttpResponseReader(
        HttpResponseMessage response,
        TimeSpan time,
        IDictionary<HttpStatusCode, Action<HttpResponseReader>> defaultStatusCodeActions, 
        IList<string> defaultLogHeaders
    ) {
        Response = response;
        Time = time;
        DefaultStatusCodeActions = defaultStatusCodeActions;
        DefaultLogHeaders = defaultLogHeaders;
        Json = Response.Content.ReadAsStringAsync().Result;
        if (Response.IsSuccessStatusCode) {
            Log.Info("Code: ".Yellow() + Response.StatusCode.ToString().Green());
            Log.Info("Time: ".Yellow() + Time.ToString().Cyan());
            if (DefaultLogHeaders.Count > 0) {
                LogHeaders(DefaultLogHeaders.ToArray());
            }
        }
        else {
            if (((int)Response.StatusCode >= 500) && ((int)Response.StatusCode <= 599)) {
                Log.Info("Code: ".Yellow() + Response.StatusCode.ToString().Red());
            }
            else {
                Log.Info("Code: ".Yellow() + Response.StatusCode.ToString().Purple());
            }
            Log.Info("Headers: ".Yellow() + Response.Headers.ToJoinedString().Cyan());
            Log.Info("Body: ".Yellow() + Json.Purple());
        }

        if (DefaultStatusCodeActions.TryGetValue(Response.StatusCode, out Action<HttpResponseReader>? handler)) {
            handler(this);
        }
    }

    public HttpResponseReader LogAllHeaders() {
        Log.Info("Headers: ".Yellow() + Response.Headers.ToJoinedString().Cyan());
        return this;
    }

    public HttpResponseReader LogHeaders(params string[] keys) {
        var keySet = new HashSet<string>(keys.Select(v => v.ToUpper()));

        var str = Response.Headers
            .Where(v => keySet.Contains(v.Key.ToUpper()))
            .ToJoinedString();

        Log.Info("Headers: ".Yellow() + str.Cyan());

        return this;
    }

    public HttpResponseReader OnStatusCode(HttpStatusCode code, Action<HttpResponseReader> action) {
        if (Response.StatusCode != code) {
            return this;
        }
        
        if (DefaultStatusCodeActions.TryGetValue(code, out Action<HttpResponseReader>? value)) {
            value(this);
        }
        else {
            action(this);
        }

        return this;
    }

    public HttpResponseReader OnStatusCodes(HttpStatusCode[] codes, Action<HttpResponseReader> action) {
        if (!codes.Contains(Response.StatusCode)) {
            return this;
        }
        
        if (DefaultStatusCodeActions.TryGetValue(Response.StatusCode, out Action<HttpResponseReader>? value)) {
            value(this);
        }
        else {
            action(this);
        }

        return this;
    }

    public HttpResponseReader OnStatusCode(int code, Action<HttpResponseReader> action) {
        return OnStatusCode((HttpStatusCode) code, action);
    }
    
    public HttpResponseReader ReadHeader(string key, out IEnumerable<string>? values) {
        Response.Headers.TryGetValues(key, out values);
        return this;
    }
    
    public HttpResponseReader ReadFirstHeader(string key, out string? value) {
        Response.Headers.TryGetValues(key, out IEnumerable<string>? values);
        value = values?.FirstOrDefault();
        return this;
    }
    
    public HttpResponseReader ReadFirstHeader(string key, out string value, string orElse) {
        Response.Headers.TryGetValues(key, out IEnumerable<string>? values);
        value = values?.FirstOrDefault(orElse) ?? orElse;
        return this;
    }
    
    public HttpResponseReader ReadFirstHeader<T>(string key, out T? value, Func<string, T> mapper) {
        ReadFirstHeader(key, out string? strValue);
        value = strValue != null ? mapper(strValue) : default;
        return this;
    }
    
    public HttpResponseReader ReadFirstHeader<T>(string key, out T value, T orElse, Func<string, T> mapper) {
        ReadFirstHeader(key, out string? strValue);
        value = strValue != null ? mapper(strValue) : orElse;
        return this;
    }

    public T ReadJson<T>() {
        return Serializer.FromJson<T>(Json)!;
    }

    public JsonElement ReadJsonElement() {
        return Serializer.ToJsonElement(Json);
    }
}