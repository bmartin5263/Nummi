using Nummi.Core.Domain.Crypto.Client;
using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.External.Alpaca;

namespace Nummi.Core.Domain.Crypto.Ordering;

public class OrderService {

    private AlpacaClientPaper PaperClient { get; }
    private AlpacaClientLive LiveClient { get; }

    public OrderService(AlpacaClientPaper paperClient, AlpacaClientLive liveClient) {
        PaperClient = paperClient;
        LiveClient = liveClient;
    }

    public async Task<Order> PlaceOrderAsync(TradingMode mode, PlaceOrderRq request) {
        switch (mode) {
            case TradingMode.Paper:
                return await new CryptoTradingClientRealtime(PaperClient).PlaceOrderAsync(request);
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }

}