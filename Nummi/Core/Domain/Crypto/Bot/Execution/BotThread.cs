using System.Collections.Concurrent;
using KSUID;
using Nummi.Core.Database;
using Nummi.Core.Domain.Crypto.Bot.Execution.Command;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Bot.Execution; 

public class BotThread {

    public uint Id { get; }
    public Ksuid? BotId { get; private set; }
    
    private CancellationToken CancellationToken { get; }
    private IServiceProvider ServiceProvider { get; }
    private ConcurrentQueue<ICommand> CommandQueue { get; }
    private TimeSpan DefaultSleepTime { get; } = TimeSpan.FromSeconds(5);
    private BotThreadController Controller { get; }

    public BotThread(uint id, IServiceProvider serviceProvider, CancellationToken cancellationToken) {
        Id = id;
        ServiceProvider = serviceProvider;
        CancellationToken = cancellationToken;
        CommandQueue = new ConcurrentQueue<ICommand>();
        Controller = new BotThreadController(this);
    }

    public void MainLoop() {
        Message("Entering Main Loop");
        while (!CancellationToken.IsCancellationRequested) {
            ProcessCommands();
            Task sleepTask = RunBotLogic();
            sleepTask.Wait(CancellationToken);
        }
    }

    private void ProcessCommands() {
        while (!CommandQueue.IsEmpty) {
            if (CommandQueue.TryDequeue(out ICommand? command)) {
                command.Execute(new BotThreadController(this));
            }
        }
    }

    private Task RunBotLogic() {
        if (BotId == null) {
            Message("No Bot Assigned");
            return Task.Delay(DefaultSleepTime, CancellationToken);
        }
        
        using var scope = ServiceProvider.CreateScope();
        
        Message("Executing Bot Strategy");
        using var appDb = scope.ServiceProvider.GetService<AppDb>()!;
        using var transaction = new DbTransaction(appDb);

        TradingBot? bot = appDb.Bots.Find(BotId);
        if (bot == null) {
            HandleBotNoLongerExists();
            return Sleep(DefaultSleepTime);
        }

        if (bot.ErrorState != null) {
            HandleBotInErrorState(bot);
            return Sleep(DefaultSleepTime);
        }
        
        if (!bot.HasTradingStrategy) {
            HandleBotStrategyMissing(bot);
            return Sleep(DefaultSleepTime);
        }
        
        var context = new BotExecutionContext(bot, scope, CancellationToken);
        try {
            bot.ExecuteStrategy(context);
            return Sleep(TimeSpan.FromMinutes(1));
        }
        catch (Exception e) {
            HandleBotStrategyError(bot, e);
            return Sleep(DefaultSleepTime);
        }
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

    private void HandleBotNoLongerExists() {
        Message($"Bot {BotId} no longer exists!");
        Controller.RemoveBot();
    }

    private void HandleBotStrategyError(TradingBot bot, Exception e) {
        Message($"{bot.Name} threw an Exception: {e}");
        bot.ErrorState = new BotError(DateTime.Now, e.ToString());
    }

    private void HandleBotStrategyMissing(TradingBot bot) {
        Message($"{bot.Name} lacks a TradingStrategy");
    }

    private void HandleBotInErrorState(TradingBot bot) {
        Message($"{bot.Name} is in an error state {bot.ErrorState!.Message}");
    }

    private Task Sleep(TimeSpan time) {
        return Task.Delay(time, CancellationToken);
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