using Alpaca.Markets;
using Environments = Alpaca.Markets.Environments;

namespace Nummi.Core.External.Alpaca; 

public class AlpacaClientPaper : IAlpacaClient {
    
    public IAlpacaDataClient DataClient { get; }
        = Environments.Paper.GetAlpacaDataClient(CreateKey());

    public IAlpacaCryptoDataClient CryptoDataClient { get; }
        = Environments.Paper.GetAlpacaCryptoDataClient(CreateKey());

    public IAlpacaTradingClient TradingClient { get; }
        = Environments.Paper.GetAlpacaTradingClient(CreateKey());

    private static SecretKey CreateKey() {
        return new SecretKey(
            Environment.GetEnvironmentVariable("ALPACA_PAPER_KEY_ID") ?? throw new ArgumentException("Missing ALPACA_PAPER_KEY_ID"),
            Environment.GetEnvironmentVariable("ALPACA_PAPER_SECRET_KEY") ?? throw new ArgumentException("Missing ALPACA_PAPER_SECRET_KEY")
        );
    }
}