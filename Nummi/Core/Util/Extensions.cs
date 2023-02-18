using Nummi.Core.Domain.Common;
using Nummi.Core.Exceptions;

namespace Nummi.Core.Util; 

public static class Extensions {
    
    public static T OrElseThrow<T>(this T? self, Func<Exception> supplier) {
        if (self == null) {
            throw supplier.Invoke();
        }
        return self;
    }

    public static T ThrowIfNull<T>(this T? self, Func<Exception> supplier) {
        return OrElseThrow(self, supplier);
    }

    public static Ksuid ToKsuid(this string self) {
        return Ksuid.FromString(self);
    }

    // https://stackoverflow.com/questions/1004698/how-to-truncate-milliseconds-off-of-a-net-datetime
    public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan) {
        if (timeSpan == TimeSpan.Zero) {
            throw new InvalidUserArgumentException("Timespan cannot be Zero for Truncate");
        }
        if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue) {
            return dateTime; // do not modify "guard" values
        }
        return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
    }

    public static DateRange Truncate(this DateRange dateRange, TimeSpan timeSpan) {
        return new DateRange(dateRange.Start.Truncate(timeSpan), dateRange.End.Truncate(timeSpan));
    }

    // https://stackoverflow.com/questions/1004698/how-to-truncate-milliseconds-off-of-a-net-datetime
    public static DateTimeOffset Truncate(this DateTimeOffset dateTime, TimeSpan timeSpan) {
        if (timeSpan == TimeSpan.Zero) {
            throw new InvalidUserArgumentException("Timespan cannot be Zero for Truncate");
        }
        if (dateTime == DateTimeOffset.MinValue || dateTime == DateTimeOffset.MaxValue) {
            return dateTime; // do not modify "guard" values
        }
        return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
    }

    public static DateTimeOffset ToUtcDateTime(this long unixMs) {
        DateTimeOffset epoch = DateTimeOffset.UnixEpoch;
        return epoch.AddMilliseconds(unixMs);
    }
}
