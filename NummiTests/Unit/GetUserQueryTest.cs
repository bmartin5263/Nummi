using Nummi.Core.Database.Common;
using Nummi.Core.Domain.New.Queries;
using Nummi.Core.Domain.New.User;
using NummiTests.Utils;

namespace NummiTests.Unit; 

public class GetUserQueryTest {

    private IUserRepository userRepository = null!;
    private GetUserQuery subject = null!;

    [SetUp]
    public void Setup() {
        userRepository = new TestUserRepository();
        subject = new GetUserQuery(userRepository);
    }

    [Test]
    public void GetUser_ShouldReturnUserById() {
        userRepository.Add(new NummiUser());
        var user = userRepository.Add(new NummiUser());
        userRepository.Add(new NummiUser());
        var id = user.Id;

        var result = subject.Execute(id.ToString());
        
        Assert.That(result, Is.EqualTo(user));
    }
    
}