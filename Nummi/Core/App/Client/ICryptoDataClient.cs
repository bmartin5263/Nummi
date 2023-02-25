using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.External.Binance;

namespace Nummi.Core.App.Client; 

public interface ICryptoDataClient {
    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period);
}