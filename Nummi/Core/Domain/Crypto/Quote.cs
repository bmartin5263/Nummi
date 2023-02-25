using System.Diagnostics.CodeAnalysis;

namespace Nummi.Core.Domain.Crypto;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")] // used by serializer
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class Quote {
    
    public string Symbol { get; }
    public DateTime TimestampUtc { get; }
    public string BidExchange { get; }
    public string AskExchange { get; }
    public decimal BidPrice { get; }
    public decimal AskPrice { get; }
    public decimal BidSize { get; }
    public decimal AskSize { get; }
    public string Tape { get; }

    public Quote(string symbol, DateTime timestampUtc, string bidExchange, string askExchange, decimal bidPrice, decimal askPrice, decimal bidSize, decimal askSize, string tape) {
        Symbol = symbol;
        TimestampUtc = timestampUtc;
        BidExchange = bidExchange;
        AskExchange = askExchange;
        BidPrice = bidPrice;
        AskPrice = askPrice;
        BidSize = bidSize;
        AskSize = askSize;
        Tape = tape;
    }
}