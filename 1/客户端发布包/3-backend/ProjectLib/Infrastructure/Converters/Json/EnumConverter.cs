using Infrastructure.Extensions;
using Newtonsoft.Json;

namespace Infrastructure.Converters;

/// <summary>
/// Enum Json转换器
/// </summary>
public class EnumConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        if (objectType.IsEnum) return true;
        else if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>) && objectType.GenericTypeArguments[0].IsEnum) return true;
        else return false;
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        string? value = reader.Value?.ToString();

        try
        {
            if (objectType.GenericTypeArguments.Length > 0)
            {
                if (value.IsEmpty()) { return null; }
                objectType = objectType.GenericTypeArguments[0];
            }

            if (Enum.TryParse(objectType, value, true, out object? result))
            {
                return result;
            }

            return objectType.IsEnum ? Enum.GetValues(objectType).GetValue(0) : null;
        }
        catch
        {
            return null;
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.ToString() ?? "");
    }
}
