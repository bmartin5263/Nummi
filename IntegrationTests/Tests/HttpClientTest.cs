using IntegrationTests.Utils;
using Nummi.Api.Model;

namespace IntegrationTests.Tests; 

public class HttpClientTest : IntegrationTest {
    [Test]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType() {
        // Arrange
        var client = Client;

        // Act
        var response = await client.GetAsync("api/strategy-template");
        IList<StrategyTemplateDto> simulations = response.ReadJson<IEnumerable<StrategyTemplateDto>>().ToList();
        
        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.That(simulations, Has.Count.EqualTo(1));
        Assert.That(simulations[0].Name, Is.EqualTo("Opportunist"));
        // Assert.That("text/html; charset=utf-8", Is.EqualTo(response.Content.Headers.ContentType!.ToString()));
    }
}