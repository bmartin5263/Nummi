using NLog;
using Nummi.Core.App.Trading;
using Nummi.Core.Bridge;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Bots;
using Nummi.Core.Domain.Strategies;

namespace Nummi.Core.App.Bots; 

public class BotThread {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public BotId BotId { get; }
    private BotExecutor BotExecutor { get; }
    private INummiServiceProvider ServiceProvider { get; }

    public BotThread(BotExecutor botExecutor, INummiServiceProvider serviceProvider, BotId botId) {
        BotExecutor = botExecutor;
        ServiceProvider = serviceProvider;
        BotId = botId;
    }

    public void Execute() {
        Log.Info($"Executing Bot {BotId}");
        using var scope = ServiceProvider.CreateScope();
        var botRepository = scope.GetService<IBotRepository>();
        
        Bot? bot = botRepository.FindByIdForExecution(BotId);
        if (bot == null) {
            Log.Info($"Bot {BotId} no longer exists");
            return;
        }
        
        if (bot.InErrorState) {
            Log.Info($"Bot {BotId} cannot execute, it is in an error state");
            return;
        }

        Strategy strategy = bot.CurrentActivation?.Strategy!;
        
        var sessionFactory = scope.GetService<TradingSessionFactory>();
        var session = sessionFactory.CreateRealtime(bot);

        StrategyExecutionResult result;
        try {
            result = strategy.Run(session);
        }
        catch (Exception e) {
            Log.Error($"Unexpected error thrown while executing Strategy {strategy.Id} on Bot {BotId}\n{e}");
            botRepository.Commit();
            return;
        }
        
        if (result.Failed) {
            Log.Error($"Bot {BotId} Strategy Execution Failed\n{result.Error!}");
            bot.SetInErrorState();
        }
        else {
            Log.Info($"Rescheduling Bot {BotId} in {strategy.Frequency.AsTimeSpan}");
            BotExecutor.ScheduleBot(new ScheduleRequest(BotId: BotId, Delay: strategy.Frequency.AsTimeSpan));
        }
        
        botRepository.Commit();
    }
}