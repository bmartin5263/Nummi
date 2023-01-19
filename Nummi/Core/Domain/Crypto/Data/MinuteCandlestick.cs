using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Data; 

[Table("Historical" + nameof(MinuteCandlestick))]
[PrimaryKey(nameof(Symbol), nameof(OpenTimeEpoch))]
public class MinuteCandlestick {
    public string Symbol { get; set; }
    public long OpenTimeEpoch { get; set; }
    public DateTime OpenTime { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal Volume { get; set; }

    private MinuteCandlestick() {
        Symbol = "";
        OpenTime = DateTime.MinValue;
    }

    public MinuteCandlestick(string symbol, long openTimeEpoch, decimal open, decimal high, decimal low, decimal close, decimal volume) {
        Symbol = symbol;
        OpenTimeEpoch = openTimeEpoch;
        OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(openTimeEpoch).DateTime;
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