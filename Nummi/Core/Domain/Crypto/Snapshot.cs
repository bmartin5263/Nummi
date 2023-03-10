using System.Diagnostics.CodeAnalysis;

namespace Nummi.Core.Domain.Crypto; 

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")] // used by serializer
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class Snapshot {
    
    public string Symbol { get; }
    public Quote? Quote { get; }
    public Trade? Trade { get; }
    public Bar? MinuteBar { get; }
    public Bar? CurrentDailyBar { get; }
    public Bar? PreviousDailyBar { get; }

    public Snapshot(string symbol, Quote? quote, Trade? trade, Bar? minuteBar, Bar? currentDailyBar, Bar? previousDailyBar) {
        Symbol = symbol;
        Quote = quote;
        Trade = trade;
        MinuteBar = minuteBar;
        CurrentDailyBar = currentDailyBar;
        PreviousDailyBar = previousDailyBar;
    }
}