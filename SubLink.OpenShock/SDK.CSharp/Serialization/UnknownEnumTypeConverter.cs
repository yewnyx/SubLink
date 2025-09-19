using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenShock.SDK.CSharp.Serialization;

public class UnknownEnumTypeConverter<T> : JsonConverter<T> where T : struct, Enum {
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType != JsonTokenType.String) throw new JsonException("Expected string value");
        var value = reader.GetString()!;

        if (Enum.TryParse<T>(value, out var result)) return result;

        // Default to last value
#if NETSTANDARD
        return Enum.GetValues(typeof(T)).Cast<T>().Last();
#else
        return Enum.GetValues<T>().Last();
#endif
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString());
}