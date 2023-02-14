using System.Collections.Concurrent;
using NLog;
using Nummi.Core.Database.Common;
using Nummi.Core.Database.EFCore;
using Nummi.Core.Domain.Crypto.Bots.Thread.Command;
using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Bots.Thread; 

public class BotThread {
    
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public uint Id { get; }
    
    // TODO - thread safe
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

    private void Initialize() {
        using var scope = ServiceProvider.CreateScope();
        using var database = scope.ServiceProvider.GetService<ITransaction>()!;

        var botThread = database.BotThreadRepository.FindById(Id);
        
        if (botThread == null) {
            database.BotThreadRepository.Add(new BotThreadEntity(Id));
        }
        else {
            database.BotThreadRepository.LoadProperty(botThread, t => t.Bot);
            if (botThread.Bot == null) 
                return;
            
            Message($"Auto Assigning Bot [{botThread.Bot.Name.Yellow()}]");
            BotId = botThread.Bot.Id;
        }
    }

    public void MainLoop() {
        Initialize();
        Message("Entering Main Loop");
        while (!CancellationToken.IsCancellationRequested) {
            ProcessCommands();
            var sleepTime = RunBotLogic();
            Task sleepTask = Task.Delay(sleepTime, CancellationToken);
            Message($"Going to Sleep For {sleepTime}");
            sleepTask.Wait(CancellationToken);
        }
        Message("Exiting");
    }

    public void AddCommand(ICommand command) {
        CommandQueue.Enqueue(command);
    }

    private void ProcessCommands() {
        while (!CommandQueue.IsEmpty) {
            if (CommandQueue.TryDequeue(out ICommand? command)) {
                try {
                    command.Execute(new BotThreadController(this));
                }
                catch (Exception e) {
                    Log.Error($"Command {command} failed to execute: {e}");
                }
            }
        }
    }

    private TimeSpan RunBotLogic() {
        if (BotId == null) {
            Message("No Bot Assigned");
            return DefaultSleepTime;
        }
        
        using var scope = ServiceProvider.CreateScope();
        // using var db = scope.ServiceProvider.GetService<Transaction>()!.EnableAutoCommit();

        Bot? bot;
        using (var db = scope.ServiceProvider.GetService<ITransaction>()!) {
            // bot = db.BotRepository.FindById(BotId);
            bot = null;
        }
        
        if (bot == null) {
            Message($"{"ERROR!!".Red()} Bot {BotId.Yellow()} no longer exists!");
            Controller.RemoveBot();
            return DefaultSleepTime;
        }

        if (bot.Mode == TradingMode.Simulated) {
            Message($"{bot.Name.Yellow()} is a Simulation Bot which cannot execute in real-time trading");
            return DefaultSleepTime;
        }
        
        if (bot.Strategy == null) {
            Message("No strategy assigned");
            return DefaultSleepTime;
        }
        
        if (bot.InErrorState) {
            Message("Cannot run strategy while in error state");
            return DefaultSleepTime;
        }
        
        if (!bot.IsTimeToExecute(out TimeSpan? sleep)) {
            Message($"Not yet time to run strategy");
            return sleep!.Value;
        }

        Message($"Waking {bot.Name.Yellow()}");
        try {
            var env = new NummiContext(ServiceProvider, scope);
            bot.RunRealtime(env);
            return bot.Strategy.Frequency;
        }
        catch (Exception e) {
            Message($"{"ERROR!!".Red()} Bot [{bot.Name.Yellow()}] threw an Exception during execution: {e}");
            return DefaultSleepTime;
        }
    }
    
    public bool IsTimeToExecute(DateTime? lastExecutedAt, TimeSpan frequency, out TimeSpan? sleep) {
        if (lastExecutedAt == null) {
            sleep = null;
            return true;
        }
        
        var now = DateTime.UtcNow;
        if (lastExecutedAt + frequency > now) {
            sleep = frequency - (now - lastExecutedAt);
            return false;
        }
        
        sleep = null;
        return true;
    }

    protected void Message(string msg) {
        Log.Info($"[{"Id".Purple()}:{Id.ToString().Cyan()}] - {msg}");
    }

    private BotThreadEntity GetBotThreadEntity(EFCoreContext appDb) {
        // var botThread = appDb.BotThreads.Find(Id);
        // if (botThread != null) {
        //     return botThread;
        // }
        var botThread = new BotThreadEntity(Id);
        // appDb.BotThreads.Add(botThread);
        appDb.Attach(botThread);
        return botThread;
    }

    public class BotThreadController {
        private BotThread Self { get; }
        
        public BotThreadController(BotThread self) {
            Self = self;
        }
        
        public void AssignBot(string botId) {
            Self.Message($"Assigning Bot {botId}");
            Self.BotId = botId;
            
            using var scope = Self.ServiceProvider.CreateScope();
            using var appDb = scope.ServiceProvider.GetService<EFCoreContext>()!;
            var bot = appDb.Bots.Find(botId).OrElseThrow(() => new EntityMissingException<Bot>(botId));

            var botThreadEntity = Self.GetBotThreadEntity(appDb);
            // botThreadEntity.Bot = bot;
            appDb.SaveChanges();
        }

        public void RemoveBot() {
            Self.BotId = null;
            
            using var scope = Self.ServiceProvider.CreateScope();
            using var appDb = scope.ServiceProvider.GetService<EFCoreContext>()!;
            var botThreadEntity = Self.GetBotThreadEntity(appDb);
            botThreadEntity.Bot = null;
            appDb.SaveChanges();
        }

        public void SimulateBot(SimulationParameters parameters, string resultId) {
            if (Self.BotId == null) {
                throw new InvalidUserArgumentException($"Thread {Self.Id} does not have a Bot to simulate");
            }
            
            using var scope = Self.ServiceProvider.CreateScope();
            using var db = scope.ServiceProvider.GetService<ITransaction>()!;
            
            // var bot = db.BotRepository.FindById(Self.BotId)
            //     .ThrowIfNull(() => new EntityMissingException<Bot>(Self.BotId));
            // // db.BotRepository.LoadProperty(bot, b => b.Strategy);
            // //
            // // if (bot.Strategy == null) {
            // //     throw new InvalidUserArgumentException($"Bot {bot.Name} does not have a Strategy to simulate");
            // // }
            //
            // var simulation = db.SimulationRepository.FindById(resultId)
            //     .OrElseThrow(() => new EntityMissingException<Simulation>("Could not find StrategyResult"));
            //
            // simulation.Start();
            db.SaveChanges();
            
            var context = new NummiContext(
                serviceProvider: Self.ServiceProvider,
                scope: scope
            );

            try {
                // var logs = bot.RunSimulation(context, parameters);
                // simulation.Finish(logs);
            }
            catch (Exception e) {
                Log.Error($"{"Error!!".Red()} Simulation failed. {e}");
                // simulation.Finish(e);
            }
        }
    }
}