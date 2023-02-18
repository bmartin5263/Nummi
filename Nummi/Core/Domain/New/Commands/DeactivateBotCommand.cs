using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New.Commands;

public class DeactivateBotCommand {
    private ITransaction Transaction { get; }
    
    public DeactivateBotCommand(ITransaction transaction) {
        Transaction = transaction;
    }

    public void Execute(Ksuid botId) {
        var bot = Transaction.BotRepository.FindById(botId)
            .OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(botId));

        if (!bot.IsActive) {
            return;
        }
        
        bot.Deactivate(); // Domain Event BotDeactivated
        Transaction.SaveAndDispose();
    }
    
}