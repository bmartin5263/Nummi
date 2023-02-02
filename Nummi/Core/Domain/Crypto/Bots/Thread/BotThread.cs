using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using NLog;
using Nummi.Core.Database;
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

    public void Initialize() {
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
                Message($"Auto Assigning Bot [{botThread.Bot.Name.Yellow()}]");
                BotId = botThread.Bot.Id;
            }
        }

        appDb.SaveChanges();
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
        using var appDb = scope.ServiceProvider.GetService<AppDb>()!;
        using var transaction = new DbTransaction(appDb);

        Bot? bot = appDb.Bots
            .Include(b => b.Strategy)
            .Include(b => b.LastStrategyLog)
            .FirstOrDefault(b => b.Id == BotId);
        
        if (bot == null) {
            Message($"{"ERROR!!".Red()} Bot {BotId.Yellow()} no longer exists!");
            Controller.RemoveBot();
            return DefaultSleepTime;
        }

        if (bot.Mode == TradingMode.Simulated) {
            Message($"{bot.Name.Yellow()} is a Simulation Bot which cannot execute in real-time trading");
            return DefaultSleepTime;
        }

        Message($"Waking {bot.Name.Yellow()}");
        try {
            var env = new BotContext(ServiceProvider, scope, appDb);
            return bot.RunRealtime(env);
        }
        catch (Exception e) {
            Message($"{"ERROR!!".Red()} Bot [{bot.Name.Yellow()}] threw an Exception during execution: {e}");
            return DefaultSleepTime;
        }
    }

    protected void Message(string msg) {
        Log.Info($"[{"Id".Purple()}:{Id.ToString().Cyan()}] - {msg}");
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
        private BotThread Self { get; }
        
        public BotThreadController(BotThread self) {
            Self = self;
        }
        
        public void AssignBot(string botId) {
            Self.Message($"Assigning Bot {botId}");
            Self.BotId = botId;
            
            using var scope = Self.ServiceProvider.CreateScope();
            using var appDb = scope.ServiceProvider.GetService<AppDb>()!;
            var bot = appDb.Bots.Find(botId).OrElseThrow(() => new EntityMissingException<Bot>(botId));

            var botThreadEntity = Self.GetBotThreadEntity(appDb);
            botThreadEntity.Bot = bot;
            appDb.SaveChanges();
        }

        public void RemoveBot() {
            Self.BotId = null;
            
            using var scope = Self.ServiceProvider.CreateScope();
            using var appDb = scope.ServiceProvider.GetService<AppDb>()!;
            var botThreadEntity = Self.GetBotThreadEntity(appDb);
            botThreadEntity.Bot = null;
            appDb.SaveChanges();
        }

        public void SimulateBot(SimulationParameters parameters, string resultId) {
            using var scope = Self.ServiceProvider.CreateScope();
            using var appDb = scope.ServiceProvider.GetService<AppDb>()!;
            
            var simulation = appDb.Simulations.Find(resultId)
                .OrElseThrow(() => new EntityNotFoundException<Simulation>("Could not find StrategyResult"));
            
            simulation.Start();
            appDb.SaveChanges();

            try {
                DoSimulateBot(parameters, simulation, appDb, scope);
            }
            catch (Exception e) {
                Log.Error($"{"Error!!".Red()} Simulation failed. {e}");
                simulation.Finish(e);
            }
            
            appDb.SaveChanges();
        }

        private void DoSimulateBot(
            SimulationParameters parameters, 
            Simulation simulation, 
            AppDb appDb, 
            IServiceScope scope
        ) {
            if (Self.BotId == null) {
                throw new InvalidUserArgumentException($"Thread {Self.Id} does not have a Bot to simulate");
            }

            var bot = appDb.Bots
                .Include(b => b.Strategy)
                .FirstOrDefault(b => b.Id == Self.BotId)
                .ThrowIfNull(() => new EntityMissingException<Bot>(Self.BotId));

            var context = new BotContext(
                serviceProvider: Self.ServiceProvider,
                scope: scope,
                appDb: appDb
            );

            var logs = bot.RunSimulation(context, parameters);
            simulation.Finish(logs);
        }
    }
}