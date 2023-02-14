using Alpaca.Markets;
using Environments = Alpaca.Markets.Environments;

namespace Nummi.Core.External.Alpaca; 

public class AlpacaClientPaper : IAlpacaClient {
    
    public IAlpacaDataClient DataClient { get; }
    public IAlpacaCryptoDataClient CryptoDataClient { get; }
    public IAlpacaTradingClient TradingClient { get; }
    
    public AlpacaClientPaper() : 
        this(Environment.GetEnvironmentVariable("ALPACA_PAPER_KEY_ID")!, Environment.GetEnvironmentVariable("ALPACA_PAPER_SECRET_KEY")!) {
    }
    
    public AlpacaClientPaper(string userId, string secret) {
        var key = CreateKey(userId, secret);
        DataClient = Environments.Paper.GetAlpacaDataClient(key);
        CryptoDataClient = Environments.Paper.GetAlpacaCryptoDataClient(key);
        TradingClient = Environments.Paper.GetAlpacaTradingClient(key);
    }

    private static SecretKey CreateKey(string userId, string key) {
        return new SecretKey(
            userId ?? throw new ArgumentException("Missing ALPACA_PAPER_KEY_ID"),
            key ?? throw new ArgumentException("Missing ALPACA_PAPER_SECRET_KEY")
        );
    }
}