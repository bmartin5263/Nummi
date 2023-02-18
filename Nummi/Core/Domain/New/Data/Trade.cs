using System.Diagnostics.CodeAnalysis;
using Alpaca.Markets;

namespace Nummi.Core.Domain.New.Data; 

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")] // used by serializer
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class Trade {
    
    public string Symbol { get; }
    public DateTime TimestampUtc { get; }
    public decimal Price { get; }
    public decimal Size { get; }
    public ulong TradeId { get; }
    public string Exchange { get; }
    public string Tape { get; }
    public string Update { get; }
    public IReadOnlyList<string> Conditions { get; }
    public TakerSide TakerSide { get; }

    public Trade(string symbol, DateTime timestampUtc, decimal price, decimal size, ulong tradeId, string exchange, string tape, string update, IReadOnlyList<string> conditions, TakerSide takerSide) {
        Symbol = symbol;
        TimestampUtc = timestampUtc;
        Price = price;
        Size = size;
        TradeId = tradeId;
        Exchange = exchange;
        Tape = tape;
        Update = update;
        Conditions = conditions;
        TakerSide = takerSide;
    }
}