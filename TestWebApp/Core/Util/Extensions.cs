using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace TestWebApp.Core.Util; 

public static class Extensions {
    
    public static DateTime ToDateTime(this DateOnly? dateOnly, DateTime ifNull) {
        if (dateOnly == null) {
            return ifNull;
        }
        var d = (DateOnly) dateOnly;
        return new DateTime(d.Year, d.Month, d.Day);
    }

}
