using System.Diagnostics.CodeAnalysis;

namespace Nummi.Core.Domain.Crypto.Data; 

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")] // used by serializer
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class Bar : IBar {
    
    public string Symbol { get; }
    public DateTime TimeUtc { get; }
    public decimal Open { get; }
    public decimal High { get; }
    public decimal Low { get; }
    public decimal Close { get; }
    public decimal Volume { get; }
    public decimal Vwap { get; }
    public ulong TradeCount { get; }

    public Bar(string symbol, DateTime timeUtc, decimal open, decimal high, decimal low, decimal close, decimal volume, decimal vwap, ulong tradeCount) {
        Symbol = symbol;
        TimeUtc = timeUtc;
        Open = open;
        High = high;
        Low = low;
        Close = close;
        Volume = volume;
        Vwap = vwap;
        TradeCount = tradeCount;
    }
}