using System.Collections.Concurrent;
using NLog;
using Nummi.Core.Bridge;
using Nummi.Core.Domain.Bots;
using Nummi.Core.Events;
using Nummi.Core.Util;

namespace Nummi.Core.App.Bots;

public record ScheduleRequest(BotId BotId, TimeSpan Delay) {
    public ScheduleRequest(BotId botId) : this(botId, TimeSpan.Zero) { }
}

public class BotExecutor : BackgroundService {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private INummiServiceProvider ServiceProvider { get; }
    private ConcurrentQueue<ScheduleRequest> ScheduleQueue { get; }
    private ConcurrentBag<BotId> UnscheduleList { get; }
    private BotScheduler BotScheduler { get; }

    public BotExecutor(INummiServiceProvider serviceProvider) {
        ServiceProvider = serviceProvider;
        ScheduleQueue = new ConcurrentQueue<ScheduleRequest>();
        UnscheduleList = new ConcurrentBag<BotId>();
        BotScheduler = new BotScheduler();

        var eventDispatcher = serviceProvider.GetSingleton<EventDispatcher>();
        eventDispatcher.OnEvent<BotActivatedEvent>(OnBotActivation);
        eventDispatcher.OnEvent<BotDeactivatedEvent>(OnBotDeactivation);
    }

    private void OnBotActivation(BotActivatedEvent e) {
        ScheduleBot(new ScheduleRequest(e.BotId));
    }
    
    private void OnBotDeactivation(BotDeactivatedEvent e) {
        UnscheduleList.Add(e.BotId);
    }

    public void ScheduleBot(ScheduleRequest scheduleRequest) {
        ScheduleQueue.Enqueue(scheduleRequest);
    }

    private void Main(CancellationToken cancellationToken) {
        Log.Info("Running Bot Executor");
        InitializeQueue();
        while (!cancellationToken.IsCancellationRequested) {
            using (var scope = ServiceProvider.CreateScope()) {
                try {
                    var context = new BotExecutorContext(ServiceProvider, scope);
                    ProcessUnscheduleRequests();
                    ProcessScheduleRequests(context);
                    WakeBots(context);
                }
                catch (Exception e) {
                    Log.Error($"Exception thrown by BotExecutor {e}");
                }
            }
            Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken).Wait(cancellationToken);
        }
    }
    
    private void ProcessUnscheduleRequests() {
        if (UnscheduleList.IsEmpty) {
            return;
        }
        
        var removeList = new HashSet<BotId>();
        while (UnscheduleList.TryTake(out BotId botId)) {
            removeList.Add(botId);
        }

        BotScheduler.Unschedule(removeList);
        Log.Info($"Unscheduled {removeList.Count.ToString().Red()} Bots");
    }

    private void ProcessScheduleRequests(BotExecutorContext context) {
        if (ScheduleQueue.IsEmpty) {
            return;
        }
        
        var addList = new List<BotNode>();
        while (ScheduleQueue.TryDequeue(out ScheduleRequest? schedule)) {
            var bot = context.FindBot(schedule.BotId);
            if (bot?.CurrentActivation == null) {
                Log.Warn($"Bot {schedule.BotId} no longer exists. Skipping activation");
                continue;
            }
            addList.Add(new BotNode(schedule.BotId, context.Now + schedule.Delay));
        }

        BotScheduler.Schedule(addList);
        Log.Info($"Scheduled {addList.Count.ToString().Green()} Bots");
    }

    private void WakeBots(BotExecutorContext context) {
        IEnumerable<BotId> botsReadyForExecution = BotScheduler.FindBotsReadyForExecution(context);
        foreach (var botId in botsReadyForExecution) {
            Bot? bot = context.FindBot(botId);
            if (bot?.CurrentActivation == null) {
                Log.Warn($"Bot {botId} no longer exists. Skipping execution");
                continue;
            }
            Task.Run(() => new BotThread(this, ServiceProvider, bot.Id).Execute());
        }
    }

    private void InitializeQueue() {
        using var scope = ServiceProvider.CreateScope();
        var context = new BotExecutorContext(ServiceProvider, scope);
        var bots = context.FindAllActiveBots();
        foreach (var bot in bots) {
            OnBotActivation(new BotActivatedEvent(bot.Id));
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        return Task.Run(() => Main(stoppingToken), stoppingToken);
    }
}