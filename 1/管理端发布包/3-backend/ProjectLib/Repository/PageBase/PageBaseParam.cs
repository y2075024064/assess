using System.Linq.Expressions;

namespace Repository;

/// <summary>
/// 分页参数接口
/// </summary>
public interface IPageParam
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
    /// 排序字段
    /// </summary>
    public string? OrderField { get; set; }

    /// <summary>
    /// 排序规则（Asc、Desc）
    /// </summary>
    public OrderRule OrderRule { get; set; }
}

/// <summary>
/// 实体分页参数接口
/// </summary>
public interface IEntityPageParam : IPageParam
{
    /// <summary>
    /// 模糊查询关键字
    /// </summary>
    public string? Keyword { get; set; }
}

/// <summary>
/// 默认分页参数模型
/// </summary>
public class DefaultPageParam : IPageParam
{
    /// <summary>
    /// 当前页数（默认：1）
    /// </summary>
    /// <example>1</example>
    public int Page { get; set; } = 1;

    /// <summary>
    /// 每页数据条数（默认：30）
    /// </summary>
    /// <example>30</example>
    public int Take { get; set; } = 30;

    /// <summary>
    /// 排序字段
    /// </summary>
    /// <example></example>
    public string? OrderField { get; set; }

    /// <summary>
    /// 排序规则
    /// </summary>
    public OrderRule OrderRule { get; set; }
}

/// <summary>
/// 默认实体分页参数模型
/// </summary>
public class DefaultEntityPageParam : DefaultPageParam, IEntityPageParam
{
    public string? Keyword { get; set; }
}

/// <summary>
/// 实体分页模型接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IEntityPageModel<TEntity> where TEntity : class
{
    /// <summary>
    /// 分页参数对象
    /// </summary>
    public IPageParam Param { get; set; }

    /// <summary>
    /// 链接查询包含的表
    /// </summary>
    public string[]? IncludeTable { get; set; }

    /// <summary>
    /// 查询谓词表达式
    /// </summary>
    /// <returns></returns>
    public Expression<Func<TEntity, bool>> Where { get; set; }
}

/// <summary>
/// 实体分页模型
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public class EntityPageModel<TEntity> : IEntityPageModel<TEntity> where TEntity : class
{
    public IPageParam Param { get; set; }

    public string[]? IncludeTable { get; set; }

    public Expression<Func<TEntity, bool>> Where { get; set; } = e => true;

    public EntityPageModel(IPageParam param)
    {
        Param = param;
    }
}
