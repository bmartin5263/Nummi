using NLog;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies.Opportunist; 

public class OpportunistStrategy : 
    Strategy, 
    IParameterizedStrategy<OpportunistStrategy.OpportunistParameters> 
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public class OpportunistParameters {
        public ISet<string> Symbols { get; set; } = new HashSet<string>();
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
        
        List<HistoricalPrice> prices = env.BinanceClientAdapter.GetSpotPrice(Parameters!.Symbols).ToList();
        foreach (var symbol in Parameters.Symbols) {
            IEnumerable<HistoricalMinuteCandlestick> candlesticks = env.BinanceClientAdapter.GetMinuteCandlestick(symbol);
            env.AppDb.HistoricalMinuteCandlesticks.AddRange(candlesticks);
        }

        env.AppDb.HistoricalPrices.AddRange(prices);

        return new Result();
    }
    
    private void Message(string msg) {
        Log.Info($"{Id} - {msg}");
    }
}