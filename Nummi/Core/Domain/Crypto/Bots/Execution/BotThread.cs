using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using NLog;
using Nummi.Core.Database;
using Nummi.Core.Domain.Crypto.Bots.Execution.Command;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Bots.Execution; 

public class BotThread {
    
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public uint Id { get; }
    public string? BotId { get; private set; }
    
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

    public void InitializeBot() {
        using var scope = ServiceProvider.CreateScope();
        using var appDb = scope.ServiceProvider.GetService<AppDb>()!;

        var botThread = appDb.BotThreads
            .Include(v => v.Bot)
            .FirstOrDefault(v => v.Id == Id);
        
        if (botThread == null) {
            appDb.BotThreads.Add(new BotThreadEntity(Id));
        }
        else {
            if (botThread.Bot != null) {
                Message($"Auto Assigning Bot [{botThread.Bot.Name}]");
                BotId = botThread.Bot.Id;
            }
        }

        appDb.SaveChanges();
    }

    public void MainLoop() {
        InitializeBot();
        Message("Entering Main Loop");
        while (!CancellationToken.IsCancellationRequested) {
            ProcessCommands();
            var sleepTime = RunBotLogic();
            Task sleepTask = Task.Delay(sleepTime, CancellationToken);
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

    private TimeSpan RunBotLogic() {
        if (BotId == null) {
            Message("No Bot Assigned");
            return DefaultSleepTime;
        }
        
        using var scope = ServiceProvider.CreateScope();
        using var appDb = scope.ServiceProvider.GetService<AppDb>()!;
        using var transaction = new DbTransaction(appDb);

        Bot? bot = appDb.Bots
            .Include(b => b.Strategy)
            .FirstOrDefault(b => b.Id == BotId);
        
        if (bot == null) {
            Message($"Bot {BotId} no longer exists!");
            Controller.RemoveBot();
            return DefaultSleepTime;
        }

        Message($"Waking Bot {bot.Name}");
        try {
            var env = new BotEnvironment(ServiceProvider, scope, appDb);
            var sleepTime = bot.WakeUp(env);
            Message($"Going to Sleep For {sleepTime}");
            return sleepTime;
        }
        catch (Exception e) {
            Message($"Trading Bot \"{bot.Name}\" threw an Exception during execution: {e}");
            return DefaultSleepTime;
        }
        finally {
            appDb.Strategies.Update(bot.Strategy!);
        }
    }

    public void RegisterBot(string botId) {
        AssertBotExists(botId);
        CommandQueue.Enqueue(new AssignBotCommand(botId));
    }

    public void DeregisterBot() {
        CommandQueue.Enqueue(new RemoveBotCommand());
    }

    private void Message(string msg, params object[] args) {
        Log.Info($"Thread #{Id} - " + msg, args);
    }

    private void AssertBotExists(string botId) {
        using var scope = ServiceProvider.CreateScope();
        using var appDb = scope.ServiceProvider.GetService<AppDb>()!;
        appDb.Bots.FindById(botId);
    }

    private BotThreadEntity GetBotThreadEntity(AppDb appDb) {
        var botThread = appDb.BotThreads.Find(Id);
        if (botThread != null) {
            return botThread;
        }
        botThread = new BotThreadEntity(Id);
        appDb.BotThreads.Add(botThread);
        appDb.Attach(botThread);
        return botThread;
    }

    public class BotThreadController {
        private BotThread BotThread { get; }
        
        public BotThreadController(BotThread botThread) {
            BotThread = botThread;
        }
        
        public void AssignBot(string botId) {
            BotThread.Message($"Assigning Bot {botId}");
            BotThread.BotId = botId;
            
            using var scope = BotThread.ServiceProvider.CreateScope();
            using var appDb = scope.ServiceProvider.GetService<AppDb>()!;
            var bot = appDb.Bots.FindById(botId);
            var botThreadEntity = BotThread.GetBotThreadEntity(appDb);
            botThreadEntity.Bot = bot;
            appDb.SaveChanges();
        }

        public void RemoveBot() {
            BotThread.BotId = null;
            
            using var scope = BotThread.ServiceProvider.CreateScope();
            using var appDb = scope.ServiceProvider.GetService<AppDb>()!;
            var botThreadEntity = BotThread.GetBotThreadEntity(appDb);
            botThreadEntity.Bot = null;
            appDb.SaveChanges();
        }
    }
}