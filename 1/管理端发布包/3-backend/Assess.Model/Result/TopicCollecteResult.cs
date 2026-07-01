namespace Assess.Model.Result;

/// <summary>
/// 收藏题目结果模型
/// </summary>
public class TopicCollecteResult
{
    /// <summary>
    /// 收藏总数
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 收藏分组集合
    /// </summary>
    public TopicCollecteNode[]? Groups { get; set; }
}

/// <summary>
/// 收藏题目分组信息
/// </summary>
public class TopicCollecteNode
{
    /// <summary>
    /// 分组名称
    /// </summary>
    public string? GroupName { get; set; }

    /// <summary>
    /// 题目编号集合
    /// </summary>
    public string?[]? TopicNumbers { get; set; }
}
