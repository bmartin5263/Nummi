namespace Nummi.Core.Domain.Crypto.Bot.Execution; 

public class BotExecutor : BackgroundService {
    
    private IServiceProvider ServiceProvider { get; }
    private uint NumThreads { get; }
    private BotThread[] Threads { get; }

    public BotExecutor(IServiceProvider serviceProvider, uint numThreads) {
        ServiceProvider = serviceProvider;
        NumThreads = numThreads;
        Threads = new BotThread[numThreads];
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        for (uint i = 0; i < NumThreads; ++i) {
            Threads[i] = new BotThread(i, ServiceProvider, stoppingToken);
            var id = i;
            Task.Run(() => Threads[id].MainLoop(), stoppingToken);
        }
        return Task.CompletedTask;
    }

    public BotThread GetThread(uint id) {
        if (id >= NumThreads) {
            throw new ArgumentException($"No thread with id {id}");
        }
        return Threads[id];
    }

    public BotThreadsOverview GetThreads() {
        return new BotThreadsOverview(
            NumThreads,
            Threads.Select(t => new BotThreadDetail(
                t.Id,
                t.BotId?.ToString()
            )).ToList()
        );
    }
}