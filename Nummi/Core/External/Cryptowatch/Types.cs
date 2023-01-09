namespace Nummi.Core.External.Cryptowatch;

public class Response<T> {
    public T? Result { get; set; }
}

public class PriceResponse {
    public decimal? Price { get; set; }
}