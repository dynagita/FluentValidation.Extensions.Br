using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentValidator.Extensions.Br.Tests.Api.Infrastructure;

public class DescriptionEnumConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(DescriptionEnumConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}

public class DescriptionEnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
    private readonly Dictionary<string, T> _descriptionToEnum;
    private readonly Dictionary<T, string> _enumToDescription;

    public DescriptionEnumConverter()
    {
        _descriptionToEnum = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        _enumToDescription = new Dictionary<T, string>();

        foreach (var value in Enum.GetValues<T>())
        {
            var member = typeof(T).GetMember(value.ToString())[0];
            var description = member.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
            _descriptionToEnum[description] = value;
            _enumToDescription[value] = description;
        }
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (str is not null && _descriptionToEnum.TryGetValue(str, out var value))
            return value;

        throw new JsonException($"Value '{str}' is not valid for enum {typeof(T).Name}. Valid values: {string.Join(", ", _descriptionToEnum.Keys)}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(_enumToDescription[value]);
    }
}
