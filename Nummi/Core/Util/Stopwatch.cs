using NLog;

namespace Nummi.Core.Util; 

public struct Stopwatch {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private DateTimeOffset StartTime { get; set; }

    public void Tick() {
        StartTime = DateTimeOffset.Now;
    }

    public TimeSpan Tock() {
        return DateTimeOffset.Now - StartTime;
    }

    public static Stopwatch Started() {
        var sw = new Stopwatch();
        sw.Tick();
        return sw;
    }

    public static void Time(string name, Action action) {
        var stopwatch = Started();
        stopwatch.Tick();
        action();
        var elapsed = stopwatch.Tock();
        Log.Info($"{name} completed in {elapsed}");
    }
    
    public static T Time<T>(string name, Func<T> func) {
        var stopwatch = Started();
        stopwatch.Tick();
        var result = func();
        var elapsed = stopwatch.Tock();
        Log.Info($"{name} completed in {elapsed}");
        return result;
    }
}