using System.ComponentModel.DataAnnotations;

namespace Assess.Model.Param;

/// <summary>
/// 测评方案添加参数模型
/// </summary>
public class TestPlanInsertParam
{
    /// <summary>
    /// 专项ID
    /// </summary>
    [Required]
    public int PartId { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    [Required]
    [Range(50, 150)]
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 答题时长（秒）
    /// </summary>
    [Range(60, 10800)]
    public int? TestTime { get; set; }

    /// <summary>
    /// 题目数量
    /// </summary>
    [Required]
    [Range(10, 200)]
    public int TopicNumber { get; set; }
}

/// <summary>
/// 测评方案更新参数模型
/// </summary>
public class TestPlanUpdateParam : TestPlanInsertParam
{
    public int Id { get; set; }
}