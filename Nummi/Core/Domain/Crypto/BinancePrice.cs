namespace Nummi.Core.Domain.Crypto;

public class BinancePrice {
    public string Symbol { get; set; }
    public string Price { get; set; }

    public BinancePrice() {
        Symbol = "";
        Price = "0";
    }

    public Price ToPrice() {
        return new Price(Symbol, decimal.Parse(Price));
    }
}