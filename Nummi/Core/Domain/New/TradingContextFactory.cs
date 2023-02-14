using Nummi.Core.Domain.Crypto.Client;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Exceptions;
using Nummi.Core.External.Alpaca;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New; 

public class TradingContextFactory {
    private CryptoDataClientLive DataClientLive { get; }
    private CryptoDataClientDbProxy DataClientDbProxy { get; }
    private AlpacaClientPaper AlpacaClientPaper { get; }
    private AlpacaClientLive AlpacaClientLive { get; }
    
    public TradingContextFactory(CryptoDataClientLive dataClientLive, CryptoDataClientDbProxy dataClientDbProxy, AlpacaClientPaper alpacaClientPaper, AlpacaClientLive alpacaClientLive) {
        DataClientLive = dataClientLive;
        DataClientDbProxy = dataClientDbProxy;
        AlpacaClientPaper = alpacaClientPaper;
        AlpacaClientLive = alpacaClientLive;
    }

    public ITradingContext CreateRealtime(Bot bot) {
        TradingMode mode = bot.Mode;
        switch (mode) {
            case TradingMode.Paper:
                return new TradingContext(
                    botId: bot.Id,
                    mode: mode,
                    fundSource: new FundSourceBot(bot),
                    dataClient: DataClientLive,
                    tradingClient: new CryptoTradingClientRealtime(AlpacaClientPaper),
                    clock: new ClockLive()
                );
            case TradingMode.Live:
                return new TradingContext(
                    botId: bot.Id,
                    mode: mode,
                    fundSource: new FundSourceBot(bot),
                    dataClient: DataClientLive,
                    tradingClient: new CryptoTradingClientRealtime(AlpacaClientLive),
                    clock: new ClockLive()
                );
            default:
                throw new InvalidUserArgumentException(nameof(mode));
        }
    }

    public ITradingContext CreateSimulated(decimal funds, IClock clock) {
        return new TradingContext(
            botId: null,
            mode: TradingMode.Simulated,
            fundSource: new FundSourceInMemory(funds),
            dataClient: DataClientDbProxy,
            tradingClient: new CryptoTradingClientSimulated(),
            clock: clock
        );
    }
    
}