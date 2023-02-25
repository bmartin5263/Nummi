using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using NLog;
using Nummi.Core.Util;

namespace Nummi.Core.External; 

public class HttpResponse {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    
    //private HttpResponseMessage Response { get; }
    
    public HttpStatusCode StatusCode { get; }
    public HttpHeaders Headers { get; }
    public TimeSpan Time { get; }
    public string JsonBody { get; }

    public HttpResponse(HttpStatusCode statusCode, HttpHeaders headers, TimeSpan time, string body) {
        StatusCode = statusCode;
        Headers = headers;
        Time = time;
        JsonBody = body;
    }

    public HttpResponse(
        HttpStatusCode statusCode, 
        HttpHeaders headers, 
        TimeSpan time, 
        string body,
        IDictionary<HttpStatusCode, Action<HttpResponse>> defaultStatusCodeActions, 
        IList<string> defaultLogHeaders
    ) : this(statusCode, headers, time, body) {
        if (((int)StatusCode >= 200) && ((int)StatusCode <= 299)) {
            Log.Info("Code: ".Yellow() + StatusCode.ToString().Green());
            Log.Info("Time: ".Yellow() + Time.ToString().Cyan());
            if (defaultLogHeaders.Count > 0) {
                LogHeaders(defaultLogHeaders.ToArray());
            }
        }
        else {
            if (((int)StatusCode >= 500) && ((int)StatusCode <= 599)) {
                Log.Info("Code: ".Yellow() + StatusCode.ToString().Red());
            }
            else {
                Log.Info("Code: ".Yellow() + StatusCode.ToString().Purple());
            }
            Log.Info("Headers: ".Yellow() + Headers.ToJoinedString().Cyan());
            Log.Info("Body: ".Yellow() + body?.ToString()?.Purple());
        }

        if (defaultStatusCodeActions.TryGetValue(StatusCode, out Action<HttpResponse>? handler)) {
            handler(this);
        }
    }

    public HttpResponse LogAllHeaders() {
        Log.Info("Headers: ".Yellow() + Headers.ToJoinedString().Cyan());
        return this;
    }

    public HttpResponse LogHeaders(params string[] keys) {
        var keySet = new HashSet<string>(keys.Select(v => v.ToUpper()));

        var str = Headers
            .Where(v => keySet.Contains(v.Key.ToUpper()))
            .ToJoinedString();

        Log.Info("Headers: ".Yellow() + str.Cyan());

        return this;
    }

    public HttpResponse OnStatusCode(HttpStatusCode code, Action<HttpResponse> action) {
        if (StatusCode == code) {
            action(this);
        }
        return this;
    }

    public HttpResponse OnStatusCodes(HttpStatusCode[] codes, Action<HttpResponse> action) {
        if (codes.Contains(StatusCode)) {
            action(this);
        }
        return this;
    }

    public HttpResponse OnStatusCode(int code, Action<HttpResponse> action) {
        return OnStatusCode((HttpStatusCode) code, action);
    }
    
    public HttpResponse ReadHeader(string key, out IEnumerable<string>? values) {
        Headers.TryGetValues(key, out values);
        return this;
    }
    
    public HttpResponse ReadFirstHeader(string key, out string? value) {
        Headers.TryGetValues(key, out IEnumerable<string>? values);
        value = values?.FirstOrDefault();
        return this;
    }
    
    public HttpResponse ReadFirstHeader(string key, out string value, string orElse) {
        Headers.TryGetValues(key, out IEnumerable<string>? values);
        value = values?.FirstOrDefault(orElse) ?? orElse;
        return this;
    }
    
    public HttpResponse ReadFirstHeader<V>(string key, out V? value, Func<string, V> mapper) {
        ReadFirstHeader(key, out string? strValue);
        value = strValue != null ? mapper(strValue) : default;
        return this;
    }
    
    public HttpResponse ReadFirstHeader<V>(string key, out V value, V orElse, Func<string, V> mapper) {
        ReadFirstHeader(key, out string? strValue);
        value = strValue != null ? mapper(strValue) : orElse;
        return this;
    }
    
    public HttpResponse ReadJson<T>(out T value) {
        value = Serializer.FromJson<T>(JsonBody)!;
        return this;
    }

    public HttpResponse ReadJsonElement(out JsonElement value) {
        value = Serializer.FromJson(JsonBody);
        return this;
    }
}