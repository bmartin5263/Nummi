namespace Nummi.Core.Domain.Crypto.Data; 

public interface IBar {
    public string Symbol { get; }
    public DateTime OpenTimeUtc { get; }
    public TimeSpan Period { get; }
    public decimal Open { get; }
    public decimal High { get; }
    public decimal Low { get; }
    public decimal Close { get; }
    public decimal Volume { get; }
}