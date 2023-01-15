using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Database;
using Nummi.Core.Domain.Crypto.Bot.Execution.Command;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Bot.Execution; 

public class BotThread {

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

    public void MainLoop() {
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

        TradingBot? bot = appDb.Bots
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
        Console.WriteLine("Thread #" + Id + " - " + msg, args);
    }

    private void AssertBotExists(string botId) {
        using var scope = ServiceProvider.CreateScope();
        using var appDb = scope.ServiceProvider.GetService<AppDb>()!;
        appDb.Bots.FindById(botId);
    }

    public class BotThreadController {
        private BotThread BotThread { get; }
        
        public BotThreadController(BotThread botThread) {
            BotThread = botThread;
        }
        
        public void AssignBot(string botId) {
            BotThread.Message($"Assigning Bot {botId}");
            BotThread.BotId = botId;
        }

        public void RemoveBot() {
            BotThread.BotId = null;
        }
    }
}