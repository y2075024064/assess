namespace Assess.Model.Result;

/// <summary>
/// 题目专项分页结果模型
/// </summary>
public class TopicPartPageResult
{
    public int Id { get; set; }

    /// <summary>
    /// 父级ID
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// 类型名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 原名称
    /// </summary>
    public string? OldName { get; set; }

    /// <summary>
    /// 推荐年级数组
    /// </summary>
    public string? RcmdGrade { get; set; }

    /// <summary>
    /// 最近一次修改时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }

    /// <summary>
    /// 排序级别
    /// </summary>
    public int SortLevel { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 是否有子节点
    /// </summary>
    public bool HasChildren { get; set; } = true;

    /// <summary>
    /// 子节点集合
    /// </summary>
    public List<TopicPartPageResult>? Children { get; set; }
}

/// <summary>
/// 题目专项树结果模型
/// </summary>
public class TopicPartTreeResult
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsLeaf { get; set; }
}