using System.Diagnostics;

namespace Nummi.Core.Util; 

public static class Assertions {

    public static void Assert(bool condition) {
        Debug.Assert(condition);
    }
    
}