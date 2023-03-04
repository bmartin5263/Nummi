using Nummi.Core.App;
using Nummi.Core.Domain.Common;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto;

public interface ITradingSession {
    public IClock Clock { get; }
    public IFundSource FundSource { get; }
    public TradingMode Mode { get; }
    public Bots.Bot? Bot { get; }
    
    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period);
    public Order PlaceOrder(OrderRequest request);
}

public interface IStrategyContext {
    public IClock Clock { get; }
    
    public P GetParameters<P>();
    public S GetState<S>();
    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period);
    public Order PlaceOrder(OrderRequest request);

    public void LogInfo(string msg);
}