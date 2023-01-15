namespace Nummi.Api.Model; 

public class StrategyFilterResponse {
    public IList<StrategyDto> Strategies { get; }

    public StrategyFilterResponse(IList<StrategyDto> strategies) {
        Strategies = strategies;
    }
}