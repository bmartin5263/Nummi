using System.ComponentModel;

namespace Nummi.Core.Domain.Strategies; 

public record StrategyFrequency {
    public static StrategyFrequency OneMinute { get; } = new(TimeSpan.FromMinutes(1));
    public static StrategyFrequency ThreeMinutes { get; } = new(TimeSpan.FromMinutes(3));
    public static StrategyFrequency FiveMinutes { get; } = new(TimeSpan.FromMinutes(5));
    public static StrategyFrequency TenMinutes { get; } = new(TimeSpan.FromMinutes(10));
    public static StrategyFrequency ThirtyMinutes { get; } = new(TimeSpan.FromMinutes(30));
    public static StrategyFrequency OneHour { get; } = new(TimeSpan.FromHours(1));
    public static StrategyFrequency SixHours { get; } = new(TimeSpan.FromHours(6));
    public static StrategyFrequency OneDay { get; } = new(TimeSpan.FromDays(1));
    public static StrategyFrequency[] Values { get; } = {
        OneMinute, ThreeMinutes, FiveMinutes, TenMinutes, ThirtyMinutes, OneHour,
        SixHours, OneDay
    };
    
    public TimeSpan AsTimeSpan { get; }
    
    private StrategyFrequency(TimeSpan asTimeSpan) {
        AsTimeSpan = asTimeSpan;
    }

    public static StrategyFrequency FromTimeSpan(TimeSpan timeSpan) {
        foreach (var frequency in Values) {
            if (frequency.AsTimeSpan == timeSpan) {
                return frequency;
            }
        }
        throw new InvalidEnumArgumentException($"No StrategyFrequency for given timespan {timeSpan}");
    }
    
    public override string ToString() {
        return AsTimeSpan.ToString();
    }

    public int CompareTo(StrategyFrequency? other) {
        return AsTimeSpan.CompareTo(other!.AsTimeSpan);
    }
    
    public static bool operator <(StrategyFrequency lhs, StrategyFrequency rhs) {
        return lhs.CompareTo(rhs) < 0;
    }
    
    public static bool operator >(StrategyFrequency lhs,StrategyFrequency rhs) {
        return lhs.CompareTo(rhs) > 0;
    }
    
    public static bool operator <=(StrategyFrequency lhs,StrategyFrequency rhs) {
        return lhs.CompareTo(rhs) <= 0;
    }
    
    public static bool operator >=(StrategyFrequency lhs,StrategyFrequency rhs) {
        return lhs.CompareTo(rhs) >= 0;
    }
}