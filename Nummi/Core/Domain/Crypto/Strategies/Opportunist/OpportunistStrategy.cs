using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies.Opportunist; 

public class OpportunistStrategy : 
    Strategy, 
    IParameterizedStrategy<OpportunistStrategy.OpportunistParameters> 
{
    public class OpportunistParameters {
        public IList<string> Symbols { get; set; } = new List<string>();
        public override string ToString() => this.ToFormattedString();
    }
    public OpportunistParameters? Parameters { get; set; }

    public class OpportunistState {
        public IDictionary<string, IList<HistoricalPrice>> HistoricalPrices { get; } =
            new Dictionary<string, IList<HistoricalPrice>>();
        public override string ToString() => this.ToFormattedString();
    }
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
        State = new OpportunistState();
        Message("Initialized");
    }

    protected override Result DoCheckForTrades(TradingContext env) {
        Parameters.ThrowIfNull(() => new ArgumentException("Missing Parameters"));
        Message("Checking For Trades");

        throw new ArithmeticException("wtf");

        var prices = env.BinanceClient.GetSpotPrice(Parameters!.Symbols).ToList();
        env.AppDb.HistoricalPrices.AddRange(prices);
        foreach (var price in prices) {
            if (!State!.HistoricalPrices.ContainsKey(price.Symbol)) {
                var list = new List<HistoricalPrice> { price };
                State.HistoricalPrices[price.Symbol] = list;
            }
            else {
                State.HistoricalPrices[price.Symbol].Add(price);
            }
        }
        
        return new Result();
    }
    
    private void Message(string msg) {
        Console.WriteLine($"{nameof(OpportunistStrategy)} - {msg}");
    }
}