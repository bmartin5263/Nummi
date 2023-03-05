using Nummi.Core.Util;

namespace IntegrationTests.Utils; 

public static class Extensions {
    public static T ReadJson<T>(this HttpResponseMessage response) {
        return Serializer.FromJson<T>(response.Content.ReadAsStringAsync().Result)!;
    }
}