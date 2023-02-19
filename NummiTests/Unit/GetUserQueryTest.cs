using Nummi.Core.Database.Common;
using Nummi.Core.Domain.New;
using Nummi.Core.Domain.New.Queries;
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

        var result = subject.Execute(id);
        
        Assert.That(result, Is.EqualTo(user));
    }
    
}