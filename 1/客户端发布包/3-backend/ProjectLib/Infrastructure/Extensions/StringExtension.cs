namespace Infrastructure.Extensions;

/// <summary>
/// String 扩展类
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// string.IsNullOrWhiteSpace 方法的扩展用法
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsEmpty(this string? str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// !string.IsNullOrWhiteSpace 方法的扩展用法
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsNotEmpty(this string? str)
    {
        return !string.IsNullOrWhiteSpace(str);
    }
}