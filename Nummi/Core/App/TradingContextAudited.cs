using NLog;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.App; 

public class StrategyContext : IStrategyContext {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public IClock Clock => TradingSession.Clock;
    
    private ITradingSession TradingSession { get; init; }
    private StrategyLogBuilder LogBuilder { get; init; }
    private object Parameters { get; }
    private object State { get; }

    public StrategyContext(ITradingSession tradingSession, StrategyLogBuilder logBuilder, object parameters, object state) {
        TradingSession = tradingSession;
        LogBuilder = logBuilder;
        Parameters = parameters;
        State = state;
    }

    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period) {
        var start = DateTime.Now;
        var result = TradingSession.GetBars(symbols, dateRange, period);
        var duration = DateTime.Now - start;
        LogBuilder.LogApiCall(duration);
        return result;
    }

    public Order PlaceOrder(OrderRequest request) {
        var fundsBefore = TradingSession.FundSource.RemainingFunds;
        var start = DateTime.Now;
        try {
            var result = TradingSession.PlaceOrder(request);
            var fundsAfter = TradingSession.FundSource.RemainingFunds;
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

    public void LogInfo(string msg) {
        Log.Info("" +
                 $"[{"Bot".Purple()}:{TradingSession.Bot?.Name.Cyan()}]" +
                 $"[{"Strategy".Purple()}:{LogBuilder.Strategy.ParentTemplateVersion.Name.Cyan()}]" +
                 $"[{"Action".Purple()}:{LogBuilder.Action.ToString().Cyan()}] {msg}"
        );
    }

    public P GetParameters<P>() {
        return (P) Parameters;
    }

    public S GetState<S>() {
        return (S) State;
    }
}