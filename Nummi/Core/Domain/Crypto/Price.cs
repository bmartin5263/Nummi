using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto; 

[Table("HistoricalPrice")]
[PrimaryKey(nameof(Symbol), nameof(Time))]
public class Price {
    public string Symbol { get; init; }
    public DateTime Time { get; init; }
    public decimal Value { get; init; }

    private Price() {
        Symbol = "";
        Time = DateTime.UtcNow;
        Value = 0;
    }

    public Price(string symbol, decimal value, DateTime? time = null) {
        Symbol = symbol;
        Time = time ?? DateTime.UtcNow.Truncate(TimeSpan.FromMilliseconds(1));
        Value = value;
    }

    public override string ToString() {
        return this.ToFormattedString();
    }
}