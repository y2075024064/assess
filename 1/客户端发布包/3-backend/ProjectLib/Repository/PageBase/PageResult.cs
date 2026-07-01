namespace Repository;

/// <summary>
/// 分页基本结果类型
/// </summary>
/// <typeparam name="TModel">结果基类型</typeparam>
public class PageResult<TModel>
{
    /// <summary>
    /// 数据总条数
    /// </summary>
    public long Total { get; set; }

    /// <summary>
    /// 数据列表
    /// </summary>
    public List<TModel> Data { get; set; } = new List<TModel>();
}