namespace Nummi.Core.Domain.Crypto.Bots.Thread.Command; 

public class RemoveBotCommand : ICommand {
    public void Execute(BotThread.BotThreadController controller) {
        controller.RemoveBot();
    }
}