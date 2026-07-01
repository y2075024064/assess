using Infrastructure.Extensions;
using System.Globalization;
using Newtonsoft.Json;

namespace Infrastructure.Converters;

/// <summary>
/// TimeOnly Json转换器
/// </summary>
public class TimeOnlyConverter : JsonConverter<TimeOnly?>
{
    public virtual string StringFormat { get; private set; }

    /// <summary>
    /// TimeOnly Json转换器
    /// </summary>
    public TimeOnlyConverter()
    {
        StringFormat = "HH:mm:ss";
    }

    public TimeOnlyConverter(string format)
    {
        StringFormat = format;
    }

    public override void WriteJson(JsonWriter writer, TimeOnly? value, JsonSerializer serializer)
    {
        if (value == null) { writer.WriteValue(""); }
        else { writer.WriteValue(((TimeOnly)value).ToString(StringFormat, CultureInfo.InvariantCulture)); }
    }

    public override TimeOnly? ReadJson(JsonReader reader, Type objectType, TimeOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string? value = reader.Value?.ToString();
        if (!value.IsEmpty() && TimeOnly.TryParse(value, out TimeOnly time))
        {
            return time;
        }
        else if (objectType.Name == "TimeOnly")
        {
            return new TimeOnly();
        }

        return null;
    }
}