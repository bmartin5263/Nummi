using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Ordering;

public class OrderService {

    private TradingContextFactory TradingContextFactory { get; }

    public OrderService(TradingContextFactory tradingContextFactory) {
        TradingContextFactory = tradingContextFactory;
    }

    public Order PlaceOrder(TradingMode mode, OrderRequest request) {
        switch (mode) {
            case TradingMode.Paper:
                var context = TradingContextFactory.Create(mode, 10m, new ClockLive());
                return context.PlaceOrder(request);
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }

}