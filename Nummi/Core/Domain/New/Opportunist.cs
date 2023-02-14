using Nummi.Core.Domain.Common;
using Nummi.Core.Exceptions;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New; 

public class OpportunistParameters {
    public ISet<string> Symbols { get; init; } = new HashSet<string>();
    public override string ToString() => this.ToFormattedString();
}

public class OpportunistState {
    
}

public class OpportunistStrategy : IStrategyImpl<OpportunistParameters, OpportunistState> {
    
    public void Initialize(ITradingContext ctx, OpportunistParameters? parameters, ref OpportunistState? state) {
        if (parameters == null) {
            throw new InvalidStateException("Parameters is null");
        }
        if (parameters.Symbols == null) {
            throw new InvalidStateException("Symbols cannot be null");
        }

        var now = ctx.Clock.NowUtc;
        Console.WriteLine($"Initializing (nowUtc={now.ToString().Yellow()} now={now.ToLocalTime().ToString().Yellow()})");
        var bars = ctx.GetBars(
            symbols: parameters.Symbols, 
            dateRange: new DateRange(now - TimeSpan.FromMinutes(60), now), 
            Period.Second
        );
        
        foreach (var symbol in parameters.Symbols) {
            var symbolBars = bars[symbol];
            DateTimeOffset minDate = symbolBars[0].OpenTime;
            DateTimeOffset maxDate = symbolBars[^1].OpenTime;
            Console.WriteLine($"{symbol.Red()}: {minDate.ToLocalTime().ToString().Yellow()} - {maxDate.ToLocalTime().ToString().Yellow()}");
        }
    }
    
    public void CheckForTrades(ITradingContext ctx, OpportunistParameters? parameters, ref OpportunistState? state) {
        if (parameters!.Symbols == null) {
            throw new InvalidUserArgumentException("Symbols cannot be null");
        }
        Console.WriteLine($"Checking For Trades: {ctx.Clock.Now.ToString().Yellow()} / {ctx.Clock.NowUtc.ToString().Yellow()}");
    }
}