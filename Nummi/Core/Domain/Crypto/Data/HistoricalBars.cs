using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Data; 

public class HistoricalBars {
    public TimeSpan TimeSpan { get; }
    public DateTime EndTime { get; }
    public IDictionary<string, IEnumerable<MinuteBar>> Bars { get; }

    public HistoricalBars(TimeSpan timeSpan, DateTime endTime, IDictionary<string, IEnumerable<MinuteBar>> bars) {
        TimeSpan = timeSpan;
        EndTime = endTime;
        Bars = bars;
    }

    public override string ToString() {
        return this.ToFormattedString();
    }
}