using System.Collections.Concurrent;
using Alpaca.Markets;
using KSUID;
using Nummi.Core.Database;
using Nummi.Core.Domain.Stocks.Bot.Execution.Command;
using Nummi.Core.Domain.Stocks.Bot.Strategy;

namespace Nummi.Core.Domain.Stocks.Bot.Execution; 

public class BotThread {

    public uint Id { get; }
    private CancellationToken CancellationToken { get; }
    private IServiceProvider ServiceProvider { get; }
    private ConcurrentQueue<ICommand> CommandQueue { get; }
    private TimeSpan DefaultSleepTime { get; } = TimeSpan.FromSeconds(5);

    public Ksuid? BotId { get; private set; }

    public BotThread(uint id, IServiceProvider serviceProvider, CancellationToken cancellationToken) {
        Id = id;
        ServiceProvider = serviceProvider;
        CancellationToken = cancellationToken;
        CommandQueue = new ConcurrentQueue<ICommand>();
    }

    public async void MainLoop() {
        Message("Entering Main Loop");
        while (!CancellationToken.IsCancellationRequested) {
            ProcessCommands();
            if (BotId != null) {
                using var scope = ServiceProvider.CreateScope();
                await RunBotStrategy(scope);
            }
            else {
                Message("No Bot Assigned");
                await Task.Delay(TimeSpan.FromSeconds(5), CancellationToken);
            }
        }
    }

    public void ProcessCommands() {
        while (!CommandQueue.IsEmpty) {
            if (CommandQueue.TryDequeue(out ICommand? command)) {
                command.Execute(new BotThreadController(this));
            }
        }
    }

    public Task RunBotStrategy(IServiceScope scope) {
        Message("Executing Bot Strategy");
        StockBot? bot = FetchBot(scope);
        if (bot == null) {
            Message($"Bot {BotId} no longer exists!");
            new BotThreadController(this).RemoveBot();
            return Sleep();
        }
        
        ITradingStrategy? strategy = bot.Strategy;
        if (strategy == null) {
            Message($"{bot} lacks a TradingStrategy");
            return Sleep();
        }
        var context = new BotExecutionContext(CancellationToken);
        strategy.Action(context);
        return strategy.Sleep(context);
    }
    
    public void RegisterBot(Ksuid botId) {
        CommandQueue.Enqueue(new AssignBotCommand(botId));
    }

    public void DeregisterBot() {
        CommandQueue.Enqueue(new RemoveBotCommand());
    }

    private void Message(string msg, params object[] args) {
        Console.WriteLine(Id + " - " + msg, args);
    }

    private StockBot? FetchBot(IServiceScope scope) {
        using var appDb = scope.ServiceProvider.GetService<AppDb>()!;
        return appDb.Bots.FirstOrDefault(b => b.Id == BotId);
    }

    private Task Sleep() {
        return Task.Delay(DefaultSleepTime, CancellationToken);
    }

    public class BotThreadController {
        private BotThread BotThread { get; }
        
        public BotThreadController(BotThread botThread) {
            BotThread = botThread;
        }
        
        public void AssignBot(Ksuid botId) {
            BotThread.Message($"Assigning Bot {botId}");
            BotThread.BotId = botId;
        }

        public void RemoveBot() {
            BotThread.BotId = null;
        }
    }
}