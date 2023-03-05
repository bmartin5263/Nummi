using Nummi.Core.App.Trading;
using Nummi.Core.Domain.Bots;

namespace Nummi.Core.App.Bots; 

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