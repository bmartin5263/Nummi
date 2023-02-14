using KSUID;
using Nummi.Core.Domain.Common;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New; 

public interface ITradingContext {
    public IClock Clock { get; }
    public decimal RemainingFunds { get; }
    public TradingMode Mode { get; }
    public Ksuid? BotId { get; }

    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period);
    public Order PlaceOrder(OrderRequest request);
}