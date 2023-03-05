using Nummi.Core.App.Queries;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.User;
using NummiTests.Utils;

namespace NummiTests.Tests.Unit; 

public class GetUserQueryTest {

    private IUserRepository userRepository = null!;
    private IStrategyTemplateRepository strategyTemplateRepository = null!;
    private GetUserQuery subject = null!;

    [SetUp]
    public void Setup() {
        userRepository = new TestUserRepository();
        strategyTemplateRepository = new StrategyTemplateTestRepository();
        subject = new GetUserQuery(userRepository, strategyTemplateRepository);
    }

    [Test]
    public void GetUser_ShouldReturnUserById() {
        userRepository.Add(new NummiUser());
        var user = userRepository.Add(new NummiUser());
        userRepository.Add(new NummiUser());

        var result = subject.Execute(user.Id);
        
        Assert.That(result, Is.EqualTo(user));
    }
    
}