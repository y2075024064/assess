namespace Assess;

/// <summary>
/// 咨询信件状态枚举
/// </summary>
public enum LetterState : byte
{
    /// <summary>
    /// 新信件
    /// </summary>
    New,

    /// <summary>
    /// 已查看
    /// </summary>
    Viewed,

    /// <summary>
    /// 处理中
    /// </summary>
    Processing,

    /// <summary>
    /// 已解决
    /// </summary>
    Resolved,

    /// <summary>
    /// 会话结束
    /// </summary>
    Finished
}
