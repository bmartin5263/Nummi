namespace Nummi.Core.Domain.Crypto.Bot; 

public class BotErrorHistory {
    public List<BotError> History { get; private set; } = new List<BotError>();
    public void Add(BotError error) {
        History.Add(error);
    }
}