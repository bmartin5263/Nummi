namespace Nummi.Api.Model; 

public class BotFilterResponse {
    public IList<StockBotDto> Bots { get; }

    public BotFilterResponse(IList<StockBotDto> bots) {
        Bots = bots;
    }
}