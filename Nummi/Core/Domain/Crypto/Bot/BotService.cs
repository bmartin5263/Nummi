using KSUID;
using Nummi.Core.Database;
using static Nummi.Core.Util.Assertions;

namespace Nummi.Core.Domain.Crypto.Bot; 

public class BotService {

    private AppDb AppDb { get; }
    
    public BotService(AppDb appDb) {
        AppDb = appDb;
    }

    public TradingBot CreateBot(CreateBotRequest request) {
        var bot = new TradingBot(request.Name!);
        AppDb.Bots.Add(bot);
        Assert(AppDb.SaveChanges() == 1);
        return bot;
    }

    public TradingBot GetBot(Ksuid id) {
        return AppDb.Bots.FindById(id);
    }

    public TradingBot SetBotStrategy(Ksuid botId, Ksuid strategyId) {
        var bot = GetBot(botId);
        var strategy = AppDb.Strategies.FindById(strategyId);
        bot.Strategy = strategy;
        AppDb.SaveChanges();
        return GetBot(botId);
    }

    public void ValidateId(Ksuid id) {
        GetBot(id);
    }
    
}