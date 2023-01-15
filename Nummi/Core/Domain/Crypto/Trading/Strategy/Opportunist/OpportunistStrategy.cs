using System.ComponentModel.DataAnnotations.Schema;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Trading.Strategy.Opportunist; 

public class OpportunistStrategy : 
    TradingStrategy, 
    IParameterizedStrategy<OpportunistStrategy.OpportunistParameters> 
{
    public class OpportunistParameters {
        public string? SomeData { get; set; }
        public int? Number { get; set; }
        public override string ToString() => this.ToFormattedString();
    }
    [Column("OpportunistParameters")] 
    public OpportunistParameters? Parameters { get; set; }

    public class OpportunistState {
        public int Counter { get; set; }
        public OpportunistState(int counter) { Counter = counter; }
        public override string ToString() => this.ToFormattedString();
    }
    [Column("OpportunistState")]
    public OpportunistState? State { get; set; }
    
    public Type ParameterObjectType => typeof(OpportunistParameters);

    public OpportunistStrategy() : base(TimeSpan.FromMinutes(1)) { }

    public void AcceptParameters(OpportunistParameters parameters) {
        Parameters = parameters;
    }

    public override IDictionary<string, object?> GetStateMap() {
        return new Dictionary<string, object?> {
            { "state", State },
            { "parameters", Parameters }
        };
    }

    protected override void DoInitialize(TradingContext env) {
        Parameters.ThrowIfNull(() => new ArgumentException("Missing Parameters"));
        State = new OpportunistState(10);
        Message("Initialized");
    }

    protected override Result DoCheckForTrades(TradingContext env) {
        Parameters.ThrowIfNull(() => new ArgumentException("Missing Parameters"));
        Message("Checking For Trades");
        Message($"Parameters: {Parameters}");
        Message($"State: {State}");
        ++State!.Counter;
        ++Parameters!.Number;
        return new Result();
    }
    
    private void Message(string msg) {
        Console.WriteLine($"{nameof(OpportunistStrategy)} - {msg}");
    }
}