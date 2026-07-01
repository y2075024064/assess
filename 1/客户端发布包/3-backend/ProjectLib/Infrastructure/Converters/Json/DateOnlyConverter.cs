using Infrastructure.Extensions;
using System.Globalization;
using Newtonsoft.Json;

namespace Infrastructure.Converters;

/// <summary>
/// DateOnly Json转换器
/// </summary>
public class DateOnlyConverter : JsonConverter<DateOnly?>
{
    public virtual string StringFormat { get; private set; }

    /// <summary>
    /// DateOnly Json转换器
    /// </summary>
    public DateOnlyConverter()
    {
        StringFormat = "yyyy-MM-dd";
    }

    public DateOnlyConverter(string format)
    {
        StringFormat = format;
    }

    public override void WriteJson(JsonWriter writer, DateOnly? value, JsonSerializer serializer)
    {
        if (value == null) { writer.WriteValue(""); }
        else { writer.WriteValue(((DateOnly)value).ToString(StringFormat, CultureInfo.InvariantCulture)); }
    }

    public override DateOnly? ReadJson(JsonReader reader, Type objectType, DateOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string? value = reader.Value?.ToString();
        if (!value.IsEmpty() && DateOnly.TryParse(value, out DateOnly date))
        {
            return date;
        }
        else if (objectType.Name == "DateOnly")
        {
            return new DateOnly();
        }

        return null;
    }
}