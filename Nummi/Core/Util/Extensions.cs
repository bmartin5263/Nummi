using KSUID;

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

    public static DateTime Round(this DateTime date, TimeSpan span) {
        long ticks = (date.Ticks + (span.Ticks / 2) + 1)/ span.Ticks;
        return new DateTime(ticks * span.Ticks);
    }
    
    public static DateTime Floor(this DateTime date, TimeSpan span) {
        long ticks = (date.Ticks / span.Ticks);
        return new DateTime(ticks * span.Ticks);
    }
    
    public static DateTime Ceil(this DateTime date, TimeSpan span) {
        long ticks = (date.Ticks + span.Ticks - 1) / span.Ticks;
        return new DateTime(ticks * span.Ticks);
    }

    public static DateTime UtcToLocalTime(this DateTime timeUtc) {
        return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, TimeZoneInfo.Local);
    }
}
