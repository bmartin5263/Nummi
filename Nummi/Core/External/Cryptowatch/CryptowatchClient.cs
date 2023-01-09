using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Nummi.Core.External.Cryptowatch; 

public class CryptowatchClient {

    private readonly HttpClient client = new HttpClient();

    public CryptowatchClient() {
        client.BaseAddress = new Uri("https://api.cryptowat.ch/markets/kraken");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<global::Coinbase.Models.Response<PriceResponse>> GetPriceAsync() {
        HttpResponseMessage response = await client.GetAsync("btcusd/price");
        if (response.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<global::Coinbase.Models.Response<PriceResponse>>(await response.Content.ReadAsStringAsync())!;
        }
        throw new ExternalClientException();
    }
    
}