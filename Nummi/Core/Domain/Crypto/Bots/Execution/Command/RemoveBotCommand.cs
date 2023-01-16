namespace Nummi.Core.Domain.Crypto.Bots.Execution.Command; 

public class RemoveBotCommand : ICommand {
    public void Execute(BotThread.BotThreadController controller) {
        controller.RemoveBot();
    }
}