using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Data.Common;
using System.Data;

namespace Repository;

/// <summary>
/// 仓储接口
/// 作者：官伟
/// </summary>
public interface IServiceBase<TDbContext, TEntity> where TDbContext : DbContext where TEntity : class, new()
{
    public DbContextAdapter<TDbContext> Adapter { get; }

    #region 增加

    /// <summary>
    /// 插入单条数据
    /// </summary>
    /// <param name="entity">数据实体</param>
    /// <returns></returns>
    bool Insert(TEntity entity);

    /// <summary>
    /// 异步插入单条数据
    /// </summary>
    /// <param name="entity">数据实体</param>
    /// <returns></returns>
    Task<bool> InsertAsync(TEntity entity);

    /// <summary>
    /// 插入多条数据
    /// </summary>
    /// <param name="entities">数据实体集合</param>
    /// <returns></returns>
    int Insert(ICollection<TEntity> entities);

    /// <summary>
    /// 异步插入多条数据
    /// </summary>
    /// <param name="entities">数据实体集合</param>
    /// <returns></returns>
    Task<int> InsertAsync(ICollection<TEntity> entities);

    #endregion

    #region 删除

    /// <summary>
    /// 删除单条数据
    /// </summary>
    /// <param name="entity">要删除的数据实体</param>
    /// <returns></returns>
    bool Delete(TEntity entity);

    /// <summary>
    /// 异步删除单条数据
    /// </summary>
    /// <param name="entity">要删除的数据实体</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(TEntity entity);

    /// <summary>
    /// 删除多条数据
    /// </summary>
    /// <param name="entities">要删除的多条数据实体</param>
    /// <returns></returns>
    int Delete(ICollection<TEntity> entities);

    /// <summary>
    /// 异步删除多条数据
    /// </summary>
    /// <param name="entities">要删除的多条数据实体</param>
    /// <returns></returns>
    Task<int> DeleteAsync(ICollection<TEntity> entities);

    /// <summary>
    /// 删除符合条件的数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <returns></returns>
    int Delete(Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// 异步删除符合条件的数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <returns></returns>
    Task<int> DeleteAsync(Expression<Func<TEntity, bool>> expression);

    #endregion

    #region 修改

    /// <summary>
    /// 修改单条数据
    /// </summary>
    /// <param name="entity">数据实体</param>
    /// <param name="fields">要修改的字段</param>
    /// <returns></returns>
    bool Update(TEntity entity, params string[]? fields);

    /// <summary>
    /// 异步修改单条数据
    /// </summary>
    /// <param name="entity">数据实体</param>
    /// <param name="fields">要修改的字段</param>
    /// <returns></returns>
    Task<bool> UpdateAsync(TEntity entity, params string[]? fields);

    /// <summary>
    /// 修改多条数据
    /// </summary>
    /// <param name="entities">数据实体集合</param>
    /// <param name="fields">要修改的字段</param>
    /// <returns></returns>
    int Update(ICollection<TEntity> entities, params string[]? fields);

    /// <summary>
    /// 异步修改多条数据
    /// </summary>
    /// <param name="entities">数据实体集合</param>
    /// <param name="fields">要修改的字段</param>
    /// <returns></returns>
    Task<int> UpdateAsync(ICollection<TEntity> entities, params string[]? fields);

    /// <summary>
    /// 修改符合条件的数据
    /// </summary>
    /// <param name="filterExpression">查询条件表达式</param>
    /// <param name="updateExpression">更新操作表达式</param>
    /// <returns></returns>
    int Update(Expression<Func<TEntity, bool>> filterExpression, Action<TEntity> updateExpression);

    /// <summary>
    /// 异步修改符合条件的数据
    /// </summary>
    /// <param name="filterExpression">查询条件表达式</param>
    /// <param name="updateExpression">更新操作表达式</param>
    /// <returns></returns>
    Task<int> UpdateAsync(Expression<Func<TEntity, bool>> filterExpression, Action<TEntity> updateExpression);

    /// <summary>
    /// 修改符合条件的数据
    /// </summary>
    /// <param name="filterExpression">查询条件表达式</param>
    /// <param name="updateExpression">更新操作表达式</param>
    /// <returns></returns>
    int Update(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression);

    /// <summary>
    /// 异步修改符合条件的数据
    /// </summary>
    /// <param name="filterExpression">查询条件表达式</param>
    /// <param name="updateExpression">更新操作表达式</param>
    /// <returns></returns>
    Task<int> UpdateAsync(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression);

    #endregion

    #region 查询

    /// <summary>
    /// 判断存在
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <returns></returns>
    bool Exists(Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// 异步判断存在
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <returns></returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// 根据主键查询单条数据
    /// </summary>
    /// <param name="primaryKeyValue">主键值</param>
    /// <returns></returns>
    TEntity? GetEntity(object primaryKeyValue);

    /// <summary>
    /// 根据主键异步查询单条数据
    /// </summary>
    /// <param name="primaryKeyValue">主键值</param>
    /// <returns></returns>
    Task<TEntity?> GetEntityAsync(object primaryKeyValue);

    /// <summary>
    /// 根据条件查询第一条数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    TEntity? GetEntity(Expression<Func<TEntity, bool>> expression, params string[]? includeTable);

    /// <summary>
    /// 根据条件异步查询第一条数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<TEntity?> GetEntityAsync(Expression<Func<TEntity, bool>> expression, params string[]? includeTable);

    /// <summary>
    /// 根据条件查询第一条数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    TResult? GetEntity<TResult>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression, params string[]? includeTable) where TResult : class, new();

    /// <summary>
    /// 异步根据条件查询第一条数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<TResult?> GetEntityAsync<TResult>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression, params string[]? includeTable) where TResult : class, new();

    /// <summary>
    /// 根据条件查询数据数量
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <returns></returns>
    long GetCount(Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// 根据条件异步查询数据数量
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <returns></returns>
    Task<long> GetCountAsync(Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// 查询符合条件的前几条数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderFieldExpression">排序字段表达式</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="topNumber">查询数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    List<TEntity> GetTopList<Tkey>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, Tkey>> orderFieldExpression,
        OrderRule orderRule, int topNumber, params string[]? includeTable);

    /// <summary>
    /// 异步查询符合条件的前几条数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderFieldExpression">排序字段表达式</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="topNumber">查询数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<List<TEntity>> GetTopListAsync<Tkey>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, Tkey>> orderFieldExpression,
        OrderRule orderRule, int topNumber, params string[]? includeTable);

    /// <summary>
    /// 查询符合条件的前几条数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="topNumber">查询数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    List<TEntity> GetTopList(
        Expression<Func<TEntity, bool>> expression,
        string orderField, OrderRule orderRule, int topNumber, params string[]? includeTable);

    /// <summary>
    /// 异步查询符合条件的前几条数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="topNumber">查询数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<List<TEntity>> GetTopListAsync(
        Expression<Func<TEntity, bool>> expression,
        string orderField, OrderRule orderRule, int topNumber, params string[]? includeTable);

