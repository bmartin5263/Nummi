using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies;

public interface ITradingContext {
    public IClock Clock { get; }
    public decimal RemainingFunds { get; }
    public TradingMode Mode { get; }
    public string? BotId { get; }

    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period);
    public Order PlaceOrder(OrderRequest request);
}