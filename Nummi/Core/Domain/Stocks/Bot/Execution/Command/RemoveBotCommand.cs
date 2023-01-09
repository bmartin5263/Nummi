using KSUID;

namespace Nummi.Core.Domain.Stocks.Bot.Execution.Command; 

public class RemoveBotCommand : ICommand {
    public void Execute(BotThread.BotThreadController botThread) {
        botThread.RemoveBot();
    }
}