    /// <summary>
    /// 获取符合条件的数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    List<TEntity> GetList(Expression<Func<TEntity, bool>> expression, params string[]? includeTable);

    /// <summary>
    /// 异步获取符合条件的数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression, params string[]? includeTable);

    /// <summary>
    /// 获取符合条件的数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    List<TResult> GetList<TResult>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression,
        params string[]? includeTable) where TResult : class, new();

    /// <summary>
    /// 异步获取符合条件的数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<List<TResult>> GetListAsync<TResult>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression,
        params string[]? includeTable) where TResult : class, new();

    /// <summary>
    /// 分页获取数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderFieldExpression">排序字段表达式</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="page">查询页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    PageResult<TEntity> GetPage<Tkey>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, Tkey>> orderFieldExpression,
        OrderRule orderRule, int page, int pageSize, params string[]? includeTable);

    /// <summary>
    /// 异步分页获取数据
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderFieldExpression">排序字段表达式</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="page">查询页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<PageResult<TEntity>> GetPageAsync<Tkey>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, Tkey>> orderFieldExpression,
        OrderRule orderRule, int page, int pageSize, params string[]? includeTable);

    /// <summary>
    /// 分页获取数据
    /// </summary>
    /// <typeparam name="TResult">数据类型</typeparam>
    /// <typeparam name="Tkey">排序字段</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="orderFieldExpression">排序字段表达式</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="page">查询页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    PageResult<TResult> GetPage<TResult, Tkey>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression,
        Expression<Func<TEntity, Tkey>> orderFieldExpression,
        OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TResult : class;

    /// <summary>
    /// 异步分页获取数据
    /// </summary>
    /// <typeparam name="TResult">数据类型</typeparam>
    /// <typeparam name="Tkey">排序字段</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="orderFieldExpression">排序字段表达式</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="page">查询页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<PageResult<TResult>> GetPageAsync<TResult, Tkey>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression,
        Expression<Func<TEntity, Tkey>> orderFieldExpression,
        OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TResult : class;

    /// <summary>
    /// 分页获取实体集合
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="page">条目开始位置</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    PageResult<TEntity> GetPage(
        Expression<Func<TEntity, bool>> expression,
        string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable);

    /// <summary>
    /// 异步分页获取实体集合
    /// </summary>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="page">条目开始位置</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<PageResult<TEntity>> GetPageAsync(
        Expression<Func<TEntity, bool>> expression,
        string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable);

    /// <summary>
    /// 分页获取实体集合
    /// </summary>
    /// <typeparam name="TResult">数据类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="page">条目开始位置</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    PageResult<TResult> GetPage<TResult>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression,
        string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TResult : class;

    /// <summary>
    /// 异步分页获取实体集合
    /// </summary>
    /// <typeparam name="TResult">数据类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="page">条目开始位置</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<PageResult<TResult>> GetPageAsync<TResult>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression,
        string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TResult : class;

    /// <summary>
    /// 获取分页
    /// </summary>
    /// <param name="model">分页模型</param>
    /// <returns></returns>
    PageResult<TEntity> GetPage(IEntityPageModel<TEntity> model);

    /// <summary>
    /// 异步获取分页
    /// </summary>
    /// <param name="model">分页模型</param>
    /// <returns></returns>
    Task<PageResult<TEntity>> GetPageAsync(IEntityPageModel<TEntity> model);

    /// <summary>
    /// 获取分页
    /// </summary>
    /// <typeparam name="TModel">数据类型</typeparam>
    /// <param name="model">分页模型</param>
    /// <returns></returns>
    PageResult<TModel> GetPage<TModel>(IEntityPageModel<TEntity> model) where TModel : class;

    /// <summary>
    /// 异步获取分页
    /// </summary>
    /// <typeparam name="TModel">数据类型</typeparam>
    /// <param name="model">分页模型</param>
    /// <returns></returns>
    Task<PageResult<TModel>> GetPageAsync<TModel>(IEntityPageModel<TEntity> model) where TModel : class;

    /// <summary>
    /// 获取分页
    /// </summary>
    /// <typeparam name="TResult">数据类型</typeparam>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="model">分页模型</param>
    /// <returns></returns>
    PageResult<TResult> GetPage<TResult>(Expression<Func<TEntity, TResult>> selectExpression, IEntityPageModel<TEntity> model) where TResult : class;

    /// <summary>
    /// 异步获取分页
    /// </summary>
    /// <typeparam name="TResult">数据类型</typeparam>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="model">分页模型</param>
    /// <returns></returns>
    Task<PageResult<TResult>> GetPageAsync<TResult>(Expression<Func<TEntity, TResult>> selectExpression, IEntityPageModel<TEntity> model) where TResult : class;

    #endregion

    #region 其他扩展操作

    /// <summary>
    /// 获取数据库上下文
    /// </summary>
    TDbContext GetDbContext();

    /// <summary>
    /// 执行单条sql操作(更新、删除、添加)
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns></returns>
    int ExecuteSqlCommand(string sql, params DbParameter[] parameters);

    /// <summary>
    /// 异步执行单条sql操作(更新、删除、添加)
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns></returns>
    Task<int> ExecuteSqlCommandAsync(string sql, params DbParameter[] parameters);

    /// <summary>
    /// 执行sql返回DataTable数据集
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns></returns>
    DataTable SqlQueryToDataTable(string sql, params DbParameter[] parameters);

    /// <summary>
    /// 异步执行sql返回DataTable数据集
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns></returns>
    Task<DataTable> SqlQueryToDataTableAsync(string sql, params DbParameter[] parameters);

    /// <summary>
    /// 执行sql返回DataSet数据集
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns></returns>
    DataSet SqlQueryToDataSet(string sql, params DbParameter[] parameters);

    /// <summary>
    /// 异步执行sql返回DataSet数据集
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns></returns>
    Task<DataSet> SqlQueryToDataSetAsync(string sql, params DbParameter[] parameters);

    /// <summary>
    /// 执行sql返回单个值
    /// </summary>
    /// <typeparam name="T">查询所返回对象的类型</typeparam>
    /// <param name="sql">sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns></returns>
    object? SqlQuerySingleValue(string sql, params DbParameter[] parameters);

    /// <summary>
    /// 异步执行sql返回单个值
    /// </summary>
    /// <typeparam name="T">查询所返回对象的类型</typeparam>
    /// <param name="sql">sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns></returns>
    Task<object?> SqlQuerySingleValueAsync(string sql, params DbParameter[] parameters);

    /// <summary>
    /// 执行sql返回单条数据
    /// </summary>
    /// <typeparam name="T">查询所返回对象的类型</typeparam>
    /// <param name="sql">sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns></returns>
    TModel? SqlQuerySingle<TModel>(string sql, params DbParameter[] parameters) where TModel : class, new();

    /// <summary>
    /// 异步执行sql返回单条数据
    /// </summary>
    /// <typeparam name="T">查询所返回对象的类型</typeparam>
    /// <param name="sql">sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns></returns>
    Task<TModel?> SqlQuerySingleAsync<TModel>(string sql, params DbParameter[] parameters) where TModel : class, new();

    /// <summary>
    /// 执行sql返回多条数据
    /// </summary>
    /// <typeparam name="T">查询所返回对象的类型</typeparam>
    /// <param name="sql">sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns></returns>
    List<TModel> SqlQueryList<TModel>(string sql, params DbParameter[] parameters) where TModel : class, new();

    /// <summary>
    /// 异步执行sql返回多条数据
    /// </summary>
    /// <typeparam name="T">查询所返回对象的类型</typeparam>
    /// <param name="sql">sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns></returns>
    Task<List<TModel>> SqlQueryListAsync<TModel>(string sql, params DbParameter[] parameters) where TModel : class, new();

    #endregion
}