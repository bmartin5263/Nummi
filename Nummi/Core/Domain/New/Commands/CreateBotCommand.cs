using Nummi.Core.Database.Common;
using Nummi.Core.Domain.New.User;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New.Commands;

public record CreateBotParameters {
    public required string Name { get; init; }
    public required TradingMode Mode { get; init; }
    public decimal? Funds { get; init; }
}

public class CreateBotCommand {
    
    private IUserRepository UserRepository { get; }
    
    public CreateBotCommand(IUserRepository userRepository) {
        UserRepository = userRepository;
    }

    public Bot Execute(string userId, CreateBotParameters parameters) {
        var user = UserRepository.FindById(userId.ToKsuid())
            .OrElseThrow(() => EntityNotFoundException<NummiUser>.IdNotFound(userId));
        
        var bot = new Bot(parameters.Name, parameters.Mode, parameters.Funds ?? 0.0m);
        user.Bots.Add(bot);
        
        UserRepository.Commit();
        return bot;
    }
    
}