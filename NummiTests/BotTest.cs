// using Nummi.Core.Domain.Crypto.Bot;
// using Nummi.Core.Domain.Crypto.Bot.Execution;
// using Nummi.Core.Domain.Crypto.Bot.Strategy;
// using Nummi.Core.Domain.Crypto.Trading.Strategy;
//
// namespace NummiTests;
//
// public class BotTest {
//     
//     [SetUp]
//     public void Setup() { }
//
//     [Test]
//     public void CreateBot_ExpectDefaultValuesSet() {
//         var bot = new TradingBot("Jerry");
//         Assert.Multiple(() =>
//         {
//             Assert.That(bot.Id, Is.Not.Null);
//             Assert.That(bot.Name, Is.EqualTo("Jerry"));
//             Assert.That(bot.Strategy, Is.EqualTo(null));
//             Assert.That(bot.HasTradingStrategy, Is.False);
//             Assert.That(bot.Funds, Is.Zero);
//             Assert.That(bot.Profit, Is.Zero);
//             Assert.That(bot.TimesExecuted, Is.Zero);
//             Assert.That(bot.TimesFailed, Is.Zero);
//             Assert.That(bot.ErrorState, Is.Null);
//         });
//     }
//
//     [Test]
//     public void ExecuteStrategy_WithNonNullStrategy_ExpectSuccess() {
//         var bot = new TradingBot("Jerry") {
//             Strategy = new TestStrategy()
//         };
//         
//         bot.ExecuteStrategy(null);
//         Assert.Pass();
//     }
// }
//
// internal class TestStrategy : IStrategy<> {
//     public void Execute(BotExecutionContext context) {
//     }
// }