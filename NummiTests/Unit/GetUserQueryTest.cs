using Nummi.Core.Database.Common;
using Nummi.Core.Domain.New;
using Nummi.Core.Domain.New.Queries;
using NummiTests.Utils;

namespace NummiTests.Unit; 

public class GetUserQueryTest {

    private ITransaction? transaction;
    private GetUserQuery? subject;

    [SetUp]
    public void Setup() {
        transaction = UnitTestUtils.CreateTestTransaction();
        subject = new GetUserQuery(transaction.UserRepository);
    }

    [Test]
    public void GetUser_ShouldReturnUserById() {
        transaction!.UserRepository.Add(new NummiUser());
        var user = transaction!.UserRepository.Add(new NummiUser());
        transaction!.UserRepository.Add(new NummiUser());
        var id = user.Id;

        var result = subject!.Execute(id);
        
        Assert.That(result, Is.EqualTo(user));
    }
    
}