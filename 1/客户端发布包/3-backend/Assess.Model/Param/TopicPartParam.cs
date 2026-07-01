using System.ComponentModel.DataAnnotations;

namespace Assess.Model.Param;

/// <summary>
/// 题目科目新增参数模型
/// </summary>
public class TopicPartInsertParam
{
    /// <summary>
    /// 父级ID
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// 类型名称
    /// </summary>
    [StringLength(32)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 推荐年级数组
    /// </summary>
    public string[]? RcmdGrade { get; set; }

    /// <summary>
    /// 排序级别
    /// </summary>
    public int SortLevel { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 题目科目更改参数模型
/// </summary>
public class TopicPartUpdateParam : TopicPartInsertParam
{
    /// <summary>
    /// 题目科目ID
    /// </summary>
    public int Id { get; set; }
}
