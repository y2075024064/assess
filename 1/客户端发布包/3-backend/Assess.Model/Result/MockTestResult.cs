namespace Assess.Model.Result;

/// <summary>
/// 模拟测评排行信息模型
/// </summary>
public class MockTestRankInfo
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 总答题数
    /// </summary>
    public int AnswerCount { get; set; }

    /// <summary>
    /// 正确率
    /// </summary>
    public decimal Accuracy { get; set; }

    /// <summary>
    /// 综合排名分（答题数×0.6 + 正确率×0.4）
    /// </summary>
    public decimal RankScore { get; set; }

    /// <summary>
    /// 排名
    /// </summary>
    public int Rank { get; set; }

    /// <summary>
    /// 是否为当前登录用户
    /// </summary>
    public bool IsCurrentUser { get; set; }

    /// <summary>
    /// 用户头像地址
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string? Nickname { get; set; }
}

/// <summary>
/// 模拟测评历史记录模型
/// </summary>
public class MockTestRecordModel
{
    /// <summary>
    /// 记录ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 类别名称
    /// </summary>
    public string? PartName { get; set; }

    /// <summary>
    /// 年级
    /// </summary>
    public string? Grade { get; set; }

    /// <summary>
    /// 开始答题时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 答题结束时间
    /// </summary>
    public DateTime? OverTime { get; set; }

    /// <summary>
    /// 平均难度
    /// </summary>
    public decimal AvgDifficulty { get; set; }

    /// <summary>
    /// 当前成绩
    /// </summary>
    public decimal CurrScore { get; set; }
}
