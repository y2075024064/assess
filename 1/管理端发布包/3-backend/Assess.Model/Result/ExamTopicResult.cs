namespace Assess.Model.Result;

/// <summary>
/// 考试题目列表项信息模型
/// </summary>
public class ExamTopicListItem
{
    /// <summary>
    /// 考试题目Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 题目ID
    /// </summary>
    public uint TopicId { get; set; }

    /// <summary>
    /// 题目标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 题目难度，0.0 - 1.0，越小越难
    /// </summary>
    public decimal Level { get; set; }

    /// <summary>
    /// 题目内容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 题目标签数组
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// 推荐年级数组
    /// </summary>
    public string? RcmdGrade { get; set; }

    /// <summary>
    /// 分数
    /// </summary>
    public int Score { get; set; }
}
