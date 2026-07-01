using System.ComponentModel.DataAnnotations;
using Repository;

namespace Assess.Model.Param;

/// <summary>
/// 题目分页参数模型
/// </summary>
public class TopicPageParam : DefaultEntityPageParam
{
    /// <summary>
    /// 题目类型
    /// </summary>
    public TopicType? Type { get; set; }

    /// <summary>
    /// 分部ID
    /// </summary>
    public int? PartId { get; set; }

    /// <summary>
    /// 出题用户姓名
    /// </summary>
    public string? MemberName { get; set; }

    /// <summary>
    /// 用途
    /// </summary>
    public TopicUseType? UseType { get; set; }
}

/// <summary>
/// 题目创建参数模型
/// </summary>
public class TopicInsertParam
{
    /// <summary>
    /// 题目类型
    /// </summary>
    public TopicType Type { get; set; }

    /// <summary>
    /// 题目标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 题目难度，0.1 - 1.0，越小越难
    /// </summary>
    public decimal Level { get; set; } = 1;

    /// <summary>
    /// 分部ID
    /// </summary>
    public int? PartId { get; set; }

    /// <summary>
    /// 题目标签数组
    /// </summary>
    public string[]? Label { get; set; }

    /// <summary>
    /// 推荐年级数组
    /// </summary>
    public string[]? RcmdGrade { get; set; }

    /// <summary>
    /// 考察目的
    /// </summary>
    public string? Purpose { get; set; }

    /// <summary>
    /// 题目内容（HTML）
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 用途
    /// </summary>
    public TopicUseType UseType { get; set; }

    /// <summary>
    /// 正确答案（json）
    /// </summary>
    public string? RightAnswer { get; set; }

    /// <summary>
    /// 解析
    /// </summary>
    public string? Analysis { get; set; }

    /// <summary>
    /// 选项数组（每项为 HTML 文本）
    /// </summary>
    public string? Options { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    [StringLength(256)]
    public string? Origin { get; set; }

    /// <summary>
    /// 是否原创
    /// </summary>
    public bool IsOriginal { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 题目更改参数模型
/// </summary>
public class TopicUpdateParam : TopicInsertParam
{
    public uint Id { get; set; }
}