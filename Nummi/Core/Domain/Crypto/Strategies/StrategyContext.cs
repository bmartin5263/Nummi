using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies; 

public class StrategyContext : ITradingContext {
    private TradingContext Delegate { get; init; }
    public StrategyLogBuilder LogBuilder { get; }

    public IClock Clock => Delegate.Clock;
    public ICryptoDataClient DataClient => Delegate.DataClient;

    public StrategyContext(TradingContext delegateContext, StrategyLogBuilder logBuilder) {
        LogBuilder = logBuilder;
        Delegate = delegateContext;
    }

    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period) {
        // log start
        var result = Delegate.GetBars(symbols, dateRange, period);
        // log end
        return result;
    }
}