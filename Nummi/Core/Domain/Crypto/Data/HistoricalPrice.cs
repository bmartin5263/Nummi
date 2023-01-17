using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Data; 

[Table("HistoricalPrice")]
[PrimaryKey(nameof(Symbol), nameof(Time))]
public class HistoricalPrice {
    public string Symbol { get; init; }
    public DateTime Time { get; init; }
    public decimal Price { get; init; }

    private HistoricalPrice() {
        Symbol = "";
        Time = DateTime.Now;
        Price = 0;
    }

    public HistoricalPrice(string symbol, decimal price, DateTime? time = null) {
        Symbol = symbol;
        Time = time ?? DateTime.Now.ToUniversalTime().Round(new TimeSpan(0, 1, 0));
        Price = price;
    }

    public override string ToString() {
        return this.ToFormattedString();
    }
}