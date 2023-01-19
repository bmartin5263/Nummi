using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies.Opportunist; 

public class OpportunistStrategy : Strategy, IParameterizedStrategy<OpportunistParameters> {
    
    public ISet<string>? Symbols { get; private set; }
    
    public Type ParameterObjectType => typeof(OpportunistParameters);
    public OpportunistParameters Parameters => new() {
        Symbols = Symbols!
    };

    public OpportunistStrategy() : base(TimeSpan.FromMinutes(1)) {
        
    }

    public void AcceptParameters(OpportunistParameters parameters) {
        Symbols = parameters.Symbols;
    }

    protected override void OnInitialize(TradingContext env) {
        Message("Initialized");
    }
    
    // protected override void OnClockLeapDetected(TimeSpan overtime) ?

    protected override void CheckForTrades(TradingInterface env) {
        if (Symbols == null) {
            throw new ArgumentException("Symbols cannot be null");
        }
        Message("Checking For Trades");
    }
}

public class OpportunistParameters {
    public ISet<string>? Symbols { get; init; } = new HashSet<string>();
    public override string ToString() => this.ToFormattedString();
}