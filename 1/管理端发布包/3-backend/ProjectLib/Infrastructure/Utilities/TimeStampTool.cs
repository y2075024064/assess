namespace Infrastructure.Utilities;

/// <summary>
/// 时间戳工具类
/// 作者：官伟
/// </summary>
public static class TimeStampTool
{
    #region 获取时间戳

    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <param name="timeKind">时间类型（只能为 Local、Utc）</param>
    /// <param name="format">时间戳格式（只能为 10、13）</param>
    /// <returns></returns>
    private static double GetTimestamp(int format, DateTimeKind timeKind)
    {
        var timeSpan = timeKind switch
        {
            DateTimeKind.Utc => DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, timeKind),
            DateTimeKind.Local => DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, timeKind),
            _ => throw new Exception("时间类型 只能为 Local、Utc"),
        };

        return format switch
        {
            10 => timeSpan.TotalSeconds,
            13 => timeSpan.TotalMilliseconds,
            _ => throw new Exception("时间戳格式 只能为 10、13"),
        };
    }

    /// <summary>
    /// 获取10位时间戳
    /// </summary>
    /// <param name="timeKind">时间类型（只能为 Local、Utc，默认 Local）</param>
    /// <returns></returns>
    public static uint Get10Timestamp(DateTimeKind timeKind = DateTimeKind.Local)
    {
        return Convert.ToUInt32(GetTimestamp(10, timeKind));
    }

    /// <summary>
    /// 获取13位时间戳
    /// </summary>
    /// <param name="timeKind">时间类型（只能为 Local、Utc，默认 Local）</param>
    /// <returns></returns>
    public static long Get13Timestamp(DateTimeKind timeKind = DateTimeKind.Local)
    {
        return Convert.ToInt64(GetTimestamp(13, timeKind));
    }

    #endregion

    #region 验证时间戳

    /// <summary>
    /// 验证时间戳（10位、13位皆可）
    /// </summary>
    /// <param name="timestamp">时间戳</param>
    /// <param name="timeDiff">允许时差（10位时单位为 秒，13位时单位为 毫秒）</param>
    /// <param name="timeKind">时间类型（只能为 Local、Utc，默认 Local）</param>
    /// <returns></returns>
    public static bool ValidateTimestamp(double timestamp, int timeDiff, DateTimeKind timeKind = DateTimeKind.Local)
    {
        TimeSpan timeSpan = timeKind switch
        {
            DateTimeKind.Utc => DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, timeKind),
            DateTimeKind.Local => DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, timeKind),
            _ => throw new Exception("时间类型 只能为 Local、Utc"),
        };

        int format = timestamp.ToString("f0").Length;

        //现在的时间戳
        double nowTimestamp = format switch
        {
            10 => timeSpan.TotalSeconds,
            13 => timeSpan.TotalMilliseconds,
            _ => throw new Exception("时间戳格式 错误"),
        };
        double nowTimeDiff = nowTimestamp - timestamp;  //现在的时差

        return -timeDiff <= nowTimeDiff && nowTimeDiff <= timeDiff;
    }

    #endregion

    #region 转换时间戳

    /// <summary>
    /// 将时间戳装换为DateTime（10位、13位皆可）
    /// </summary>
    /// <param name="timestamp">时间戳</param>
    /// <param name="timeKind">时间类型（只能为 Local、Utc，默认 Local）</param>
    /// <param name="toTimeKind">返回的时间类型（只能为 Local、Utc，默认与 timeKind 一致）</param>
    /// <returns></returns>
    public static DateTime TimestampToDateTime(double timestamp, DateTimeKind timeKind = DateTimeKind.Local, DateTimeKind toTimeKind = DateTimeKind.Utc)
    {
        var startTime = timeKind switch
        {
            DateTimeKind.Utc => new DateTime(1970, 1, 1, 0, 0, 0, timeKind),
            DateTimeKind.Local => new DateTime(1970, 1, 1, 0, 0, 0, timeKind),
            _ => throw new Exception("时间类型 只能为 Local、Utc"),
        };

        int format = timestamp.ToString("f0").Length;

        var newTime = format switch
        {
            10 => startTime.AddSeconds(timestamp),
            13 => startTime.AddMilliseconds(timestamp),
            _ => throw new Exception("时间戳格式 错误"),
        };

        if (newTime.Kind != toTimeKind)
        {
            newTime = toTimeKind == DateTimeKind.Local ? newTime.ToLocalTime() : newTime.ToUniversalTime();
        }

        return newTime;
    }

    #endregion
}