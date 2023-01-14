using System.ComponentModel.DataAnnotations.Schema;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Trading.Strategy.Opportunist; 

public class OpportunistStrategy : TradingStrategy, IParameterizedStrategy<OpportunistParameters> {

    [Column("OpportunistParameters")]
    public OpportunistParameters? Parameters { get; set; }

    [Column("OpportunistState")] 
    public OpportunistState State { get; private set;  } = new("lololol", 69);

    public OpportunistStrategy() : base(TimeSpan.FromMinutes(1)) {
    }
    
    protected override void DoInitialize(TradingContext env) {
        Parameters.ThrowIfNull(() => new ArgumentException("Missing Parameters"));
    }

    protected override Result DoCheckForTrades(TradingContext env) {
        Parameters.ThrowIfNull(() => new ArgumentException("Missing Parameters"));
        return new Result();
    }

    public Type ParameterObjectType => typeof(OpportunistParameters);
    
    public void AcceptParameters(OpportunistParameters parameters) {
        Parameters = parameters;
    }

    public override IDictionary<string, object?> GetStateMap() {
        return new Dictionary<string, object?> {
            { "state", State },
            { "parameters", Parameters }
        };
    }
}