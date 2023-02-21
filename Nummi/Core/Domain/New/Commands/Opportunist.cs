using JetBrains.Annotations;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Exceptions;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New.Commands; 

[UsedImplicitly]
public class OpportunistParameters {
    [UsedImplicitly]
    public HashSet<string> Symbols { get; init; } = new();
    public override string ToString() => this.ToFormattedString();
}

[UsedImplicitly]
public class OpportunistState {
    
}

public class OpportunistStrategy : IStrategyLogicBuiltin<OpportunistParameters, OpportunistState> {
    public string Name => "Opportunist";
    public TimeSpan Frequency => TimeSpan.FromMinutes(1);

    public void Initialize(ITradingContext<OpportunistParameters, OpportunistState> ctx) {
        if (ctx.Parameters == null) {
            throw new InvalidSystemStateException("Parameters is null");
        }
        if (ctx.State == null) {
            throw new InvalidSystemStateException("Symbols cannot be null");
        }

        var now = ctx.Clock.NowUtc;
        Console.WriteLine($"Initializing (nowUtc={now.ToString().Yellow()} now={now.ToLocalTime().ToString().Yellow()})");
        var bars = ctx.GetBars(
            symbols: ctx.Parameters.Symbols, 
            dateRange: new DateRange(now - TimeSpan.FromMinutes(60), now), 
            Period.Second
        );
        
        foreach (var symbol in ctx.Parameters.Symbols) {
            var symbolBars = bars[symbol];
            DateTimeOffset minDate = symbolBars[0].OpenTime;
            DateTimeOffset maxDate = symbolBars[^1].OpenTime;
            Console.WriteLine($"{symbol.Red()}: {minDate.ToLocalTime().ToString().Yellow()} - {maxDate.ToLocalTime().ToString().Yellow()}");
        }
    }
    
    public void CheckForTrades(ITradingContext<OpportunistParameters, OpportunistState> ctx) {
        if (ctx.Parameters!.Symbols == null) {
            throw new InvalidUserArgumentException("Symbols cannot be null");
        }
        Console.WriteLine($"Checking For Trades: {ctx.Clock.Now.ToString().Yellow()} / {ctx.Clock.NowUtc.ToString().Yellow()}");
    }
}