using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Domain.Crypto.Ordering;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies;

public interface ITradingContext {
    public IClock Clock { get; }
    public decimal Funds { get; }
    public TradingMode Mode { get; }

    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period);
    public Order PlaceOrder(OrderRequest request);
}