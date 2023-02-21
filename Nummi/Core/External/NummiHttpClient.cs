using System.Net;
using System.Text;
using System.Web;
using NLog;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.External; 

public class NummiHttpClient {
    
    private HttpClient Client { get; }
    private string BaseUrl { get; }
    private Dictionary<HttpStatusCode, Action<HttpResponse>> DefaultStatusCodeActions { get; } = new();
    private List<string> DefaultLogHeaders { get; } = new();

    public NummiHttpClient(HttpClient httpClient, string baseUrl) {
        Client = httpClient;
        BaseUrl = baseUrl;
    }

    public NummiHttpClient OnStatusCode(HttpStatusCode code, Action<HttpResponse> action) {
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
    private IDictionary<HttpStatusCode, Action<HttpResponse>> DefaultStatusCodeActions { get; }
    private IList<string> DefaultLogHeaders { get; }
    private IList<string>? PathArgs { get; set; }
    private IDictionary<string, string>? Parameters { get; set; }

    public HttpRequestBuilder(
        HttpClient client, 
        HttpMethod method, 
        string baseUrl, 
        string suffix, 
        object? body, 
        IDictionary<HttpStatusCode, Action<HttpResponse>> defaultStatusCodeActions, 
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

    public HttpResponse Execute() {
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

        var statusCode = response.StatusCode;
        var headers = response.Headers;
        var time = tock - tick;
        string body = response.Content.ReadAsStringAsync().Result;

        return new HttpResponse(statusCode, headers, time, body, DefaultStatusCodeActions, DefaultLogHeaders);
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
                        throw new SystemArgumentException($"Suffix '{Suffix}' does not have enough Path argument placeholders for {PathArgs}");
                    }
                    uri.Append(PathArgs![pathArgIndex++]);
                    inOpenBracket = false;
                }
                else {
                    throw new SystemArgumentException($"Suffix '{Suffix}' has an invalid format");
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