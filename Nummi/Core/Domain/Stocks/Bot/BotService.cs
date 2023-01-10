using KSUID;
using Nummi.Core.Database;
using Nummi.Core.Domain.Stocks.Bot.Strategy;
using Nummi.Core.Util;

using static Nummi.Core.Util.Assertions;

namespace Nummi.Core.Domain.Stocks.Bot; 

public class BotService {

    private readonly AppDb appDb;
    
    public BotService(AppDb appDb) {
        this.appDb = appDb;
    }

    public TradingBot CreateBot(CreateBotRequest request) {
        var bot = new TradingBot(request.Name!);
        appDb.Bots.Add(bot);
        Assert(appDb.SaveChanges() == 1);
        return bot;
    }

    public TradingBot GetBot(Ksuid id) {
        return appDb.Bots.First(b => b.Id == id);
    }

    public TradingBot ChangeBotStrategy(Ksuid botId, ChangeStrategyRequest request) {
        var bot = GetBot(botId);
        var strategy = TradingStrategyFactory.Create(request.FullStrategyName);
        bot.Strategy = strategy;
        appDb.SaveChanges();
        return GetBot(botId);
    }

    public void ValidateId(Ksuid id) {
        GetBot(id);
    }
    
}