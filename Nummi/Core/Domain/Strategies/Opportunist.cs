using JetBrains.Annotations;
using Nummi.Core.App;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Strategies; 

[UsedImplicitly]
public class OpportunistParameters {
    [UsedImplicitly]
    public required HashSet<string> Symbols { get; init; }
    public override string ToString() => this.ToFormattedString();
}

[UsedImplicitly]
public class OpportunistState {
    public int Counter { get; set; } = 10;
}

public class OpportunistStrategy : IStrategyLogicBuiltin {
    public Ksuid Id => Ksuid.FromString("2M1waaJZigKETPHIgF7kaZK342l");
    public string Name => "Opportunist";
    public TimeSpan Frequency => TimeSpan.FromMinutes(1);
    public Type ParameterType => typeof(OpportunistParameters);
    public Type StateType => typeof(OpportunistState);

    public void Initialize(IStrategyContext ctx) {
        OpportunistParameters parameters = ctx.GetParameters<OpportunistParameters>();

        var now = ctx.Clock.NowUtc;
        Console.WriteLine($"Initializing (nowUtc={now.ToString().Yellow()} now={now.ToLocalTime().ToString().Yellow()})");
        var bars = ctx.GetBars(
            symbols: parameters.Symbols, 
            dateRange: new DateRange(now - TimeSpan.FromMinutes(60), now), 
            period: Period.Second
        );
        
        foreach (var symbol in parameters.Symbols) {
            Console.WriteLine($"{symbol.Red()}");
        }
    }
    
    public void CheckForTrades(IStrategyContext ctx) {
        OpportunistState state = ctx.GetState<OpportunistState>();
        state.Counter += 1;
        Console.WriteLine($"Checking For Trades: {ctx.Clock.Now.ToString().Yellow()} / {ctx.Clock.NowUtc.ToString().Yellow()}");
    }
}