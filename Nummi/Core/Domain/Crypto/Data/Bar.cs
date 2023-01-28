using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Data; 

[Table("Historical" + nameof(Bar))]
[PrimaryKey(nameof(Symbol), nameof(OpenTimeUnixMs), nameof(PeriodMs))]
public class Bar : IBar, IComparable<Bar> {

    public string Symbol { get; init; }

    private readonly long _openTimeUnixMs;
    public long OpenTimeUnixMs {
        get => _openTimeUnixMs;
        init {
            _openTimeUnixMs = value;
            _openTimeUtc = value.ToUtcDateTime();
        }
    }
    
    public long PeriodMs { get; init; }
    
    private readonly DateTime _openTimeUtc;
    public DateTime OpenTimeUtc {
        get => _openTimeUtc;
        init {
            _openTimeUtc = value;
            _openTimeUnixMs = value.ToUnixTimeMs();
        }
    }

    public decimal Open { get; init; }
    public decimal High { get; init; }
    public decimal Low { get; init; }
    public decimal Close { get; init; }
    public decimal Volume { get; init; }

    public TimeSpan Period => TimeSpan.FromMilliseconds(PeriodMs);

    private Bar() {
        Symbol = "$$$";
    }

    public Bar(string symbol, long openTimeUnixMs, long periodMs, decimal open, decimal high, decimal low, decimal close, decimal volume) {
        Symbol = symbol;
        OpenTimeUnixMs = openTimeUnixMs;
        PeriodMs = periodMs;
        Open = open;
        High = high;
        Low = low;
        Close = close;
        Volume = volume;
    }
    
    protected bool Equals(Bar other) {
        return Symbol == other.Symbol && OpenTimeUnixMs == other.OpenTimeUnixMs;
    }

    public int CompareTo(Bar? other) {
        return OpenTimeUnixMs.CompareTo(other?.OpenTimeUnixMs);
    }

    public override bool Equals(object? obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Bar)obj);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Symbol, OpenTimeUnixMs);
    }

    public override string ToString() {
        return $"{nameof(Symbol)}: {Symbol}, OpenTimeUtc: {OpenTimeUtc}, Period: {Period}, {nameof(Open)}: {Open}, {nameof(High)}: {High}, {nameof(Low)}: {Low}, {nameof(Close)}: {Close}, {nameof(Volume)}: {Volume}";
    }
}