using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Utils; 

public class IntegrationTest {
    private readonly CustomWebApplicationFactory webApplicationFactory;

    protected IntegrationTest() {
        webApplicationFactory = new CustomWebApplicationFactory();
    }

    protected HttpClient GetClient() => webApplicationFactory.CreateClient();

    protected T GetService<T>() where T : notnull => webApplicationFactory.Services.GetRequiredService<T>();
    
    protected IServiceScope CreateScope() => webApplicationFactory.Services.CreateScope();
}