using Nummi.Core.Bridge;
using Nummi.Core.Bridge.DotNet;

namespace IntegrationTests.Utils; 

public class IntegrationTest {
    private CustomWebApplicationFactory WebApplicationFactory { get; }
    private INummiServiceProvider? ServiceProvider { get; set; }
    
    private HttpClient? client;
    protected HttpClient Client {
        get {
            client ??= WebApplicationFactory.CreateClient();
            return client;
        }
    }

    protected IntegrationTest() {
        WebApplicationFactory = new CustomWebApplicationFactory();
    }

    protected T GetSingleton<T>() where T : notnull {
        ServiceProvider ??= new DotNetServiceProvider(WebApplicationFactory.Services);
        return ServiceProvider.GetSingleton<T>();
    }

    protected NummiTestScope CreateScope() {
        ServiceProvider ??= new DotNetServiceProvider(WebApplicationFactory.Services);
        return new NummiTestScope(ServiceProvider.CreateScope());
    }
    
    protected NummiAutoRollbackTestScope CreateAutoRollbackScope() {
        return new NummiAutoRollbackTestScope(CreateScope());
    }
}