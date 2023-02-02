using Nummi.Core.Database;
using Nummi.Core.Domain.Crypto.Client;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Exceptions;
using Nummi.Core.External.Alpaca;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies; 

public class TradingContextFactory {
    private AppDb AppDb { get; }
    private CryptoDataClientLive DataClientLive { get; }
    private CryptoDataClientDbProxy DataClientDbProxy { get; }
    private AlpacaClientPaper AlpacaClientPaper { get; }
    private AlpacaClientLive AlpacaClientLive { get; }
    
    public TradingContextFactory(AppDb appDb, CryptoDataClientLive dataClientLive, CryptoDataClientDbProxy dataClientDbProxy, AlpacaClientPaper alpacaClientPaper, AlpacaClientLive alpacaClientLive) {
        AppDb = appDb;
        DataClientLive = dataClientLive;
        DataClientDbProxy = dataClientDbProxy;
        AlpacaClientPaper = alpacaClientPaper;
        AlpacaClientLive = alpacaClientLive;
    }

    public TradingContext Create(TradingMode mode, decimal funds, IClock clock) {
        switch (mode) {
            case TradingMode.Simulated:
                return new TradingContext(
                    mode: mode,
                    dataClient: DataClientLive,
                    tradingClient: new CryptoTradingClientRealtime(AlpacaClientPaper),
                    allowance: funds,
                    appDb: AppDb,
                    clock: clock
                );
            case TradingMode.Paper:
                return new TradingContext(
                    mode: mode,
                    dataClient: DataClientLive,
                    tradingClient: new CryptoTradingClientRealtime(AlpacaClientPaper),
                    allowance: funds,
                    appDb: AppDb,
                    clock: new ClockLive()
                );
            case TradingMode.Live:
                return new TradingContext(
                    mode: mode,
                    dataClient: DataClientLive,
                    tradingClient: new CryptoTradingClientRealtime(AlpacaClientLive),
                    allowance: funds,
                    appDb: AppDb,
                    clock: new ClockLive()
                );
            default:
                throw new InvalidUserArgumentException(nameof(mode));
        }
    }
    
}