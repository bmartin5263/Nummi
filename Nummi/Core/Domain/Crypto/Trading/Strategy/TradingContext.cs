using Nummi.Core.Domain.Crypto.Client;

namespace Nummi.Core.Domain.Crypto.Trading.Strategy;

public class TradingContext {
    public ICryptoClient CryptoClient { get; }
    public decimal Allowance { get; }

    public TradingContext(ICryptoClient cryptoClient, decimal allowance) {
        CryptoClient = cryptoClient;
        Allowance = allowance;
    }
}