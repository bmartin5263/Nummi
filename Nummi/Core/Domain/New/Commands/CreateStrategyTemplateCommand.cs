using Nummi.Core.Database.Common;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New.Commands;

public record DefineCSharpStrategyTemplateParameters {
    public required string Name { get; init; }
    public required TradingMode Mode { get; init; }
    public decimal? Funds { get; init; }
}

public class DefineCSharpStrategyTemplateCommand {
    
    private IUserRepository UserRepository { get; }
    
    public DefineCSharpStrategyTemplateCommand(IUserRepository userRepository) {
        UserRepository = userRepository;
    }

    public Bot Execute(string userId, DefineCSharpStrategyTemplateParameters parameters) {
        var user = UserRepository.FindById(userId)
            .OrElseThrow(() => EntityNotFoundException<NummiUser>.IdNotFound(userId));
        
        var bot = new Bot(parameters.Name, parameters.Mode, parameters.Funds ?? 0.0m);
        user.Bots.Add(bot);
        
        UserRepository.Commit();
        return bot;
    }
    
}