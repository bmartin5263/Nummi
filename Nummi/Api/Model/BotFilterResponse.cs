namespace Nummi.Api.Model; 

public class BotFilterResponse {
    public IList<BotDto> Bots { get; }

    public BotFilterResponse(IList<BotDto> bots) {
        Bots = bots;
    }
}