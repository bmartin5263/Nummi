using Nummi.Core.App.User;
using Nummi.Core.Bridge;
using Nummi.Core.Domain.User;

namespace IntegrationTests.Utils; 

public class NummiAutoRollbackTestScope : INummiScope {
    private NummiTestScope Delegate { get; }
    private AutoRollback MyAutoRollback { get; }

    public NummiAutoRollbackTestScope(NummiTestScope @delegate) {
        Delegate = @delegate;
        MyAutoRollback = Delegate.AutoRollback();
    }

    public void Dispose() {
        MyAutoRollback.Dispose();
        Delegate.Dispose();
    }

    public T GetScoped<T>() where T : notnull {
       return Delegate.GetScoped<T>();
    }

    public async Task<NummiUser> CreateUserAsync(
        string username = "nummitest", 
        string email = "nummitest@example.com", 
        string password = "Password1!"
    ) {
        var response = await GetScoped<RegisterCommand>().ExecuteAsync(new RegisterCommandParameters(
            Username: username,
            Email: email,
            Password: password)
        );
        if (!response.Success) {
            throw new Exception("Failed to create test user");
        }
        return response.User!;
    }
}