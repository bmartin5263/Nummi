using Alpaca.Markets;
using Environments = Alpaca.Markets.Environments;

namespace Nummi.Core.External.Alpaca; 

public class AlpacaClientLive : IAlpacaClient {

    public IAlpacaDataClient DataClient { get; }
        = Environments.Live.GetAlpacaDataClient(CreateKey());

    public IAlpacaCryptoDataClient CryptoDataClient { get; }
        = Environments.Live.GetAlpacaCryptoDataClient(CreateKey());

    public IAlpacaTradingClient TradingClient { get; }
        = Environments.Live.GetAlpacaTradingClient(CreateKey());

    private static SecretKey CreateKey() {
        return new SecretKey(
            Environment.GetEnvironmentVariable("ALPACA_LIVE_KEY_ID") ?? throw new ArgumentException("Missing ALPACA_LIVE_KEY_ID"),
            Environment.GetEnvironmentVariable("ALPACA_LIVE_SECRET_KEY") ?? throw new ArgumentException("Missing ALPACA_LIVE_SECRET_KEY")
        );
    }
}