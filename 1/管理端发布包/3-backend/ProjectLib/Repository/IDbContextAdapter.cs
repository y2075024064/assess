using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Data.Common;
using System.Data;

namespace Repository;

/// <summary>
/// 名    称：IDbContextAdapter
/// 描    述：数据库操作装饰器接口
/// </summary>
public interface IDbContextAdapter<out TDbContext> where TDbContext : DbContext
{
    #region 增加

    /// <summary>
    /// 添加单条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="entity">数据实体</param>
    /// <returns></returns>
    bool Insert<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// 异步添加单条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="entity">数据实体</param>
    /// <returns></returns>
    Task<bool> InsertAsync<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// 添加多条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="entities">数据实体集合</param>
    /// <returns></returns>
    int Insert<TEntity>(ICollection<TEntity> entities) where TEntity : class;

    /// <summary>
    /// 异步添加多条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="entities">数据实体集合</param>
    /// <returns></returns>
    Task<int> InsertAsync<TEntity>(ICollection<TEntity> entities) where TEntity : class;

    #endregion

    #region 删除

    /// <summary>
    /// 删除单条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="entity">数据实体</param>
    /// <returns></returns>
    bool Delete<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// 异步删除单条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="entity">数据实体</param>
    /// <returns></returns>
    Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// 删除多条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="entities">数据实体集合</param>
    /// <returns></returns>
    int Delete<TEntity>(ICollection<TEntity> entities) where TEntity : class;

    /// <summary>
    /// 异步删除多条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="entities">数据实体集合</param>
    /// <returns></returns>
    Task<int> DeleteAsync<TEntity>(ICollection<TEntity> entities) where TEntity : class;

    /// <summary>
    /// 删除符合条件的数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <returns></returns>
    int Delete<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

    /// <summary>
    /// 异步删除符合条件的数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <returns></returns>
    Task<int> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

    #endregion

    #region 修改

    /// <summary>
    /// 修改单条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="entity">数据实体</param>
    /// <param name="fields">要修改的字段</param>
    /// <returns></returns>
    bool Update<TEntity>(TEntity entity, params string[]? fields) where TEntity : class;

    /// <summary>
    /// 异步修改单条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="entity">数据实体</param>
    /// <param name="fields">要修改的字段</param>
    /// <returns></returns>
    Task<bool> UpdateAsync<TEntity>(TEntity entity, params string[]? fields) where TEntity : class;

    /// <summary>
    /// 修改多条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="entities">数据实体集合</param>
    /// <param name="fields">要修改的字段</param>
    /// <returns></returns>
    int Update<TEntity>(ICollection<TEntity> entities, params string[]? fields) where TEntity : class;

    /// <summary>
    /// 异步修改多条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="entities">数据实体集合</param>
    /// <param name="fields">要修改的字段</param>
    /// <returns></returns>
    Task<int> UpdateAsync<TEntity>(ICollection<TEntity> entities, params string[]? fields) where TEntity : class;

    /// <summary>
    /// 修改符合条件的数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="filterExpression">查询条件表达式</param>
    /// <param name="updateExpression">更新操作表达式</param>
    /// <returns></returns>
    int Update<TEntity>(
        Expression<Func<TEntity, bool>> filterExpression,
        Action<TEntity> updateExpression) where TEntity : class;

    /// <summary>
    /// 异步修改符合条件的数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="filterExpression">查询条件表达式</param>
    /// <param name="updateExpression">更新操作表达式</param>
    /// <returns></returns>
    Task<int> UpdateAsync<TEntity>(
        Expression<Func<TEntity, bool>> filterExpression,
        Action<TEntity> updateExpression) where TEntity : class;

    /// <summary>
    /// 修改符合条件的数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="filterExpression">查询条件表达式</param>
    /// <param name="updateExpression">更新操作表达式</param>
    /// <returns></returns>
    int Update<TEntity>(
        Expression<Func<TEntity, bool>> filterExpression,
        Expression<Func<TEntity, TEntity>> updateExpression) where TEntity : class;

    /// <summary>
    /// 异步修改符合条件的数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="filterExpression">查询条件表达式</param>
    /// <param name="updateExpression">更新操作表达式</param>
    /// <returns></returns>
    Task<int> UpdateAsync<TEntity>(
        Expression<Func<TEntity, bool>> filterExpression,
        Expression<Func<TEntity, TEntity>> updateExpression) where TEntity : class;

    #endregion

    #region 查询

    /// <summary>
    /// 根据主键查询单条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="primaryKeyValue">主键值</param>
    /// <returns></returns>
    TEntity? GetEntity<TEntity>(object primaryKeyValue) where TEntity : class;

    /// <summary>
    /// 根据主键异步查询单条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="primaryKeyValue">主键值</param>
    /// <returns></returns>
    Task<TEntity?> GetEntityAsync<TEntity>(object primaryKeyValue) where TEntity : class;

    /// <summary>
    /// 根据条件查询第一条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    TEntity? GetEntity<TEntity>(Expression<Func<TEntity, bool>> expression, params string[]? includeTable) where TEntity : class;

