using Nummi.Core.Domain.Common;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New; 

public interface ITradingContext {
    public IClock Clock { get; }
    public decimal RemainingFunds { get; }
    public TradingMode Mode { get; }
    public Ksuid? BotId { get; }
    public object? Parameters { get; }
    public object? State { get; set; }

    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period);
    public Order PlaceOrder(OrderRequest request);
}

public interface ITradingContext<P, S> : ITradingContext where P : class where S : class {
    public new P? Parameters { get; } 
    public new S? State { get; set; }

    object? ITradingContext.Parameters => Parameters;

    object? ITradingContext.State {
        get => State;
        set => State = (S?) value;
    }
}