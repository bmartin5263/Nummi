using Nummi.Core.Domain.Crypto.Data;

namespace Nummi.Core.Domain.Crypto.Strategies; 

public class TradingInterface {
    private StrategyLogBuilder LogBuilder { get; set; }

    public TradingInterface(StrategyLogBuilder logBuilder) {
        LogBuilder = logBuilder;
    }

    public IEnumerable<MinuteBar> GetMinuteBars(IEnumerable<string> symbols) {
        throw new NotImplementedException();
    } 

    public IEnumerable<MinuteBar> GetHistoricalMinuteBars(
        IEnumerable<string> symbols,
        DateTime from,
        DateTime to
    ) {
        throw new NotImplementedException();
    } 
}