using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New; 

public class TradingContextAudited : ITradingContext {
    private ITradingContext Delegate { get; init; }
    public StrategyLogBuilder LogBuilder { get; }

    public IClock Clock => Delegate.Clock;
    public decimal RemainingFunds => Delegate.RemainingFunds;
    public TradingMode Mode => Delegate.Mode;
    public Ksuid? BotId => Delegate.BotId;
    public object? Parameters => Delegate.Parameters;

    public object? State {
        get => Delegate.State;
        set => Delegate.State = value;
    }

    public TradingContextAudited(ITradingContext delegateContext, StrategyLogBuilder logBuilder) {
        LogBuilder = logBuilder;
        Delegate = delegateContext;
    }

    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period) {
        var start = DateTime.Now;
        var result = Delegate.GetBars(symbols, dateRange, period);
        var duration = DateTime.Now - start;
        LogBuilder.LogApiCall(duration);
        return result;
    }

    public Order PlaceOrder(OrderRequest request) {
        var fundsBefore = RemainingFunds;
        var start = DateTime.Now;
        try {
            var result = Delegate.PlaceOrder(request);
            var fundsAfter = RemainingFunds;
            LogBuilder.LogOrder(request, fundsBefore, fundsAfter);
            return result;
        }
        catch (Exception e) {
            LogBuilder.LogOrder(request, fundsBefore, e);
            throw;
        }
        finally {
            var duration = DateTime.Now - start;
            LogBuilder.LogApiCall(duration);
        }
    }
}