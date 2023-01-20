namespace Nummi.Core.Domain.Crypto.Data; 

public class BinancePrice {
    public string Symbol { get; set; }
    public string Price { get; set; }

    public BinancePrice() {
        Symbol = "";
        Price = "0";
    }

    public Price ToHistoricalPrice() {
        return new Price(Symbol, decimal.Parse(Price));
    }
}