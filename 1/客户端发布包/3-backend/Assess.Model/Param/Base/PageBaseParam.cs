namespace Assess.Model.Param;

/// <summary>
/// 分页参数基础模型
/// </summary>
public class PageBaseParam
{
    /// <summary>
    /// 当前页数
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// 每页数据条数
    /// </summary>
    public int Take { get; set; }

    /// <summary>
    /// 跳过数据数
    /// </summary>
    public int Skip => (Page - 1) * Take;
}
