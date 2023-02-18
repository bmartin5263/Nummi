using Nummi.Core.Domain.New;

namespace NummiTests.Unit.Domain; 

public class BotTest {

    [Test]
    public void CreateBot_ShouldHaveDefaultValuesSet() {
        var bot = new Bot(name: "Jerry", mode: TradingMode.Live, funds: 100.0m);
        
        Assert.That(bot.Id, Is.Not.Null);
        Assert.That(bot.Name, Is.EqualTo("Jerry"));
        Assert.That(bot.Funds, Is.EqualTo(100.0m));
        Assert.That(bot.Mode, Is.EqualTo(TradingMode.Live));
        Assert.That(bot.InErrorState, Is.False);
        Assert.That(bot.CurrentActivation, Is.Null);
        Assert.That(bot.IsActive, Is.False);
        Assert.That(bot.ActivationHistory, Is.Empty);
    }

    [Test]
    public void FundAllocation_WithValidValues_ShouldChangeBotFunds() {
        var bot = new Bot(name: "Jerry", mode: TradingMode.Live, funds: 100.0m);
        Assert.That(bot.Funds, Is.EqualTo(100.0m));
        
        bot.AllocateFunds(20.0m);
        Assert.That(bot.Funds, Is.EqualTo(120.0m));

        bot.SubtractFunds(100.0m);
        Assert.That(bot.Funds, Is.EqualTo(20.0m));
    }
    
}