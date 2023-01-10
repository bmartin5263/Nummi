using KSUID;

namespace Nummi.Core.Domain.Crypto.Bot.Execution.Command; 

public class AssignBotCommand : ICommand {
    private Ksuid BotId { get; }
    
    public AssignBotCommand(Ksuid botId) {
        BotId = botId;
    }
    
    public void Execute(BotThread.BotThreadController controller) {
        controller.AssignBot(BotId);
    }
}