using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Data; 

[Table("Historical" + nameof(MinuteBar))]
[PrimaryKey(nameof(Symbol), nameof(OpenTimeEpoch))]
public class MinuteBar : IBar{
    public string Symbol { get; set; }
    public long OpenTimeEpoch { get; set; }
    public DateTime OpenTimeUtc { get; set; }

    [NotMapped]
    public DateTime OpenTimeLocal => OpenTimeUtc.ToLocalTime();
    
    [NotMapped]
    public DateTime CloseTimeUtc { get; set; }

    [NotMapped]
    public DateTime CloseTimeLocal => CloseTimeUtc.ToLocalTime();
    
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal Volume { get; set; }

    private MinuteBar() {
        Symbol = "";
        OpenTimeUtc = DateTime.MinValue;
    }

    public MinuteBar(string symbol, long openTimeEpoch, long closeTimeEpoch, decimal open, decimal high, decimal low, decimal close, decimal volume) {
        Symbol = symbol;
        OpenTimeEpoch = openTimeEpoch;
        OpenTimeUtc = DateTimeOffset.FromUnixTimeMilliseconds(openTimeEpoch).DateTime;
        CloseTimeUtc = DateTimeOffset.FromUnixTimeMilliseconds(closeTimeEpoch).DateTime;
        Open = open;
        High = high;
        Low = low;
        Close = close;
        Volume = volume;
    }

    public override string ToString() {
        return this.ToFormattedString();
    }
}