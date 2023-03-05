using IntegrationTests.Utils;
using Microsoft.Extensions.DependencyInjection;
using Nummi.Core.App.Commands;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Domain.User;

namespace IntegrationTests.Tests.Database; 

public class ServiceTest : IntegrationTest {
    public ServiceTest() {
        
    }

    [Test]
    public void test() {
        using var scope = CreateScope();
        // var transaction = scope.ServiceProvider.GetRequiredService<ITransaction>();
        // (transaction.DbContext as EFCoreContext)!.Database.BeginTransaction();

        var createBotCommand = scope.ServiceProvider.GetRequiredService<CreateBotCommand>();
        var bot = createBotCommand.Execute(new CreateBotParameters {
            Funds = 200,
            Mode = TradingMode.Live,
            Name = "Bob",
            UserId = IdentityId.FromString("52f12441f3874d38835bdcc3174b9f1a")
        });
        
        Assert.That(bot.Name, Is.EqualTo("Bob"));
        
        // (transaction.DbContext as EFCoreContext)!.ChangeTracker.Clear();
    }
}