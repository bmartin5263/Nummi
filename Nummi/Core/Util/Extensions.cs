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

}
