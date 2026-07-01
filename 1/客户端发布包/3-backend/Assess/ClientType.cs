namespace Assess;

/// <summary>
/// 客户端类型
/// </summary>
public enum ClientType : byte
{
    /// <summary>
    /// 未知
    /// </summary>
    Unknown,

    /// <summary>
    /// 小程序
    /// </summary>
    Mini,

    /// <summary>
    /// 移动H5
    /// </summary>
    MobileH5,

    /// <summary>
    /// PCH5
    /// </summary>
    PCH5,

    /// <summary>
    /// 其他
    /// </summary>
    Other
}