using System.Diagnostics;

namespace TestWebApp.Core.Util; 

public static class Assert {

    public static void NotNull<T>(T obj) where T : class {
        Debug.Assert(obj != null);
    }
    
}