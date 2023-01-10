namespace Nummi.Core.Domain.Crypto.Bot.Execution.Command; 

public interface ICommand {
    public void Execute(BotThread.BotThreadController controller);
}