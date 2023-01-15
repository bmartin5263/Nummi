using System.Collections.Concurrent;
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
        var logger = scope.ServiceProvider.GetService<ILogger>()!;

        TradingBot? bot = appDb.Bots.Find(BotId);
        if (bot == null) {
            Message($"Bot {BotId} no longer exists!");
            Controller.RemoveBot();
            return DefaultSleepTime;
        }

        try {
            var env = new BotEnvironment(ServiceProvider, scope, appDb, logger);
            var sleepTime = bot.WakeUp(env);
            return sleepTime;
        }
        catch (Exception e) {
            logger.LogWarning(e, "Trading Bot \"{BotName}\" threw an Exception during execution", bot.Name);
            return DefaultSleepTime;
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