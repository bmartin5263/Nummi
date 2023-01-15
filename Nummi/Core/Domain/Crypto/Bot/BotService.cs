using Microsoft.EntityFrameworkCore;
using Nummi.Core.Database;
using Nummi.Core.Domain.Crypto.Client;
using Nummi.Core.Domain.Crypto.Trading.Strategy;
using Nummi.Core.Util;
using static Nummi.Core.Util.Assertions;

namespace Nummi.Core.Domain.Crypto.Bot; 

public class BotService {

    private AppDb AppDb { get; }
    private IServiceProvider ServiceProvider { get; }

    public BotService(AppDb appDb, IServiceProvider serviceProvider) {
        AppDb = appDb;
        ServiceProvider = serviceProvider;
    }

    public TradingBot CreateBot(CreateBotRequest request) {
        var bot = new TradingBot(request.Name!, request.Funds ?? 0);
        AppDb.Bots.Add(bot);
        Assert(AppDb.SaveChanges() == 1);
        return bot;
    }

    public TradingBot GetBotById(string id) {
        var bot = AppDb.Bots
            .Include(b => b.Strategy)
            .FirstOrDefault(b => b.Id == id);

        if (bot == null) {
            throw new EntityNotFoundException<TradingBot>(id);
        }
        
        return bot;
    }

    public void DeleteBotById(string id) {
        var bot = AppDb.Bots.Find(id);
        if (bot == null) {
            return;
        }
        AppDb.Bots.Remove(bot);
        AppDb.SaveChanges();
    }

    public IEnumerable<TradingBot> GetBots() {
        return AppDb.Bots
            .Include(b => b.Strategy)
            .ToList();
    }

    public TradingBot SetBotStrategy(string botId, string strategyId) {
        var bot = GetBotById(botId);
        var strategy = AppDb.Strategies.FindById(strategyId);
        bot.Strategy = strategy;
        AppDb.SaveChanges();
        return GetBotById(botId);
    }

    public TradingBot RunBotStrategy(string botId) {
        var bot = GetBotById(botId);

        var env = new BotEnvironment(
            serviceProvider: ServiceProvider,
            scope: ServiceProvider.CreateScope(),
            appDb: AppDb
        );

        bot.WakeUp(env);

        AppDb.Strategies.Update(bot.Strategy!);
        AppDb.SaveChanges();
        return GetBotById(botId);
    }

    public TradingStrategy RunBotStrategy2(string strategyId) {
        var strategy = AppDb.Strategies.FindById(strategyId);

        var env = new BotEnvironment(
            serviceProvider: ServiceProvider,
            scope: ServiceProvider.CreateScope(),
            appDb: AppDb
        );

        var cryptoClient = env.GetService<CryptoClientMock>();
        var tradingContext = new TradingContext(cryptoClient, 0);
        
        strategy.CheckForTrades(tradingContext);
        
        AppDb.SaveChanges();
        return strategy;
    }

    public void ValidateId(string id) {
        GetBotById(id);
    }
    
}