namespace Assess.Model.Result;

/// <summary>
/// 搜索引擎结果模型
/// </summary>
public class TopicSearchItem
{
    public uint Id { get; set; }

    /// <summary>
    /// 题目编号
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// 题目标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 题目内容摘要
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 题目标签
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// 题目类型
    /// </summary>
    public TopicType Type { get; set; }

    /// <summary>
    /// 题目难度
    /// </summary>
    public decimal Level { get; set; }

    /// <summary>
    /// 分部ID
    /// </summary>
    public int? PartId { get; set; }
}



/// <summary>
/// 题目分页结果模型
/// </summary>
public class TopicPageResult
{
    public uint Id { get; set; }

    /// <summary>
    /// 题目编号
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// 题目标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 题目类型
    /// </summary>
    public TopicType Type { get; set; }

    /// <summary>
    /// 题目难度，0.1 - 1.0，越小越难
    /// </summary>
    public decimal Level { get; set; } = 1;

    /// <summary>
    /// 考察目的
    /// </summary>
    public string? Purpose { get; set; }

    /// <summary>
    /// 题目内容（Markdown）
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 用途
    /// </summary>
    public TopicUseType UseType { get; set; }

    /// <summary>
    /// 收藏数量
    /// </summary>
    public int CollectionNum { get; set; }

    /// <summary>
    /// 评论数量
    /// </summary>
    public int CommentNum { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 最近一次修改时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }

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
/// 题目详情结果模型
/// </summary>
public class TopicDetailResult : TopicPageResult
{
    public int? PartId { get; set; }

    /// <summary>
    /// 分部名称
    /// </summary>
    public string? PartName { get; set; }

    /// <summary>
    /// 题目标签数组（json）
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// 推荐年级数组（json）
    /// </summary>
    public string? RcmdGrade { get; set; }

    /// <summary>
    /// 出题用户
    /// </summary>
    public string? MemberName { get; set; }

    /// <summary>
    /// 正确答案（json）
    /// </summary>
    public string? RightAnswer { get; set; }

    /// <summary>
    /// 解析
    /// </summary>
    public string? Analysis { get; set; }

    /// <summary>
    /// 选项
    /// </summary>
    public string? Options { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    public string? Origin { get; set; }

    /// <summary>
    /// 原始编号
    /// </summary>
    public string? OriginNumber { get; set; }

    /// <summary>
    /// 原始类型
    /// </summary>
    public string? OriginType { get; set; }
}

/// <summary>
/// 题目练习记录模型
/// </summary>
public class TopicPracticeRecord
{
    /// <summary>
    /// 题目编号
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// 是否练习过
    /// </summary>
    public bool IsTested { get; set; }

    /// <summary>
    /// 是否正确作答
    /// </summary>
    public bool? IsTrue { get; set; }
}

/// <summary>
/// 答题结果模型
/// </summary>
public class AnswerResult
{
    /// <summary>
    /// 是否正确
    /// </summary>
    public bool IsTrue { get; set; }

    /// <summary>
    /// 正确答案
    /// </summary>
    public string? RightAnswer { get; set; }

    /// <summary>
    /// 解析
    /// </summary>
    public string? Analysis { get; set; }
}

/// <summary>
/// 题目详情模型
/// </summary>
public class TopicTestModel
{
    public uint Id { get; set; }

    /// <summary>
    /// 题目编号
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// 题目标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 题目类型
    /// </summary>
    public TopicType Type { get; set; }

    /// <summary>
    /// 题目难度，0.1 - 1.0，越小越难
    /// </summary>
    public decimal Level { get; set; } = 1;

    /// <summary>
    /// 题目标签
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// 推荐年级
    /// </summary>
    public string? RcmdGrade { get; set; }

    /// <summary>
    /// 题目内容（Markdown）
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 可选项
    /// </summary>
    public string? Options { get; set; }

    /// <summary>
    /// 收藏数量
    /// </summary>
    public int CollectionNum { get; set; }

    /// <summary>
    /// 评论数量
    /// </summary>
    public int CommentNum { get; set; }

    /// <summary>
    /// 用户答案
    /// </summary>
    public string? Answer { get; set; }

    /// <summary>
    /// 正确答案
    /// </summary>
    public string? RightAnswer { get; set; }

    /// <summary>
    /// 解析
    /// </summary>
    public string? Analysis { get; set; }

    /// <summary>
    /// 是否收藏
    /// </summary>
    public bool IsCollected { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    public string? Origin { get; set; }

    /// <summary>
    /// 是否原创
    /// </summary>
    public bool IsOriginal { get; set; }

    /// <summary>
    /// 原始编号
    /// </summary>
    public string? OriginNumber { get; set; }
}

/// <summary>
/// 题目模拟测评子记录模型
/// </summary>
public class TopicTestRecordItem
{
    /// <summary>
    /// 题目编号
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// 是否练习过
    /// </summary>
    public bool IsTested { get; set; }

    /// <summary>
    /// 用户答案
    /// </summary>
    public string Answer { get; set; } = string.Empty;
}

/// <summary>
/// 题目模拟测评记录模型
/// </summary>
public class TopicTestRecordModel
{
    /// <summary>
    /// 模拟测评记录Id
    /// </summary>
    public long TestId { get; set; }

    /// <summary>
    /// 剩余答题时间
    /// </summary>
    public int? RemainTime { get; set; }

    /// <summary>
    /// 当前答题位置
    /// </summary>
    public int CurrPostion { get; set; }

    /// <summary>
    /// 记录子项集合
    /// </summary>
    public TopicTestRecordItem[] Items { get; set; } = Array.Empty<TopicTestRecordItem>();
}

/// <summary>
/// 模拟测评结果
/// </summary>
public class TopicMockResult
{
    /// <summary>
    /// 正确数量
    /// </summary>
    public int RightCount { get; set; }

    /// <summary>
    /// 错误数量
    /// </summary>
    public int ErrorCount { get; set; }

    /// <summary>
    /// 分数
    /// </summary>
    public decimal Score { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; }
}
