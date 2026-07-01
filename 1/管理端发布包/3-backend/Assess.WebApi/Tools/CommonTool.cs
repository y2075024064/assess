using Infrastructure.Utilities;

namespace Assess.WebApi;

/// <summary>
/// 公共工具类
/// </summary>
public class CommonTool
{
    private static Random _random = new();

    /// <summary>
    /// 创建用户随机账户名
    /// </summary>
    /// <returns></returns>
    public static string CreateAccount()
    {
        var timestamp = TimeStampTool.Get10Timestamp();
        var random = new Random(DateTime.Now.Millisecond);
        string str = timestamp.ToString() + random.Next(100, 1000).ToString();
        long num = long.Parse(str);

        return "Ac" + BaseConversion.TenTo62(num);
    }

    /// <summary>
    /// 创建用户昵称
    /// </summary>
    /// <returns></returns>
    public static string CreateNickname()
    {
        return "Nc" + BaseConversion.TenTo62(DateTime.Now.Ticks);
    }

    /// <summary>
    /// 创建编号
    /// </summary>
    /// <param name="prefix">前缀</param>
    /// <returns></returns>
    public static string CreateNumber(string prefix)
    {
        long timestamp = TimeStampTool.Get13Timestamp();
        string str = timestamp.ToString() + _random.Next(1000, 10000);
        long num = long.Parse(str);
        return prefix + BaseConversion.TenTo62(num);
    }
}