    /// <summary>
    /// 根据条件异步查询第一条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<TEntity?> GetEntityAsync<TEntity>(Expression<Func<TEntity, bool>> expression, params string[]? includeTable) where TEntity : class;

    /// <summary>
    /// 根据条件查询第一条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TResult">数据类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    TResult? GetEntity<TEntity, TResult>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression, params string[]? includeTable) where TEntity : class where TResult : class, new();

    /// <summary>
    /// 异步根据条件查询第一条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TResult">数据类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<TResult?> GetEntityAsync<TEntity, TResult>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression, params string[]? includeTable) where TEntity : class where TResult : class, new();

    /// <summary>
    /// 根据条件查询数据数量
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <returns></returns>
    long GetCount<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

    /// <summary>
    /// 根据条件异步查询数据数量
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <returns></returns>
    Task<long> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

    /// <summary>
    /// 查询符合条件的前几条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="Tkey">排序字段</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderFieldExpression">排序字段表达式</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="topNumber">查询数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    List<TEntity> GetTopList<TEntity, Tkey>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, Tkey>> orderFieldExpression,
        OrderRule orderRule, int topNumber, params string[]? includeTable) where TEntity : class;

    /// <summary>
    /// 异步查询符合条件的前几条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="Tkey">排序字段</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderFieldExpression">排序字段表达式</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="topNumber">查询数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<List<TEntity>> GetTopListAsync<TEntity, Tkey>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, Tkey>> orderFieldExpression,
        OrderRule orderRule, int topNumber, params string[]? includeTable) where TEntity : class;

    /// <summary>
    /// 查询符合条件的前几条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="topNumber">查询数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    List<TEntity> GetTopList<TEntity>(
        Expression<Func<TEntity, bool>> expression,
        string orderField, OrderRule orderRule, int topNumber, params string[]? includeTable) where TEntity : class;

    /// <summary>
    /// 异步查询符合条件的前几条数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="topNumber">查询数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<List<TEntity>> GetTopListAsync<TEntity>(
        Expression<Func<TEntity, bool>> expression,
        string orderField, OrderRule orderRule, int topNumber, params string[]? includeTable) where TEntity : class;

    /// <summary>
    /// 获取符合条件的数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    List<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> expression, params string[]? includeTable) where TEntity : class;

    /// <summary>
    /// 异步获取符合条件的数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<List<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> expression, params string[]? includeTable) where TEntity : class;

    /// <summary>
    /// 获取符合条件的数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TResult">数据类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    List<TResult> GetList<TEntity, TResult>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression,
        params string[]? includeTable) where TEntity : class where TResult : class, new();

    /// <summary>
    /// 异步获取符合条件的数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TResult">数据类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<List<TResult>> GetListAsync<TEntity, TResult>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression,
        params string[]? includeTable) where TEntity : class where TResult : class, new();

    /// <summary>
    /// 分页获取数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="Tkey">排序字段</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderFieldExpression">排序字段表达式</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="page">查询页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    PageResult<TEntity> GetPage<TEntity, Tkey>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, Tkey>> orderFieldExpression,
        OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TEntity : class;

    /// <summary>
    /// 异步分页获取数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="Tkey">排序字段</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderFieldExpression">排序字段表达式</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="page">查询页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<PageResult<TEntity>> GetPageAsync<TEntity, Tkey>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, Tkey>> orderFieldExpression,
        OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TEntity : class;

    /// <summary>
    /// 分页获取数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
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
    PageResult<TResult> GetPage<TEntity, TResult, Tkey>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression,
        Expression<Func<TEntity, Tkey>> orderFieldExpression,
        OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TEntity : class where TResult : class;

    /// <summary>
    /// 异步分页获取数据
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
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
    Task<PageResult<TResult>> GetPageAsync<TEntity, TResult, Tkey>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression,
        Expression<Func<TEntity, Tkey>> orderFieldExpression,
        OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TEntity : class where TResult : class;

    /// <summary>
    /// 分页获取实体集合
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="page">条目开始位置</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    PageResult<TEntity> GetPage<TEntity>(
        Expression<Func<TEntity, bool>> expression,
        string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TEntity : class;

    /// <summary>
    /// 异步分页获取实体集合
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="page">条目开始位置</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<PageResult<TEntity>> GetPageAsync<TEntity>(
        Expression<Func<TEntity, bool>> expression,
        string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TEntity : class;

    /// <summary>
    /// 分页获取实体集合
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TResult">数据类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="page">条目开始位置</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    PageResult<TResult> GetPage<TEntity, TResult>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression,
        string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TEntity : class where TResult : class;

    /// <summary>
    /// 异步分页获取实体集合
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TResult">数据类型</typeparam>
    /// <param name="expression">查询条件表达式</param>
    /// <param name="selectExpression">选择器表达式</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderRule">排序规则</param>
    /// <param name="page">条目开始位置</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    Task<PageResult<TResult>> GetPageAsync<TEntity, TResult>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selectExpression,
        string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TEntity : class where TResult : class;

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