using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using CsvHelper;
using Nummi.Core.Bridge;
using Nummi.Core.Domain.Common;

namespace Nummi.Core.Util; 

public static class Serializer {
    
    private const string TYPE_FIELD = "$type";
    
    private static JsonSerializerOptions DEFAULT_OPTIONS => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        TypeInfoResolver = new PrivateConstructorContractResolver(),
        Converters = {
            new KsuidConverter()
        }
    };

    public static IEnumerable<T> FromCsv<T>(string path) {
        var reader = new StreamReader(path);
        return FromCsv<T>(reader);
    }

    public static IEnumerable<T> FromCsv<T>(Stream stream) {
        var reader = new StreamReader(stream);
        return FromCsv<T>(reader);
    }

    public static IEnumerable<T> FromCsv<T>(StreamReader reader) {
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<T>();
    }

    public static string ToJson<T>(T value) {
        return JsonSerializer.Serialize(value, DEFAULT_OPTIONS);
    }
    
    public static string? DocumentToJson(JsonDocument? document) {
        if (document == null) {
            return null;
        }
        using var stream = new MemoryStream();
        Utf8JsonWriter writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
        document.WriteTo(writer);
        writer.Flush();
        return Encoding.UTF8.GetString(stream.ToArray());
    }
    
    public static async void ToJsonAsync<T>(Stream writer, T value) { 
        await JsonSerializer.SerializeAsync(writer, value, DEFAULT_OPTIONS);
    }

    public static JsonNode? ToJsonNode(string json) {
        return JsonSerializer.SerializeToNode(json, DEFAULT_OPTIONS);
    }

    public static T? FromJson<T>(string json) {
        return JsonSerializer.Deserialize<T>(json, DEFAULT_OPTIONS);
    }

    public static JsonElement FromJson(string json) {
        return JsonSerializer.Deserialize<JsonElement>(json, DEFAULT_OPTIONS);
    }

    // public static T? FromJsonToNode<T>(string json) {
    //     var DEFAULT_OPTIONS = new JsonSerializerOptions
    //     {
    //         PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    //     };
    //     return JsonSerializer.Deserialize<T>(json, DEFAULT_OPTIONS);
    // }

    public static T? FromJson<T>(string json, Type type) {
        return (T?) JsonSerializer.Deserialize(json, type, DEFAULT_OPTIONS);
    }

    public static T? FromJson<T>(JsonNode? node, Type type) {
        return (T?) node.Deserialize(type, DEFAULT_OPTIONS);
    }

    public class AbstractTypeConverter<T> : JsonConverter<T> {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            if (reader.TokenType != JsonTokenType.StartObject) {
                throw new JsonException();
            }

            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            if (!jsonDocument.RootElement.TryGetProperty(TYPE_FIELD, out var typeProperty)) {
                throw new JsonException();
            }

            var type = Type.GetType(typeProperty.GetString()!)!;

            var jsonObject = jsonDocument.RootElement.GetRawText();
            var result = (T) JsonSerializer.Deserialize(jsonObject, type, options)!;

            return result;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
            var node = (JsonObject) JsonSerializer.SerializeToNode((object)value!, options)!;
            node.Add(TYPE_FIELD, value!.GetType().FullName);
            node.WriteTo(writer);
        }
    }

    public class KsuidConverter : JsonConverter<Ksuid> {
        public override Ksuid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            return Ksuid.FromString(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, Ksuid value, JsonSerializerOptions options) {
            writer.WriteStringValue(value.ToString());
        }
    }
    
    public class JwtConverter : JsonConverter<Jwt> {
        public override Jwt Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            return Jwt.FromString(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, Jwt value, JsonSerializerOptions options) {
            writer.WriteStringValue(value.ToString());
        }
    }
}

public class PrivateConstructorContractResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object && jsonTypeInfo.CreateObject is null) {
            if (jsonTypeInfo.Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).All(c => c.GetParameters().Length != 0)) {
                // The type doesn't have public constructors
                jsonTypeInfo.CreateObject = () => Activator.CreateInstance(jsonTypeInfo.Type, true)!;
            }
        }

        return jsonTypeInfo;
    }
}