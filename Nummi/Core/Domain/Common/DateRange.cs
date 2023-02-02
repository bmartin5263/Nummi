using Nummi.Core.Exceptions;

namespace Nummi.Core.Domain.Common; 

public struct DateRange {
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public TimeSpan Duration => End - Start;

    public DateRange(DateTime start, DateTime end) {
        if (start > end) {
            throw new InvalidSystemArgumentException($"'start' ({start}) cannot be after 'end' ({end})");
        }
        if (start.Kind != end.Kind) {
            throw new InvalidSystemArgumentException($"'start' kind ({start.Kind}) != 'end' kind ({end.Kind})");
        }
        Start = start;
        End = end;
    }

    public override string ToString() {
        return $"DateRange(start={Start.ToLocalTime()}, end={End.ToLocalTime()})";
    }
}