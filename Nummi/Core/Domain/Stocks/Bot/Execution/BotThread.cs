using Nummi.Core.Domain.Stocks.Bot.Strategy;

namespace Nummi.Core.Domain.Stocks.Bot.Execution; 

public class BotThread {

    private uint Id { get; }
    private CancellationToken CancellationToken { get; }
    private StockBot? Bot { get; set; }

    public BotThread(uint id, CancellationToken cancellationToken) {
        Id = id;
        CancellationToken = cancellationToken;
    }

    public async Task MainLoop() {
        Message("Entering Main Loop");
        var context = new BotExecutionContext(CancellationToken);
        while (!CancellationToken.IsCancellationRequested) {
            if (Bot == null) {
                Message("Bot is null!");
                await Task.Delay(TimeSpan.FromSeconds(5), CancellationToken);
            }
            else {
                Message("Executing Bot Strategy");
                var strategy = Bot.Strategy;
                strategy.Action(context);
                await strategy.Sleep(context);
            }
        }
    }

    private void Message(string msg, params object[] args) {
        Console.WriteLine(Id + " - " + msg, args);
    }
}