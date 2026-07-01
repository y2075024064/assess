namespace Assess.Model.Result;

/// <summary>
/// 年级信息分页结果模型
/// </summary>
public class GradePageResult
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
    /// 排序级别
    /// </summary>
    public int SortLevel { get; set; }

    /// <summary>
    /// 是否使用
    /// </summary>
    public bool IsUse { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool IsShow { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}
