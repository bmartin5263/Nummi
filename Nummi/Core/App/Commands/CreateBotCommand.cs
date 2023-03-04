using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Bots;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Domain.User;

namespace Nummi.Core.App.Commands;

public record CreateBotParameters {
    public required IdentityId UserId { get; init; }
    public required string Name { get; init; }
    public required TradingMode Mode { get; init; }
    public decimal? Funds { get; init; }
}

public class CreateBotCommand {
    
    private IUserRepository UserRepository { get; }
    
    public CreateBotCommand(IUserRepository userRepository) {
        UserRepository = userRepository;
    }

    public Bot Execute(CreateBotParameters parameters) {
        var user = UserRepository.FindById(parameters.UserId);
        var bot = new Bot(parameters.Name, parameters.Mode, parameters.Funds ?? 0.0m);
        user.Bots.Add(bot);
        UserRepository.Commit();
        return bot;
    }
    
}