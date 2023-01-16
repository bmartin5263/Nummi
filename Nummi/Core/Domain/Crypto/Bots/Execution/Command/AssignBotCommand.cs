namespace Nummi.Core.Domain.Crypto.Bots.Execution.Command; 

public class AssignBotCommand : ICommand {
    private string BotId { get; }
    
    public AssignBotCommand(string botId) {
        BotId = botId;
    }
    
    public void Execute(BotThread.BotThreadController controller) {
        controller.AssignBot(BotId);
    }
}