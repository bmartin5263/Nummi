using IntegrationTests.Utils;
using Nummi.Core.App.Commands;
using Nummi.Core.Domain.Crypto;

namespace IntegrationTests.Tests.Database; 

public class ServiceTest : IntegrationTest {

    [Test]
    public async Task test() {
        using var scope = CreateAutoRollbackScope();
        var user = await scope.CreateUserAsync();

        var createBotCommand = scope.GetScoped<CreateBotCommand>();
        var bot = createBotCommand.Execute(new CreateBotParameters {
            Funds = 200,
            Mode = TradingMode.Live,
            Name = "Bob",
            UserId = user.Id
        });
        
        Assert.That(bot.Name, Is.EqualTo("Bob"));
    }
}