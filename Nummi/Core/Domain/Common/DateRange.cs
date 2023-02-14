using Nummi.Core.Exceptions;

namespace Nummi.Core.Domain.Common; 

public struct DateRange {
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public TimeSpan Duration => End - Start;

    public DateRange(DateTimeOffset start, DateTimeOffset end) {
        if (start > end) {
            throw new InvalidSystemArgumentException($"'start' ({start}) cannot be after 'end' ({end})");
        }
        Start = start;
        End = end;
    }

    public override string ToString() {
        return $"DateRange(start={Start.ToLocalTime()}, end={End.ToLocalTime()})";
    }
}