using Nummi.Core.Domain.Crypto.Bots.Thread.Command;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Bots.Thread; 

public class BotExecutionManager : BackgroundService {
    
    private IServiceProvider ServiceProvider { get; }
    private uint NumThreads { get; }
    private BotThread[] Threads { get; }

    public BotExecutionManager(IServiceProvider serviceProvider, uint numThreads) {
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

    public BotThreadDetail AssignBot(Bot bot) {
        BotThread? availableThread = null;
        foreach (var thread in Threads) {
            if (thread.BotId == bot.Id) {
                throw new InvalidArgumentException($"{bot.Name} is already active");
            }
            if (thread.BotId == null) {
                availableThread = thread;
            }
        }

        if (availableThread == null) {
            throw new InvalidStateException($"No threads available to run {bot.Name}");
        }
        
        availableThread.AddCommand(new AssignBotCommand(bot.Id));
        
        return new BotThreadDetail(
            id: availableThread.Id,
            botId: bot.Id
        );
    }

    public BotThreadDetail RemoveBot(Bot bot) {
        var thread = Threads
            .FirstOrDefault(t => t.BotId == bot.Id)
            .OrElseThrow(() => new InvalidArgumentException($"{bot.Name} is not active"));

        thread.AddCommand(new RemoveBotCommand());
        
        return new BotThreadDetail(
            id: thread.Id,
            botId: null
        );
    }

    public void RunBotSimulation(Bot bot, SimulationParameters parameters, Simulation simulation) {
        var thread = Threads
            .FirstOrDefault(t => t.BotId == bot.Id)
            .OrElseThrow(() => new InvalidArgumentException($"{bot.Name} is not active"));

        thread.AddCommand(new SimulateBotCommand(parameters, simulation.Id));
    }

    public bool IsBotActive(Bot bot) {
        return Threads.Any(t => t.BotId == bot.Id);
    }

    public BotThread GetThread(uint id) {
        if (id >= NumThreads) {
            throw new InvalidArgumentException($"No thread with id {id}");
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