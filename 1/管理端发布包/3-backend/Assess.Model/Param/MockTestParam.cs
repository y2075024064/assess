using System.ComponentModel.DataAnnotations;

namespace Assess.Model.Param;

/// <summary>
/// 模拟测评答题参数模型
/// </summary>
public class MockTestAnswerParam
{
    /// <summary>
    /// 模拟测评记录Id
    /// </summary>
    [Required]
    public long TestId { get; set; }

    /// <summary>
    /// 题目编号
    /// </summary>
    [Required]
    [StringLength(12)]
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// 用户答案
    /// </summary>
    [Required]
    public string[] Answer { get; set; } = Array.Empty<string>();
}
