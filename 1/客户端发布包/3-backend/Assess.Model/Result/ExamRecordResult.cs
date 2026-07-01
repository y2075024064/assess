namespace Assess.Model.Result;

/// <summary>
/// 考试记录详情信息
/// </summary>
public class ExamRecordDetail
{
    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 答题记录
    /// </summary>
    public string? AnswerRecord { get; set; }

    /// <summary>
    /// 错误题目索引数组
    /// </summary>
    public string? ErrorIndexs { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 答题结束时间
    /// </summary>
    public DateTime? OverTime { get; set; }

    /// <summary>
    /// 题目编号集合
    /// </summary>
    public string[] Numbers { get; set; } = Array.Empty<string>();
}
