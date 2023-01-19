using Nummi.Core.Domain.Crypto.Data;

namespace Nummi.Core.Domain.Crypto.Strategies; 

public class TradingInterface {
    private StrategyLogBuilder LogBuilder { get; set; }

    public TradingInterface(StrategyLogBuilder logBuilder) {
        LogBuilder = logBuilder;
    }

    public MinuteCandlestick GetMinuteCandlestick() {
        throw new NotImplementedException();
    } 
}