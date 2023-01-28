using Nummi.Core.Domain.Common;
using Nummi.Core.Exceptions;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies.Opportunist; 

public class OpportunistStrategy : Strategy, IParameterizedStrategy<OpportunistParameters> {
    
    public ISet<string>? Symbols { get; private set; }
    
    public Type ParameterObjectType => typeof(OpportunistParameters);
    public OpportunistParameters Parameters => new() {
        Symbols = Symbols
    };

    public OpportunistStrategy() : base(TimeSpan.FromMinutes(1)) {
        
    }

    public void AcceptParameters(OpportunistParameters parameters) {
        Symbols = parameters.Symbols;
    }

    protected override void Initialize(StrategyContext ctx) {
        if (Symbols == null) {
            throw new InvalidArgumentException("Symbols cannot be null");
        }

        var now = ctx.Clock.NowUtc;
        Message($"Initializing (nowUtc={now.ToString().Yellow()} now={now.ToLocalTime().ToString().Yellow()})");
        var bars = ctx.DataClient.GetBars(
            symbols: Symbols, 
            dateRange: new DateRange(now - TimeSpan.FromSeconds(10), now + TimeSpan.FromSeconds(5)), 
            Period.Second
        );
        
        foreach (var symbol in Symbols) {
            var symbolBars = bars[symbol];
            DateTime minDate = symbolBars.Min(t => t.OpenTimeUtc);
            DateTime maxDate = symbolBars.Max(t => t.OpenTimeUtc);
            Message($"{symbol.Red()}: {minDate.ToLocalTime().ToString().Yellow()} - {maxDate.ToLocalTime().ToString().Yellow()}");
        }
    }
    
    protected override void CheckForTrades(StrategyContext ctx) {
        if (Symbols == null) {
            throw new InvalidArgumentException("Symbols cannot be null");
        }
        Message($"Checking For Trades: {ctx.Clock.Now.ToString().Yellow()} / {ctx.Clock.NowUtc.ToString().Yellow()}");
    }
}

public class OpportunistParameters {
    public ISet<string>? Symbols { get; init; } = new HashSet<string>();
    public override string ToString() => this.ToFormattedString();
}