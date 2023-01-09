namespace Nummi.Core.Domain.Stocks.Data; 

public interface IBar {
    public string Symbol { get; }
    public DateTime TimeUtc { get; }
    public decimal Open { get; }
    public decimal High { get; }
    public decimal Low { get; }
    public decimal Close { get; }
}