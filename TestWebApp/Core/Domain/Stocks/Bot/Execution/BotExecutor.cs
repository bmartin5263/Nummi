using TestWebApp.Core.Domain.Stocks.Bot.Strategy;

namespace TestWebApp.Core.Domain.Stocks.Bot.Execution; 

public class BotExecutor : BackgroundService {
    
    public uint Threads { get; }

    public BotExecutor(uint threads) {
        Threads = threads;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        List<Task> tasks = new List<Task>();
        for (uint i = 0; i < Threads; ++i) {
            var id = i;
            tasks.Add(Task.Run(() => new BotThread(id, stoppingToken).MainLoop(), stoppingToken));
        }
        return Task.WhenAll(tasks);
    }

}