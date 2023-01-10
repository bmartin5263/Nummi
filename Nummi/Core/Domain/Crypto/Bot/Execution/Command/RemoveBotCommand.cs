namespace Nummi.Core.Domain.Crypto.Bot.Execution.Command; 

public class RemoveBotCommand : ICommand {
    public void Execute(BotThread.BotThreadController controller) {
        controller.RemoveBot();
    }
}