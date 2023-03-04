using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Bots;
using Nummi.Core.Exceptions;

namespace Nummi.Core.App.Commands;

public class ReactivateBotCommand {
    private IBotRepository BotRepository { get; }
    
    public ReactivateBotCommand(IBotRepository botRepository) {
        BotRepository = botRepository;
    }

    public void Execute(BotId botId) {
        var bot = BotRepository.FindById(botId);
        
        if (!bot.InErrorState) {
            throw new InvalidUserArgumentException("Bot is not in error state");
        }

        bot.Reactivate();
        BotRepository.Commit();
    }
    
}