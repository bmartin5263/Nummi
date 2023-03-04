using Nummi.Core.Database.Common;
using Nummi.Core.Domain.User;

namespace Nummi.Core.App.Queries; 

public class GetUserQuery {
    private IUserRepository UserRepository { get; }
    private IStrategyTemplateRepository StrategyTemplateRepository { get; }
    
    public GetUserQuery(IUserRepository userRepository, IStrategyTemplateRepository strategyTemplateRepository) {
        UserRepository = userRepository;
        StrategyTemplateRepository = strategyTemplateRepository;
    }

    public NummiUser Execute(IdentityId userId) {
        var user = UserRepository.FindByIdWithAllDetails(userId);
        return user;
    }
    
}