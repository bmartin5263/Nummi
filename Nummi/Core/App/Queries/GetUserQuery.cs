using Nummi.Core.Database.Common;
using Nummi.Core.Domain.New.User;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.App.Queries; 

public class GetUserQuery {
    private IUserRepository UserRepository { get; }
    
    public GetUserQuery(IUserRepository userRepository) {
        UserRepository = userRepository;
    }

    public NummiUser Execute(string userId) {
        var user = UserRepository.FindById(userId.ToKsuid())
            .OrElseThrow(() => EntityNotFoundException<NummiUser>.IdNotFound(userId));
        
        UserRepository.LoadCollection(user, u => u.Bots);
        UserRepository.LoadCollection(user, u => u.Simulations);

        return user;
    }
    
}