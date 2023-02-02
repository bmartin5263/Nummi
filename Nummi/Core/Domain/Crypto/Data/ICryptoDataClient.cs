using Nummi.Core.Domain.Common;
using Nummi.Core.External.Binance;

namespace Nummi.Core.Domain.Crypto.Data; 

public interface ICryptoDataClient {
    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period);
}