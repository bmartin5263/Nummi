namespace Nummi.Core.Domain.New;
public class Bar : IComparable<Bar> {

    public string Symbol { get; private init; }
    public DateTimeOffset OpenTime { get; private init; }
    public TimeSpan Period { get; private init; }
    public decimal Open { get; private init; }
    public decimal High { get; private init; }
    public decimal Low { get; private init; }
    public decimal Close { get; private init; }
    public decimal Volume { get; private init; }

    public Bar(string symbol, DateTimeOffset openTime, TimeSpan period, decimal open, decimal high, decimal low, decimal close, decimal volume) {
        Symbol = symbol;
        OpenTime = openTime;
        Period = period;
        Open = open;
        High = high;
        Low = low;
        Close = close;
        Volume = volume;
    }
    
    protected bool Equals(Bar other) {
        return Symbol == other.Symbol 
               && OpenTime == other.OpenTime
               && Period == other.Period;
    }

    public int CompareTo(Bar? other) {
        return OpenTime.CompareTo(other!.OpenTime);
    }

    public override bool Equals(object? obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Bar)obj);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Symbol, OpenTime, Period);
    }

    public override string ToString() {
        return $"{nameof(Symbol)}: {Symbol}, OpenTimeUtc: {OpenTime}, Period: {Period}, {nameof(Open)}: {Open}, {nameof(High)}: {High}, {nameof(Low)}: {Low}, {nameof(Close)}: {Close}, {nameof(Volume)}: {Volume}";
    }
}