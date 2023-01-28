using Nummi.Core.Exceptions;

namespace Nummi.Core.Util; 

public interface IClock {
    public DateTime Now { get; }
    public DateTime NowUtc { get; }
}

public class ClockLive : IClock {
    public DateTime Now => DateTime.Now;
    public DateTime NowUtc => DateTime.UtcNow;
}

public class ClockMock : IClock {
    
    public DateTime Now => NowUtc.ToLocalTime();
    public DateTime NowUtc { get; set; }

    public ClockMock(DateTime time) {
        if (time.Kind != DateTimeKind.Utc) {
            throw new InvalidArgumentException("Clock mock must use UTC");
        }
        NowUtc = time;
    }

    public void ChangeTime(Func<DateTime, DateTime> func) {
        NowUtc = func(NowUtc);
    }
}