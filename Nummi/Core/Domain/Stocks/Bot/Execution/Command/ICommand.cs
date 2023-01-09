namespace Nummi.Core.Domain.Stocks.Bot.Execution.Command; 

public interface ICommand {
    public void Execute(BotThread.BotThreadController botThread);
}