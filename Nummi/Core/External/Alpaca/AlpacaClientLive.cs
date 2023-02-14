using Alpaca.Markets;
using Environments = Alpaca.Markets.Environments;

namespace Nummi.Core.External.Alpaca; 

public class AlpacaClientLive : IAlpacaClient {

    public IAlpacaDataClient DataClient { get; }
    public IAlpacaCryptoDataClient CryptoDataClient { get; }
    public IAlpacaTradingClient TradingClient { get; }
    
    public AlpacaClientLive() : 
        this(Environment.GetEnvironmentVariable("ALPACA_LIVE_KEY_ID")!, Environment.GetEnvironmentVariable("ALPACA_LIVE_SECRET_KEY")!) {
    }
    
    public AlpacaClientLive(string userId, string secret) {
        var key = CreateKey(userId, secret);
        DataClient = Environments.Live.GetAlpacaDataClient(key);
        CryptoDataClient = Environments.Live.GetAlpacaCryptoDataClient(key);
        TradingClient = Environments.Live.GetAlpacaTradingClient(key);
    }

    private static SecretKey CreateKey(string userId, string key) {
        return new SecretKey(
            userId ?? throw new ArgumentException("Missing ALPACA_PAPER_KEY_ID"),
            key ?? throw new ArgumentException("Missing ALPACA_PAPER_SECRET_KEY")
        );
    }
}