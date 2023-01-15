using Microsoft.EntityFrameworkCore;
using Nummi.Core.Database;
using Nummi.Core.Util;
using static Nummi.Core.Util.Assertions;

namespace Nummi.Core.Domain.Crypto.Bot; 

public class BotService {

    private AppDb AppDb { get; }
    
    public BotService(AppDb appDb) {
        AppDb = appDb;
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
        
        AppDb.Entry(bot).Reference(e => e.Strategy).Load();

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
        return AppDb.Bots.ToList();
    }

    public TradingBot SetBotStrategy(string botId, string strategyId) {
        var bot = GetBotById(botId);
        var strategy = AppDb.Strategies.FindById(strategyId);
        bot.Strategy = strategy;
        AppDb.SaveChanges();
        return GetBotById(botId);
    }

    public void ValidateId(string id) {
        GetBotById(id);
    }
    
}