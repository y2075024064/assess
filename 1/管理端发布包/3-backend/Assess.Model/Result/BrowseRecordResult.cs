namespace Assess.Model.Result;

/// <summary>
/// 浏览记录信息模型
/// </summary>
public class BrowseRecordInfo
{
    /// <summary>
    /// 是否点赞
    /// </summary>
    public bool IsGiveLike { get; set; }

    /// <summary>
    /// 是否收藏
    /// </summary>
    public bool IsCollection { get; set; }

    /// <summary>
    /// 最后浏览时间
    /// </summary>
    public DateTime LastBrowseOn { get; set; }
}
