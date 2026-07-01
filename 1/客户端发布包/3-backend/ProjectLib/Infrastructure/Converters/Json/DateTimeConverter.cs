using Infrastructure.Extensions;
using System.Globalization;
using Newtonsoft.Json;

namespace Infrastructure.Converters;

/// <summary>
/// DateTime Json转换器
/// </summary>
public class DateTimeConverter : JsonConverter<DateTime?>
{
    public virtual string StringFormat { get; private set; }

    /// <summary>
    /// DateTime Json转换器
    /// </summary>
    public DateTimeConverter()
    {
        StringFormat = "yyyy-MM-dd HH:mm:ss";
    }

    public DateTimeConverter(string format)
    {
        StringFormat = format;
    }

    public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
    {
        if (value == null) { writer.WriteValue(""); }
        else { writer.WriteValue(((DateTime)value).ToString(StringFormat, CultureInfo.InvariantCulture)); }
    }

    public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string? value = reader.Value?.ToString();
        if (!value.IsEmpty() && DateTime.TryParse(value, out DateTime dateTime))
        {
            return dateTime;
        }
        else if (objectType.Name == "DateTime")
        {
            return new DateTime();
        }

        return null;
    }
}