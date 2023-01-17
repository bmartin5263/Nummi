using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;
using NLog;

namespace Nummi.Core.Util; 

public class NummiHttpClient {
    
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    
    private HttpClient Client { get; }
    private string BaseUrl { get; }

    public NummiHttpClient(HttpClient httpClient, string baseUrl) {
        Client = httpClient;
        BaseUrl = baseUrl;
    }

    public HttpRequestBuilder Post(string suffix = "", object? body = default) {
        return new HttpRequestBuilder(
            client: Client,
            method: HttpMethod.Post,
            baseUrl: BaseUrl,
            suffix: suffix,
            body: body
        );
    }

    public HttpRequestBuilder Get(string suffix = "") {
        return new HttpRequestBuilder(
            client: Client,
            method: HttpMethod.Get,
            baseUrl: BaseUrl,
            suffix: suffix,
            body: null
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
    private IList<string>? PathArgs { get; set; }
    private IDictionary<string, string>? Parameters { get; set; }

    public HttpRequestBuilder(HttpClient client, HttpMethod method, string baseUrl, string suffix, object? body) {
        Client = client;
        BaseUrl = baseUrl;
        Suffix = suffix;
        Body = body;
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

        var response = Client.SendAsync(new HttpRequestMessage(method: Method, requestUri: uri) {
            Content = content
        });

        return new HttpResponseReader(response.Result);
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
    private string Json { get; }

    public HttpResponseReader(HttpResponseMessage response) {
        Response = response;
        Json = Response.Content.ReadAsStringAsync().Result;
        if (Response.IsSuccessStatusCode) {
            Log.Info("Code: ".Yellow() + Response.StatusCode.ToString().Green());
        }
        else {
            Log.Info("Code: ".Yellow() + Response.StatusCode.ToString().Red());
            Log.Info("Headers: ".Yellow() + Response.Headers.ToJoinedString().Cyan());
            Log.Info("Body: ".Yellow() + Json.Purple());
        }
    }

    public HttpResponseReader LogAllHeaders(params string[] keys) {
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
        if (Response.StatusCode == code) {
            action(this);
        }

        return this;
    }

    public HttpResponseReader OnStatusCode(int code, Action<HttpResponseReader> action) {
        return OnStatusCode((HttpStatusCode) code, action);
    }

    public T ReadJson<T>() {
        return Serializer.FromJson<T>(Json)!;
    }

    public JsonElement ReadJsonElement() {
        return Serializer.ToJsonElement(Json);
    }
}