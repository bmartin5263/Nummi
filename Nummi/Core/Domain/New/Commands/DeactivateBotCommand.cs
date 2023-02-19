using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New.Commands;

public class DeactivateBotCommand {
    private IBotRepository BotRepository { get; }
    
    public DeactivateBotCommand(IBotRepository botRepository) {
        BotRepository = botRepository;
    }

    public void Execute(Ksuid botId) {
        var bot = BotRepository.FindById(botId)
            .OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(botId));

        if (!bot.IsActive) {
            return;
        }
        
        bot.Deactivate(); // Domain Event BotDeactivated
        BotRepository.Commit();
    }
    
}