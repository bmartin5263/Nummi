namespace Nummi.Core.Domain.Crypto.Bots.Thread.Command; 

public interface ICommand {
    public void Execute(BotThread.BotThreadController controller);
}