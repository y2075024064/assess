namespace Assess.Model.Result;

/// <summary>
/// 题目标签分页结果模型
/// </summary>
public class TopicLabelPageResult
{
    /// <summary>
    /// 年级ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 标签原名称
    /// </summary>
    public string? OldName { get; set; }

    /// <summary>
    /// 最近一次修改时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}
