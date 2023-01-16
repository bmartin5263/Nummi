namespace Nummi.Core.Domain.Crypto.Bots.Execution.Command; 

public interface ICommand {
    public void Execute(BotThread.BotThreadController controller);
}