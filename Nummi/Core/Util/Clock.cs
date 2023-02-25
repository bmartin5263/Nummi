namespace Nummi.Core.Util; 

public interface IClock {
    public DateTimeOffset Now { get; }
    public DateTimeOffset NowUtc { get; }
}

public class ClockLive : IClock {
    public DateTimeOffset Now => DateTimeOffset.Now;
    public DateTimeOffset NowUtc => DateTimeOffset.UtcNow;
}

public class ClockMock : IClock {
    
    public DateTimeOffset Now => NowUtc.ToLocalTime();
    public DateTimeOffset NowUtc { get; set; }

    public ClockMock() {
        NowUtc = DateTimeOffset.UtcNow;
    }
    
    public ClockMock(DateTimeOffset time) {
        NowUtc = time;
    }

    public void ChangeTime(Func<DateTimeOffset, DateTimeOffset> func) {
        NowUtc = func(NowUtc);
    }
}