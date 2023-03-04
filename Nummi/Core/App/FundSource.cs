using Nummi.Core.Domain.Bots;
using Nummi.Core.Exceptions;

namespace Nummi.Core.App; 

public interface IFundSource {
    public decimal RemainingFunds { get; }
    public void SubtractFunds(decimal amount);
}

public class FundSourceBot : IFundSource {
    private Bot Bot { get; }
    public decimal RemainingFunds => Bot.Funds;

    public FundSourceBot(Bot bot) {
        Bot = bot;
    }

    public void SubtractFunds(decimal amount) {
        Bot.SubtractFunds(amount);
    }
}

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