using Nummi.Core.Domain.Crypto.Client;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies; 

public class StrategyContext {
    private TradingEnvironment Environment { get; init; }
    public StrategyLogBuilder LogBuilder { get; }

    public IClock Clock => Environment.Clock;
    public ICryptoDataClient DataClient => Environment.DataClient;

    public StrategyContext(TradingEnvironment environment, StrategyLogBuilder logBuilder) {
        LogBuilder = logBuilder;
        Environment = environment;
    }

}