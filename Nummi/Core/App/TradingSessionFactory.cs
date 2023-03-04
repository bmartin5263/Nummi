using Nummi.Core.App.Client;
using Nummi.Core.Domain.Bots;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Exceptions;
using Nummi.Core.External.Alpaca;
using Nummi.Core.Util;

namespace Nummi.Core.App; 

public class TradingSessionFactory {
    private CryptoDataClientLive DataClientLive { get; }
    private CryptoDataClientDbProxy DataClientDbProxy { get; }
    private AlpacaClientPaper AlpacaClientPaper { get; }
    private AlpacaClientLive AlpacaClientLive { get; }
    
    public TradingSessionFactory(CryptoDataClientLive dataClientLive, CryptoDataClientDbProxy dataClientDbProxy, AlpacaClientPaper alpacaClientPaper, AlpacaClientLive alpacaClientLive) {
        DataClientLive = dataClientLive;
        DataClientDbProxy = dataClientDbProxy;
        AlpacaClientPaper = alpacaClientPaper;
        AlpacaClientLive = alpacaClientLive;
    }

    public ITradingSession CreateRealtime(Bot bot) {
        TradingMode mode = bot.Mode;
        switch (mode) {
            case TradingMode.Paper:
                return new TradingSession(
                    mode: mode,
                    fundSource: new FundSourceBot(bot),
                    dataClient: DataClientLive,
                    tradingClient: new CryptoTradingClientRealtime(AlpacaClientPaper),
                    clock: new ClockLive(),
                    bot: bot
                );
            case TradingMode.Live:
                return new TradingSession(
                    mode: mode,
                    fundSource: new FundSourceBot(bot),
                    dataClient: DataClientLive,
                    tradingClient: new CryptoTradingClientRealtime(AlpacaClientLive),
                    clock: new ClockLive(),
                    bot: bot
                );
            default:
                throw new InvalidUserArgumentException(nameof(mode));
        }
    }

    public ITradingSession CreateSimulated(decimal funds, IClock clock) {
        return new TradingSession(
            mode: TradingMode.Simulated,
            fundSource: new FundSourceInMemory(funds),
            dataClient: DataClientDbProxy,
            tradingClient: new CryptoTradingClientSimulated(),
            clock: clock,
            bot: null
        );
    }
    
}