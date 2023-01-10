using KSUID;

namespace Nummi.Core.Domain.Stocks.Bot.Execution.Command; 

public class RemoveBotCommand : ICommand {
    public void Execute(BotThread.BotThreadController controller) {
        controller.RemoveBot();
    }
}