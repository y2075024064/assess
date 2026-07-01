using System.Text;

namespace Infrastructure.Utilities;

/// <summary>
/// 进制转换器
/// </summary>
public class BaseConversion
{
    /// <summary>
    /// 将10进制转换为62进制字符串
    /// </summary>
    /// <param name="fromNum"></param>
    /// <returns></returns>
    public static string TenTo62(long fromNum)
    {
        StringBuilder sb = new();
        long num = fromNum;
        while (num > 0)
        {
            long temp = num % 62;
            num /= 62;

            char ch;
            if (temp < 10) { ch = (char)(temp + 48); }
            else if (temp < 36) { ch = (char)(temp + 55); }
            else { ch = (char)(temp + 61); }

            sb.Insert(0, ch);
        }

        return sb.ToString();
    }
}