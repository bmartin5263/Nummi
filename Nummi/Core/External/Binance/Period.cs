using Nummi.Core.Domain.Common;

namespace Nummi.Core.External.Binance; 

public class Period {
    
    public static Period Second { get; } = new(
        time: TimeSpan.FromSeconds(1), 
        intervalParam: "1s", 
        barCountExtractor: r => (long) r.Duration.TotalSeconds
    );
    
    public static Period Minute { get; } = new(
        time: TimeSpan.FromMinutes(1), 
        intervalParam: "1m", 
        barCountExtractor: r => (long) r.Duration.TotalMinutes
    );
    
    public static Period Day { get; } = new(
        time: TimeSpan.FromDays(1), 
        intervalParam: "1d", 
        barCountExtractor: r => (long) r.Duration.TotalDays
    );
    
    public TimeSpan Time { get; }
    public long UnixMs => (long) Time.TotalMilliseconds;
    public string IntervalParam { get; }
    private Func<DateRange, long> BarCountExtractor { get; }

    private Period(TimeSpan time, string intervalParam, Func<DateRange, long> barCountExtractor) {
        Time = time;
        IntervalParam = intervalParam;
        BarCountExtractor = barCountExtractor;
    }

    public BarCalls CalculateBarCalls(DateRange dateRange, double maxBarsPerCall) {
        var totalBars = BarCountExtractor(dateRange);
        Chunk(totalBars, maxBarsPerCall, out uint chunks, out uint remainder);
        return new BarCalls(dateRange, chunks, remainder);
    }
    
    private void Chunk(long value, double maxPerChunk, out uint chunks, out uint remainder) {
        chunks = (uint)(value / maxPerChunk);
        remainder = (uint)(value % (long) maxPerChunk);
    }
}

public class BarCalls {
    public DateRange DateRange { get; }
    public uint Chunks { get; }
    public uint Remainder { get; }

    public BarCalls(DateRange dateRange, uint chunks, uint remainder) {
        DateRange = dateRange;
        Chunks = chunks;
        Remainder = remainder;
    }

    public long TotalBars(uint barsPerChunk) {
        return Chunks * barsPerChunk + Remainder;
    }

    public int CallCount() {
        return (int)(Chunks + (Remainder > 0 ? 1 : 0));
    }

    public override string ToString() {
        return $"{nameof(DateRange)}: {DateRange}, {nameof(Chunks)}: {Chunks}, {nameof(Remainder)}: {Remainder}";
    }
}