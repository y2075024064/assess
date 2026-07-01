using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Data.Common;
using System.Data;
using Mapster;

namespace Repository;

/// <summary>
/// 仓储实现
/// 作者：官伟
/// </summary>
public class ServiceBase<TDbContext, TEntity> : ServiceAdapter<TDbContext>, IServiceBase<TDbContext, TEntity> where TDbContext : DbContext where TEntity : class, new()
{
    public DbContextAdapter<TDbContext> Adapter => _Adapter;

    public ServiceBase(DbContextOptions<TDbContext> options) : base(options)
    {
    }

    #region 增加

    public bool Insert(TEntity entity)
    {
        return Adapter.Insert(entity);
    }

    public Task<bool> InsertAsync(TEntity entity)
    {
        return Adapter.InsertAsync(entity);
    }

    public int Insert(ICollection<TEntity> entities)
    {
        return Adapter.Insert(entities);
    }

    public Task<int> InsertAsync(ICollection<TEntity> entities)
    {
        return Adapter.InsertAsync(entities);
    }

    #endregion

    #region 删除

    public bool Delete(TEntity entity)
    {
        return Adapter.Delete(entity);
    }

    public Task<bool> DeleteAsync(TEntity entity)
    {
        return Adapter.DeleteAsync(entity);
    }

    public int Delete(ICollection<TEntity> entities)
    {
        return Adapter.Delete(entities);
    }

    public Task<int> DeleteAsync(ICollection<TEntity> entities)
    {
        return Adapter.DeleteAsync(entities);
    }

    public int Delete(Expression<Func<TEntity, bool>> expression)
    {
        return Adapter.Delete(expression);
    }

    public Task<int> DeleteAsync(Expression<Func<TEntity, bool>> expression)
    {
        return Adapter.DeleteAsync(expression);
    }

    #endregion

    #region 修改

    public bool Update(TEntity entity, params string[]? fields)
    {
        return Adapter.Update(entity, fields);
    }

    public Task<bool> UpdateAsync(TEntity entity, params string[]? fields)
    {
        return Adapter.UpdateAsync(entity, fields);
    }

    public int Update(ICollection<TEntity> entities, params string[]? fields)
    {
        return Adapter.Update(entities, fields);
    }

    public Task<int> UpdateAsync(ICollection<TEntity> entities, params string[]? fields)
    {
        return Adapter.UpdateAsync(entities, fields);
    }

    public int Update(Expression<Func<TEntity, bool>> filterExpression, Action<TEntity> updateExpression)
    {
        return Adapter.Update(filterExpression, updateExpression);
    }

    public Task<int> UpdateAsync(Expression<Func<TEntity, bool>> filterExpression, Action<TEntity> updateExpression)
    {
        return Adapter.UpdateAsync(filterExpression, updateExpression);
    }

    public int Update(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression)
    {
        return Adapter.Update(filterExpression, updateExpression);
    }

    public Task<int> UpdateAsync(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression)
    {
        return Adapter.UpdateAsync(filterExpression, updateExpression);
    }

    #endregion

    #region 查询

    public bool Exists(Expression<Func<TEntity, bool>> expression)
    {
        return Adapter.GetCount(expression) > 0;
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await Adapter.GetCountAsync(expression) > 0;
    }

    public TEntity? GetEntity(object primaryKeyValue)
    {
        return Adapter.GetEntity<TEntity>(primaryKeyValue);
    }

    public Task<TEntity?> GetEntityAsync(object primaryKeyValue)
    {
        return Adapter.GetEntityAsync<TEntity>(primaryKeyValue);
    }

    public TEntity? GetEntity(Expression<Func<TEntity, bool>> expression, params string[]? includeTable)
    {
        return Adapter.GetEntity(expression, includeTable);
    }

    public Task<TEntity?> GetEntityAsync(Expression<Func<TEntity, bool>> expression, params string[]? includeTable)
    {
        return Adapter.GetEntityAsync(expression, includeTable);
    }

