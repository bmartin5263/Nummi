using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using CsvHelper;

namespace Nummi.Core.Util; 

public class Serializer {
    private const string TYPE_FIELD = "$type";

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
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        return JsonSerializer.Serialize(value, serializeOptions);
    }

    public static T? FromJson<T>(string json) {
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        return JsonSerializer.Deserialize<T>(json, serializeOptions);
    }

    public static T? FromJson<T>(string json, Type type) {
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        return (T?) JsonSerializer.Deserialize(json, type, serializeOptions);
    }

    public static T? FromJson<T>(JsonNode? node, Type type) {
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        return (T?) node.Deserialize(type, serializeOptions);
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
            node.Add(TYPE_FIELD, value!.GetType().Name);
            node.WriteTo(writer);
        }
    }
}
