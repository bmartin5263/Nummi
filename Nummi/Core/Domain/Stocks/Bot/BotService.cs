using KSUID;
using Nummi.Core.Database;
using Nummi.Core.Util;

using static Nummi.Core.Util.Assertions;

namespace Nummi.Core.Domain.Stocks.Bot; 

public class BotService {

    private readonly AppDb appDb;
    
    public BotService(AppDb appDb) {
        this.appDb = appDb;
    }

    public StockBot CreateBot(CreateBotRequest request) {
        var bot = new StockBot(request.Name!);
        appDb.Bots.Add(bot);
        Assert(appDb.SaveChanges() == 1);
        return bot;
    }

    public StockBot GetBot(Ksuid id) {
        return appDb.Bots.First(b => b.Id == id);
    }

    public void ValidateId(Ksuid id) {
        GetBot(id);
    }
    
}