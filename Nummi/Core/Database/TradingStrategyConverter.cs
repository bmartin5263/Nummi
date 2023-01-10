using Coinbase.Models;
using KSUID;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Nummi.Core.Domain.Stocks.Bot.Strategy;

namespace Nummi.Core.Database;

public class KsuidConverter : ValueConverter<Ksuid, string> {
    public KsuidConverter()
        : base(
            v => v.ToString(),
            v => Ksuid.FromString(v)
        )
    {
    }
}

public class GenericJsonConverter<T> : ValueConverter<T, string> {
    public GenericJsonConverter()
        : base(
            v => JsonConvert.SerializeObject(v, new JsonSerializerSettings 
            { 
                NullValueHandling = NullValueHandling.Ignore
            }),
            v => JsonConvert.DeserializeObject<T>(v, new JsonSerializerSettings 
            {
                NullValueHandling = NullValueHandling.Ignore
            })!
        )
    {
    }
}

public class TradingStrategyConverter : ValueConverter<ITradingStrategy, string> {
    public TradingStrategyConverter()
        : base(
            v => JsonConvert.SerializeObject(v, new JsonSerializerSettings 
            { 
                NullValueHandling = NullValueHandling.Ignore,
                Converters = { new TradingStrategyJsonConverter() } 
            }),
            v => JsonConvert.DeserializeObject<ITradingStrategy>(v, new JsonSerializerSettings 
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = { new TradingStrategyJsonConverter() }
            })!
        )
    {
    }
}

public class TradingStrategyJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) {
        return typeof(ITradingStrategy).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JObject jo = JObject.Load(reader);
        var typeProp = jo["Type"];
        if (typeProp == null) {
            throw new MissingFieldException("Failed to load Trading Strategy from DB. Missing 'Type' field on Trading Strategy");
        }
        var type = jo["Type"]!.Value<string>()!;
        return TradingStrategyFactory.Create(type);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        JToken t = JToken.FromObject(value);

        if (t.Type != JTokenType.Object)
        {
            t.WriteTo(writer);
        }
        else
        {
            JObject o = (JObject)t;
            o.AddFirst(new JProperty("Type", new JValue(value.GetType().FullName)));
            o.WriteTo(writer);
        }
    }
}