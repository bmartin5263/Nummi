using Nummi.Core.App;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Strategies; 

public class OpportunistParameters {
    public required HashSet<string> Symbols { get; init; }
    public override string ToString() => this.ToFormattedString();
}

public class OpportunistState {
    public int Counter { get; set; } = 10;
}

public class OpportunistStrategy : IStrategyLogicBuiltin {
    public Guid Id => Guid.Parse("1e2cfa596f8544e7820fc989f28ee570");
    public string Name => "Opportunist";
    public StrategyFrequency Frequency => StrategyFrequency.OneMinute;
    public Type ParameterType => typeof(OpportunistParameters);
    public Type StateType => typeof(OpportunistState);

    public void Initialize(IStrategyContext ctx) {
        var now = ctx.Clock.NowUtc;
        ctx.LogInfo($"Initializing (nowUtc={now.ToString().Yellow()} now={now.ToLocalTime().ToString().Yellow()})");
        
        OpportunistParameters parameters = ctx.GetParameters<OpportunistParameters>();

        // var bars = ctx.GetBars(
        //     symbols: parameters.Symbols, 
        //     dateRange: new DateRange(now - TimeSpan.FromSeconds(1), now), 
        //     period: Period.Second
        // );
        //
        foreach (var symbol in parameters.Symbols) {
            ctx.LogInfo($"{symbol.Red()}");
        }
    }
    
    public void CheckForTrades(IStrategyContext ctx) {
        OpportunistState state = ctx.GetState<OpportunistState>();
        state.Counter += 1;
        ctx.LogInfo($"Checking For Trades: {ctx.Clock.Now.ToString().Yellow()} / {ctx.Clock.NowUtc.ToString().Yellow()}");
    }
}