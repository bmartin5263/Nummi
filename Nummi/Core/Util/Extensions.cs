using KSUID;
using Nummi.Core.Domain.Common;
using Nummi.Core.Exceptions;

namespace Nummi.Core.Util; 

public static class Extensions {
    
    public static DateTime ToDateTime(this DateOnly? dateOnly, DateTime ifNull) {
        if (dateOnly == null) {
            return ifNull;
        }
        var d = (DateOnly) dateOnly;
        return new DateTime(d.Year, d.Month, d.Day);
    }

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
            throw new InvalidArgumentException("Timespan cannot be Zero for Truncate");
        }
        if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue) {
            return dateTime; // do not modify "guard" values
        }
        return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
    }

    public static DateRange Truncate(this DateRange dateRange, TimeSpan timeSpan) {
        return new DateRange(dateRange.Start.Truncate(timeSpan), dateRange.End.Truncate(timeSpan));
    }

    public static R? MaxOrDefault<T, R>(this ICollection<T> source, Func<T, R> selector) {
        return source.Count == 0 ? default : source.Max(selector);
    }

    public static SortedSet<T> ToSortedSet<T>(this IEnumerable<T> source) {
        return new SortedSet<T>(source);
    }

    public static long ToUnixTimeMs(this DateTime time) {
        return ((DateTimeOffset)time).ToUnixTimeMilliseconds();
    }
    
    public static DateTime ToUtcDateTime(this long unixMs) {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddMilliseconds(unixMs);
        return dateTime;
    }
}
