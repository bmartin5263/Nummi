namespace Nummi.Core.Domain.Crypto.Bots.Thread.Command; 

public class AssignBotCommand : ICommand {
    private string BotId { get; }
    
    public AssignBotCommand(string botId) {
        BotId = botId;
    }
    
    public void Execute(BotThread.BotThreadController controller) {
        controller.AssignBot(BotId);
    }
}