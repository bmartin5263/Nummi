using Nummi.Core.Database.Common;
using Nummi.Core.Domain.User;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New.Commands;

public record CreateBotParameters {
    public required string Name { get; init; }
    public required TradingMode Mode { get; init; }
    public decimal? Funds { get; init; }
}

public class CreateBotCommand {
    private ITransaction Transaction { get; }
    
    public CreateBotCommand(ITransaction transaction) {
        Transaction = transaction;
    }

    public Bot Execute(string userId, CreateBotParameters parameters) {
        var user = Transaction.UserRepository.FindById(userId)
            .OrElseThrow(() => EntityNotFoundException<NummiUser>.IdNotFound(userId));
        
        var bot = new Bot(parameters.Name, parameters.Mode, parameters.Funds ?? 0.0m);
        user.Bots.Add(bot);
        
        Transaction.SaveAndDispose();
        return bot;
    }
    
}