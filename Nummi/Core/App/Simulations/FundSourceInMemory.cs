using Nummi.Core.App.Trading;
using Nummi.Core.Exceptions;

namespace Nummi.Core.App.Simulations; 

public class FundSourceInMemory : IFundSource {
    private decimal Funds { get; set; }
    public decimal RemainingFunds => Funds;

    public FundSourceInMemory(decimal funds) {
        Funds = funds;
    }

    public void SubtractFunds(decimal amount) {
        Funds -= amount;
        if (Funds < 0) {
            throw new SystemArgumentException("Funds is negative");
        }
    }
}