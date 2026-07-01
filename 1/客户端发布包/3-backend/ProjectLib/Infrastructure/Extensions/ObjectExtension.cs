using Newtonsoft.Json;

namespace Infrastructure.Extensions;

/// <summary>
/// 对象扩展
/// </summary>
public static class ObjectExtension
{
    /// <summary>
    /// 对象转 Json 字符串
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static string ToJson(this object obj, JsonSerializerSettings? settings = null)
    {
        settings ??= new JsonSerializerSettings() { Formatting = Formatting.None };
        return JsonConvert.SerializeObject(obj, settings);
    }

    /// <summary>
    /// 对象转 Json 字符串
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="value"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static string ToJson<TValue>(this TValue value, JsonSerializerSettings? settings = null) where TValue : class
    {
        settings ??= new JsonSerializerSettings() { Formatting = Formatting.None };
        return JsonConvert.SerializeObject(value, settings);
    }

    /// <summary>
    /// 将 Json 字符串转为动态对象
    /// </summary>
    /// <param name="json"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static object? ToModel(this string? json, JsonSerializerSettings? settings = null)
    {
        if (json == null) { return null; }
        return settings == null ? JsonConvert.DeserializeObject(json) : JsonConvert.DeserializeObject(json, settings);
    }

    /// <summary>
    /// 将 Json 字符串转为对象
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="json"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static TValue? ToModel<TValue>(this string? json, JsonSerializerSettings? settings = null) where TValue : class
    {
        if (json == null) { return null; }
        return JsonConvert.DeserializeObject<TValue>(json, settings);
    }
}