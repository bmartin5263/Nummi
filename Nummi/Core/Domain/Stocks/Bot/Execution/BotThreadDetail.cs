using KSUID;

namespace Nummi.Core.Domain.Stocks.Bot.Execution; 

public class BotThreadDetail {
    public uint Id { get; }
    public string? BotId { get; }

    public BotThreadDetail(uint id, string? botId) {
        Id = id;
        BotId = botId;
    }
}