    public TResult? GetEntity<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, params string[]? includeTable) where TResult : class, new()
    {
        return Adapter.GetEntity(expression, selectExpression, includeTable);
    }

    public Task<TResult?> GetEntityAsync<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, params string[]? includeTable) where TResult : class, new()
    {
        return Adapter.GetEntityAsync(expression, selectExpression, includeTable);
    }

    public long GetCount(Expression<Func<TEntity, bool>> expression)
    {
        return Adapter.GetCount(expression);
    }

    public Task<long> GetCountAsync(Expression<Func<TEntity, bool>> expression)
    {
        return Adapter.GetCountAsync(expression);
    }

    public List<TEntity> GetTopList<Tkey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, Tkey>> orderFieldExpression, OrderRule orderRule, int topNumber, params string[]? includeTable)
    {
        return Adapter.GetTopList(expression, orderFieldExpression, orderRule, topNumber, includeTable);
    }

    public Task<List<TEntity>> GetTopListAsync<Tkey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, Tkey>> orderFieldExpression, OrderRule orderRule, int topNumber, params string[]? includeTable)
    {
        return Adapter.GetTopListAsync(expression, orderFieldExpression, orderRule, topNumber, includeTable);
    }

    public List<TEntity> GetTopList(Expression<Func<TEntity, bool>> expression, string orderField, OrderRule orderRule, int topNumber, params string[]? includeTable)
    {
        return Adapter.GetTopList(expression, orderField, orderRule, topNumber, includeTable);
    }

    public Task<List<TEntity>> GetTopListAsync(Expression<Func<TEntity, bool>> expression, string orderField, OrderRule orderRule, int topNumber, params string[]? includeTable)
    {
        return Adapter.GetTopListAsync(expression, orderField, orderRule, topNumber, includeTable);
    }

    public List<TEntity> GetList(Expression<Func<TEntity, bool>> expression, params string[]? includeTable)
    {
        return Adapter.GetList(expression, includeTable);
    }

    public Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression, params string[]? includeTable)
    {
        return Adapter.GetListAsync(expression, includeTable);
    }

    public List<TResult> GetList<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, params string[]? includeTable) where TResult : class, new()
    {
        return Adapter.GetList(expression, selectExpression, includeTable);
    }

    public Task<List<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, params string[]? includeTable) where TResult : class, new()
    {
        return Adapter.GetListAsync(expression, selectExpression, includeTable);
    }

    public PageResult<TEntity> GetPage<Tkey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, Tkey>> orderFieldExpression, OrderRule orderRule, int page, int pageSize, params string[]? includeTable)
    {
        return Adapter.GetPage(expression, orderFieldExpression, orderRule, page, pageSize, includeTable);
    }

    public Task<PageResult<TEntity>> GetPageAsync<Tkey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, Tkey>> orderFieldExpression, OrderRule orderRule, int page, int pageSize, params string[]? includeTable)
    {
        return Adapter.GetPageAsync(expression, orderFieldExpression, orderRule, page, pageSize, includeTable);
    }

    public PageResult<TResult> GetPage<TResult, Tkey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, Expression<Func<TEntity, Tkey>> orderFieldExpression, OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TResult : class
    {
        return Adapter.GetPage(expression, selectExpression, orderFieldExpression, orderRule, page, pageSize, includeTable);
    }

    public Task<PageResult<TResult>> GetPageAsync<TResult, Tkey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, Expression<Func<TEntity, Tkey>> orderFieldExpression, OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TResult : class
    {
        return Adapter.GetPageAsync(expression, selectExpression, orderFieldExpression, orderRule, page, pageSize, includeTable);
    }

    public PageResult<TEntity> GetPage(Expression<Func<TEntity, bool>> expression, string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable)
    {
        return Adapter.GetPage(expression, orderField, orderRule, page, pageSize, includeTable);
    }

    public Task<PageResult<TEntity>> GetPageAsync(Expression<Func<TEntity, bool>> expression, string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable)
    {
        return Adapter.GetPageAsync(expression, orderField, orderRule, page, pageSize, includeTable);
    }

    public PageResult<TResult> GetPage<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TResult : class
    {
        return Adapter.GetPage(expression, selectExpression, orderField, orderRule, page, pageSize, includeTable);
    }

    public Task<PageResult<TResult>> GetPageAsync<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TResult : class
    {
        return Adapter.GetPageAsync(expression, selectExpression, orderField, orderRule, page, pageSize, includeTable);
    }

    public PageResult<TEntity> GetPage(IEntityPageModel<TEntity> model)
    {
        PageResult<TEntity> pagedResult = new();

        if (model != null)
        {
            var result = Adapter.GetPage(model.Where, model.Param.OrderField,
                model.Param.OrderRule, model.Param.Page, model.Param.Take, model.IncludeTable);
            pagedResult.Data = result.Data.ToList();
            pagedResult.Total = result.Total;
        }

        return pagedResult;
    }

    public async Task<PageResult<TEntity>> GetPageAsync(IEntityPageModel<TEntity> model)
    {
        PageResult<TEntity> pagedResult = new();

        if (model != null)
        {
            var result = await Adapter.GetPageAsync(model.Where, model.Param.OrderField,
                model.Param.OrderRule, model.Param.Page, model.Param.Take, model.IncludeTable).NotWaitBack();
            pagedResult.Data = result.Data.ToList();
            pagedResult.Total = result.Total;
        }

        return pagedResult;
    }

    public PageResult<TModel> GetPage<TModel>(IEntityPageModel<TEntity> model) where TModel : class
    {
        PageResult<TModel> pagedResult = new();

        if (model != null)
        {
            var result = Adapter.GetPage(model.Where, model.Param.OrderField,
                model.Param.OrderRule, model.Param.Page, model.Param.Take, model.IncludeTable);
            pagedResult.Data = result.Data.Select(e => e.Adapt<TEntity, TModel>()).ToList();
            pagedResult.Total = result.Total;
        }

        return pagedResult;
    }

    public async Task<PageResult<TModel>> GetPageAsync<TModel>(IEntityPageModel<TEntity> model) where TModel : class
    {
        PageResult<TModel> pagedResult = new();

        if (model != null)
        {
            var result = await Adapter.GetPageAsync(model.Where, model.Param.OrderField,
                model.Param.OrderRule, model.Param.Page, model.Param.Take, model.IncludeTable).NotWaitBack();
            pagedResult.Data = result.Data.Select(e => e.Adapt<TEntity, TModel>()).ToList();
            pagedResult.Total = result.Total;
        }

        return pagedResult;
    }

    public PageResult<TResult> GetPage<TResult>(Expression<Func<TEntity, TResult>> selectExpression, IEntityPageModel<TEntity> model) where TResult : class
    {
        return Adapter.GetPage(model.Where, selectExpression, model.Param.OrderField,
            model.Param.OrderRule, model.Param.Page, model.Param.Take, model.IncludeTable);
    }

    public Task<PageResult<TResult>> GetPageAsync<TResult>(Expression<Func<TEntity, TResult>> selectExpression, IEntityPageModel<TEntity> model) where TResult : class
    {
        return Adapter.GetPageAsync(model.Where, selectExpression, model.Param.OrderField,
            model.Param.OrderRule, model.Param.Page, model.Param.Take, model.IncludeTable);
    }

    #endregion

    #region 其他扩展操作

    public TDbContext GetDbContext()
    {
        return Adapter.GetDbContext();
    }

    public int ExecuteSqlCommand(string sql, params DbParameter[] parameters)
    {
        return Adapter.ExecuteSqlCommand(sql, parameters);
    }

    public Task<int> ExecuteSqlCommandAsync(string sql, params DbParameter[] parameters)
    {
        return Adapter.ExecuteSqlCommandAsync(sql, parameters);
    }

    public DataTable SqlQueryToDataTable(string sql, params DbParameter[] parameters)
    {
        return Adapter.SqlQueryToDataTable(sql, parameters);
    }

    public Task<DataTable> SqlQueryToDataTableAsync(string sql, params DbParameter[] parameters)
    {
        return Adapter.SqlQueryToDataTableAsync(sql, parameters);
    }

    public DataSet SqlQueryToDataSet(string sql, params DbParameter[] parameters)
    {
        return Adapter.SqlQueryToDataSet(sql, parameters);
    }

    public Task<DataSet> SqlQueryToDataSetAsync(string sql, params DbParameter[] parameters)
    {
        return Adapter.SqlQueryToDataSetAsync(sql, parameters);
    }

    public object? SqlQuerySingleValue(string sql, params DbParameter[] parameters)
    {
        return Adapter.SqlQuerySingleValue(sql, parameters);
    }

    public Task<object?> SqlQuerySingleValueAsync(string sql, params DbParameter[] parameters)
    {
        return Adapter.SqlQuerySingleValueAsync(sql, parameters);
    }

    public TModel? SqlQuerySingle<TModel>(string sql, params DbParameter[] parameters) where TModel : class, new()
    {
        return Adapter.SqlQuerySingle<TModel>(sql, parameters);
    }

    public Task<TModel?> SqlQuerySingleAsync<TModel>(string sql, params DbParameter[] parameters) where TModel : class, new()
    {
        return Adapter.SqlQuerySingleAsync<TModel>(sql, parameters);
    }

    public List<TModel> SqlQueryList<TModel>(string sql, params DbParameter[] parameters) where TModel : class, new()
    {
        return Adapter.SqlQueryList<TModel>(sql, parameters);
    }

    public Task<List<TModel>> SqlQueryListAsync<TModel>(string sql, params DbParameter[] parameters) where TModel : class, new()
    {
        return Adapter.SqlQueryListAsync<TModel>(sql, parameters);
    }

    #endregion
}