using Nummi.Core.Domain.Crypto;

namespace Nummi.Core.External.Binance;

public interface IBinanceClient {
    int GetKlinesWeight { get; }
    int GetKlinesMaxLimit { get; }
    
    BinanceResponse<IList<Bar>> GetKlines(string symbol, DateTimeOffset startTime, DateTimeOffset endTime, Period period, int limit);
    BinanceResponse<ExchangeInfo> GetExchangeInfo();
    void Wait(TimeSpan time);
}