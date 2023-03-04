using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Bots;
using Nummi.Core.Exceptions;

namespace Nummi.Core.App.Commands;

public class DeactivateBotCommand {
    private IBotRepository BotRepository { get; }
    
    public DeactivateBotCommand(IBotRepository botRepository) {
        BotRepository = botRepository;
    }

    public void Execute(BotId botId) {
        var bot = BotRepository.FindById(botId);
        if (!bot.IsActive) {
            throw new InvalidUserArgumentException("Bot is not active");
        }
        
        bot.Deactivate(); // Domain Event BotDeactivated
        BotRepository.Commit();
    }
    
}