using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Util;

namespace Nummi.Core.External.Binance; 

public class BinanceClient {
    
    public IEnumerable<HistoricalPrice> GetSpotPrice(IEnumerable<string> symbols) {
        var client = new HttpClient();
        
        var queryString = '[' + string.Join(",", symbols.Select(s => $"\"{s}\"")) + ']';
        Console.WriteLine(queryString);
        
        string url = $"https://api.binance.us/api/v3/ticker/price?symbols={queryString}";
        var response = client.GetAsync(url).Result;
        var responseString = response.Content.ReadAsStringAsync().Result;

        return Serializer.FromJson<IList<BinancePrice>>(responseString)!
            .Select(v => v.ToHistoricalPrice());
    }
    
    public IEnumerable<HistoricalPrice> GetSpotPrice() {
        var client = new HttpClient();

        string url = $"https://api.binance.us/api/v3/ticker/price";
        var response = client.GetAsync(url).Result;
        var responseString = response.Content.ReadAsStringAsync().Result;

        return Serializer.FromJson<IList<BinancePrice>>(responseString)!
            .Select(v => v.ToHistoricalPrice());
    }

    private global::Binance.Net.Clients.BinanceClient Client { get; } = new() {
        // ClientOptions = {
        //     ApiCredentials = new ApiCredentials("", "")
        // }
    };

}