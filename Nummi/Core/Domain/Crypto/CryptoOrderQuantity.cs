using Nummi.Core.Exceptions;

namespace Nummi.Core.Domain.Crypto;

public record struct CryptoOrderQuantity {
    public decimal? Dollars { get; set; }
    public decimal? Coins { get; set; }

    public void Validate() {
        if (Dollars == null && Coins == null) {
            throw new InvalidUserArgumentException("Must provide either Coins or Dollars value");
        }
        if (Dollars != null && Coins != null) {
            throw new InvalidUserArgumentException("Can only provide either Coins or Dollars value, not both");
        }
    }
}