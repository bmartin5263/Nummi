using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto.Data;

namespace Nummi.Core.External.Binance;

public interface IBinanceClient {
    
    IDictionary<string, Bar> GetBar(ISet<string> symbols, DateTime time, Period period);
    
    IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period);
    
    IDictionary<string, List<Bar>> GetBars(IDictionary<string, DateRange> symbols, Period period);
    
    ExchangeInfo GetExchangeInfo();
}