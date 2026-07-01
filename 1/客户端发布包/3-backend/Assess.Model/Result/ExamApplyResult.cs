namespace Assess.Model.Result;

/// <summary>
/// 我的考试信息模型
/// </summary>
public class MyExamInfo
{
    /// <summary>
    /// 考试名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 考试ID
    /// </summary>
    public int ExaminationId { get; set; }

    /// <summary>
    /// 考试记录ID
    /// </summary>
    public long? ExamineRecordId { get; set; }

    /// <summary>
    /// 考试开始日期
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// 考试开考时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 是否考试过
    /// </summary>
    public bool IsTested { get; set; }

    /// <summary>
    /// 距离考试还有多少天
    /// </summary>
    public int Days { get; set; }

    /// <summary>
    /// 考试成绩
    /// </summary>
    public decimal? Score { get; set; }

    /// <summary>
    /// 是否支付
    /// </summary>
    public bool IsPay { get; set; }

    /// <summary>
    /// 报名时间
    /// </summary>
    public DateTime ApplyTime { get; set; }
}
