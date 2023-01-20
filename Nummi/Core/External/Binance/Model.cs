using Nummi.Core.Util;

namespace Nummi.Core.External.Binance; 

public class ExchangeInfo {
    public string? Timezone { get; set; }
    public long? ServerTime { get; set; }
    public IEnumerable<RateLimit> RateLimits { get; set; } = new List<RateLimit>();

    public override string ToString() {
        return this.ToFormattedString();
    }
}

public class RateLimit {
    public string? RateLimitType { get; set; }
    public string? Interval { get; set; }
    public int? IntervalNum { get; set; }
    public int? Limit { get; set; }
    
    public override string ToString() {
        return this.ToFormattedString();
    }
}