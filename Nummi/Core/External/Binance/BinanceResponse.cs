using System.Net;

namespace Nummi.Core.External.Binance; 

public class BinanceResponse<T> {
    public HttpStatusCode StatusCode { get; }
    public T Content { get; }
    public int UsedWeight1M { get; }
    public int? RetryAfter { get; }

    public BinanceResponse(HttpStatusCode statusCode, T content, int usedWeight1M, int? retryAfter) {
        StatusCode = statusCode;
        Content = content;
        UsedWeight1M = usedWeight1M;
        RetryAfter = retryAfter;
    }
}