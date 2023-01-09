using System.ComponentModel;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Stocks.Data; 

// unix,date,symbol,open,high,low,close,Volume BTC,Volume USD
public class BitstampBar : IBar {
    
    [Name("unix")]
    public long Unix { get; set; } = default;
    
    [Name("date")]
    public DateTime TimeUtc { get; set; } = default;
    
    [Name("symbol")]
    public string Symbol { get; set; } = default!;
    
    [Name("open")]
    public decimal Open { get; set; } = default;
    
    [Name("high")]
    public decimal High { get; set; } = default;
    
    [Name("low")]
    public decimal Low { get; set; } = default;
    
    [Name("close")]
    public decimal Close { get; set; } = default;

    public override string ToString() {
        return this.ToFormattedString();
    }
}