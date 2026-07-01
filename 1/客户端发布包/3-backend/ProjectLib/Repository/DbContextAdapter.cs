using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using System.Linq.Expressions;
using Z.EntityFramework.Plus;
using System.Data.SqlClient;
using System.Data.Common;
using System.Dynamic;
using System.Data;
using Dapper;

namespace Repository;

public class DbContextAdapter<TDbContext> : IDbContextAdapter<TDbContext> where TDbContext : DbContext
{
    private readonly DbContextOptions<TDbContext> contextOptions;

    public DbContextAdapter(DbContextOptions<TDbContext> options)
    {
        contextOptions = options;
    }

    #region 操作

    /// <summary>
    /// 创建上下文
    /// </summary>
    public TDbContext CreateDbContext()
    {
        var dbContext = Activator.CreateInstance(typeof(TDbContext), contextOptions) as TDbContext;

        return dbContext!;
    }

    /// <summary>
    /// 获取实体的第一个属性名
    /// </summary>
    /// <param name="entityType">实体Type</param>
    /// <returns></returns>
    private static string GetFirstDefaultPropertyName(Type entityType)
    {
        var firstOrDefault = entityType.GetProperties().FirstOrDefault();
        if (firstOrDefault != null)
        {
            var propertyName = firstOrDefault.Name;
            return propertyName;
        }

        return "Id";
    }

    /// <summary>
    /// 创建连接查询
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="query">数据查询对象</param>
    /// <param name="includeTable">级联查询表名</param>
    /// <returns></returns>
    internal static IQueryable<TEntity> CreateJoinQuery<TEntity>(IQueryable<TEntity> query, params string[]? includeTable) where TEntity : class
    {
        if (includeTable?.Length > 0)
        {
            foreach (string item in includeTable)
            {
                query = query.Include(item);
            }
        }

        return query;
    }

    /// <summary>
    /// 填充数据库命令参数
    /// </summary>
    /// <param name="cmd">数据库命令</param>
    /// <param name="cmdParms">sql参数</param>
    /// <returns></returns>
    private static void FillCommandParam(DbCommand cmd, params DbParameter[]? cmdParms)
    {
        if (cmdParms?.Length > 0)
        {
            foreach (DbParameter parameter in cmdParms)
            {
                if ((parameter.Direction == ParameterDirection.InputOutput ||
                     parameter.Direction == ParameterDirection.Input) &&
                    (parameter.Value == null))
                {
                    parameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(parameter);
            }
        }
    }

    #endregion

    #region 增加

    public bool Insert<TEntity>(TEntity entity) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (entity == null) { return false; }
        _ = context.Add(entity);
        return context.SaveChanges() > 0;
    }

    public async Task<bool> InsertAsync<TEntity>(TEntity entity) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (entity == null) { return false; }
        _ = context.Add(entity);
        return (await context.SaveChangesAsync()) > 0;
    }

    public int Insert<TEntity>(ICollection<TEntity> entities) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (entities?.Count > 0)
        {
            context.AddRange(entities);
            return context.SaveChanges();
        }
        return 0;
    }

    public async Task<int> InsertAsync<TEntity>(ICollection<TEntity> entities) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (entities?.Count > 0)
        {
            context.AddRange(entities);
            return await context.SaveChangesAsync();
        }
        return 0;
    }

    #endregion

    #region 删除

    public bool Delete<TEntity>(TEntity entity) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (entity != null)
        {
            context.Remove(entity);
            return context.SaveChanges() > 0;
        }
        return false;
    }

    public async Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (entity != null)
        {
            context.Remove(entity);
            return (await context.SaveChangesAsync().NotWaitBack()) > 0;
        }
        return false;
    }

    public int Delete<TEntity>(ICollection<TEntity> entities) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (entities?.Count > 0)
        {
            context.RemoveRange(entities);
            return context.SaveChanges();
        }
        return 0;
    }

    public async Task<int> DeleteAsync<TEntity>(ICollection<TEntity> entities) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (entities?.Count > 0)
        {
            context.RemoveRange(entities);
            return await context.SaveChangesAsync().NotWaitBack();
        }
        return 0;
    }

    public int Delete<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (expression != null)
        {
            return context.Set<TEntity>().Where(expression).Delete();
        }
        return 0;
    }

    public async Task<int> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (expression != null)
        {
            return await context.Set<TEntity>().Where(expression).DeleteAsync().NotWaitBack();
        }
        return 0;
    }

    #endregion

    #region 修改

    public bool Update<TEntity>(TEntity entity, params string[]? fields) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (entity != null)
        {
            var entry = context.Update(entity);
            if (fields?.Length > 0)
            {
                foreach (string item in fields)
                {
                    entry.Property(item).IsModified = true;
                }
            }
            return context.SaveChanges() > 0;
        }
        return false;
    }

    public async Task<bool> UpdateAsync<TEntity>(TEntity entity, params string[]? fields) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (entity != null)
        {
            var entry = context.Update(entity);
            if (fields?.Length > 0)
            {
                foreach (string item in fields)
                {
                    entry.Property(item).IsModified = true;
                }
            }
            return (await context.SaveChangesAsync().NotWaitBack()) > 0;
        }
        return false;
    }

    public int Update<TEntity>(ICollection<TEntity> entities, params string[]? fields) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (entities?.Count > 0)
        {
            if (fields?.Length > 0)
            {
                foreach (var entity in entities)
                {
                    var entry = context.Attach(entity);
                    foreach (string item in fields)
                    {
                        entry.Property(item).IsModified = true;
                    }
                }
            }
            else
            {
                context.UpdateRange(entities);
            }
            return context.SaveChanges();
        }
        return 0;
    }

    public async Task<int> UpdateAsync<TEntity>(ICollection<TEntity> entities, params string[]? fields) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (entities?.Count > 0)
        {
            if (fields?.Length > 0)
            {
                foreach (var entity in entities)
                {
                    var entry = context.Attach(entity);
                    foreach (string item in fields)
                    {
                        entry.Property(item).IsModified = true;
                    }
                }
            }
            else
            {
                context.UpdateRange(entities);
            }
            return await context.SaveChangesAsync().NotWaitBack();
        }
        return 0;
    }

    public int Update<TEntity>(Expression<Func<TEntity, bool>> filterExpression, Action<TEntity> updateExpression) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (filterExpression != null && updateExpression != null)
        {
            List<TEntity> entities = context.Set<TEntity>().Where(filterExpression).ToList();
            entities.ForEach(updateExpression);

            return context.SaveChanges();
        }
        return 0;
    }

    public async Task<int> UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> filterExpression, Action<TEntity> updateExpression) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (filterExpression != null && updateExpression != null)
        {
            List<TEntity> entities = await context.Set<TEntity>().Where(filterExpression).ToListAsync();
            entities.ForEach(updateExpression);

            return await context.SaveChangesAsync();
        }
        return 0;
    }

    public int Update<TEntity>(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (filterExpression != null && updateExpression != null)
        {
            return context.Set<TEntity>().Where(filterExpression).UpdateFromQuery(updateExpression);
        }
        return 0;
    }

    public async Task<int> UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (filterExpression != null && updateExpression != null)
        {
            return await context.Set<TEntity>().Where(filterExpression).UpdateFromQueryAsync(updateExpression).NotWaitBack();
        }
        return 0;
    }

    #endregion

    #region 查询

    public TEntity? GetEntity<TEntity>(object primaryKeyValue) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (primaryKeyValue != null)
        {
            return context.Set<TEntity>().Find(primaryKeyValue);
        }
        return null;
    }

    public async Task<TEntity?> GetEntityAsync<TEntity>(object primaryKeyValue) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (primaryKeyValue != null)
        {
            return await context.Set<TEntity>().FindAsync(primaryKeyValue).ConfigureAwait(false);
        }
        return null;
    }

    public TEntity? GetEntity<TEntity>(Expression<Func<TEntity, bool>> expression, params string[]? includeTable) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (expression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);
            return query.FirstOrDefault(expression);
        }
        return null;
    }

    public async Task<TEntity?> GetEntityAsync<TEntity>(Expression<Func<TEntity, bool>> expression, params string[]? includeTable) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (expression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);
            return await query.FirstOrDefaultAsync(expression).NotWaitBack();
        }
        return null;
    }

    public TResult? GetEntity<TEntity, TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, params string[]? includeTable)
        where TEntity : class
        where TResult : class, new()
    {
        using TDbContext context = CreateDbContext();
        if (expression != null && selectExpression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);
            return query.Where(expression).Select(selectExpression).FirstOrDefault();
        }
        return null;
    }

    public async Task<TResult?> GetEntityAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, params string[]? includeTable)
        where TEntity : class
        where TResult : class, new()
    {
        using TDbContext context = CreateDbContext();
        if (expression != null && selectExpression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);
            return await query.Where(expression).Select(selectExpression).FirstOrDefaultAsync().NotWaitBack();
        }
        return null;
    }

    public long GetCount<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (expression != null)
        {
            return context.Set<TEntity>().LongCount(expression);
        }
        return 0;
    }

    public async Task<long> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (expression != null)
        {
            return await context.Set<TEntity>().LongCountAsync(expression).NotWaitBack();
        }
        return 0;
    }

    public List<TEntity> GetTopList<TEntity, Tkey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, Tkey>> orderFieldExpression, OrderRule orderRule, int topNumber, params string[]? includeTable) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (expression != null && orderFieldExpression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);
            if (orderRule == OrderRule.Desc)
            {
                query = query.OrderByDescending(orderFieldExpression);
            }
            else
            {
                query = query.OrderBy(orderFieldExpression);
            }
            return query.Take(topNumber).ToList();
        }
        return new List<TEntity>();
    }

    public async Task<List<TEntity>> GetTopListAsync<TEntity, Tkey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, Tkey>> orderFieldExpression, OrderRule orderRule, int topNumber, params string[]? includeTable) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (expression != null && orderFieldExpression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);
            if (orderRule == OrderRule.Desc)
            {
                query = query.OrderByDescending(orderFieldExpression);
            }
            else
            {
                query = query.OrderBy(orderFieldExpression);
            }
            return await query.Take(topNumber).ToListAsync().NotWaitBack();
        }
        return new List<TEntity>();
    }

    public List<TEntity> GetTopList<TEntity>(Expression<Func<TEntity, bool>> expression, string orderField, OrderRule orderRule, int topNumber, params string[]? includeTable) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (expression != null && !string.IsNullOrWhiteSpace(orderField))
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);

            if (orderRule == OrderRule.Asc)
            {
                query = query.OrderByDynamic(e => $"e.{orderField}");
            }
            else
            {
                query = query.OrderByDescendingDynamic(e => $"e.{orderField}");
            }

            return query.Take(topNumber).ToList();
        }
        return new List<TEntity>();
    }

    public async Task<List<TEntity>> GetTopListAsync<TEntity>(Expression<Func<TEntity, bool>> expression, string orderField, OrderRule orderRule, int topNumber, params string[]? includeTable) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (expression != null && !string.IsNullOrWhiteSpace(orderField))
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);

            if (orderRule == OrderRule.Asc)
            {
                query = query.OrderByDynamic(e => $"e.{orderField}");
            }
            else
            {
                query = query.OrderByDescendingDynamic(e => $"e.{orderField}");
            }

            return await query.Take(topNumber).ToListAsync().NotWaitBack();
        }
        return new List<TEntity>();
    }

    public List<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> expression, params string[]? includeTable) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (expression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);
            return query.ToList();
        }
        return new List<TEntity>();
    }

    public async Task<List<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> expression, params string[]? includeTable) where TEntity : class
    {
        using TDbContext context = CreateDbContext();
        if (expression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);
            return await query.ToListAsync().NotWaitBack();
        }
        return new List<TEntity>();
    }

    public List<TResult> GetList<TEntity, TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, params string[]? includeTable)
        where TEntity : class
        where TResult : class, new()
    {
        using TDbContext context = CreateDbContext();
        if (expression != null && selectExpression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);
            return query.Select(selectExpression).ToList();
        }
        return new List<TResult>();
    }

    public async Task<List<TResult>> GetListAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, params string[]? includeTable)
        where TEntity : class
        where TResult : class, new()
    {
        using TDbContext context = CreateDbContext();
        if (expression != null && selectExpression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);
            return await query.Select(selectExpression).ToListAsync().NotWaitBack();
        }
        return new List<TResult>();
    }

    public PageResult<TEntity> GetPage<TEntity, Tkey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, Tkey>> orderFieldExpression, OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TEntity : class
    {
        PageResult<TEntity> pagedResult = new();

        using TDbContext context = CreateDbContext();
        if (expression != null && orderFieldExpression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            var countFuture = context.Set<TEntity>().Where(expression).DeferredLongCount().FutureValue();
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);

            if (orderRule == OrderRule.Desc)
            {
                query = query.OrderByDescending(orderFieldExpression);
            }
            else
            {
                query = query.OrderBy(orderFieldExpression);
            }

            var listFuture = query.Skip(pageSize * (page - 1)).Take(pageSize).Future();

            pagedResult.Total = countFuture.Value;
            pagedResult.Data = listFuture.ToList();
        }

        return pagedResult;
    }

    public async Task<PageResult<TEntity>> GetPageAsync<TEntity, Tkey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, Tkey>> orderFieldExpression, OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TEntity : class
    {
        PageResult<TEntity> pagedResult = new();

        using TDbContext context = CreateDbContext();
        if (expression != null && orderFieldExpression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            var countFuture = context.Set<TEntity>().Where(expression).DeferredLongCount().FutureValue();
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);

            if (orderRule == OrderRule.Desc)
            {
                query = query.OrderByDescending(orderFieldExpression);
            }
            else
            {
                query = query.OrderBy(orderFieldExpression);
            }

            var listFuture = query.Skip(pageSize * (page - 1)).Take(pageSize).Future();

            pagedResult.Total = await countFuture.ValueAsync();
            pagedResult.Data = await listFuture.ToListAsync();
        }

        return pagedResult;
    }

    public PageResult<TResult> GetPage<TEntity, TResult, Tkey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, Expression<Func<TEntity, Tkey>> orderFieldExpression, OrderRule orderRule, int page, int pageSize, params string[]? includeTable)
        where TEntity : class
        where TResult : class
    {
        PageResult<TResult> pagedResult = new();

        using TDbContext context = CreateDbContext();
        if (expression != null && orderFieldExpression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            var countFuture = context.Set<TEntity>().Where(expression).DeferredLongCount().FutureValue();
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);

            if (orderRule == OrderRule.Desc)
            {
                query = query.OrderByDescending(orderFieldExpression);
            }
            else
            {
                query = query.OrderBy(orderFieldExpression);
            }

            var listFuture = query.Select(selectExpression).Skip(pageSize * (page - 1)).Take(pageSize).Future();

            pagedResult.Total = countFuture.Value;
            pagedResult.Data = listFuture.ToList();
        }

        return pagedResult;
    }

    public async Task<PageResult<TResult>> GetPageAsync<TEntity, TResult, Tkey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, Expression<Func<TEntity, Tkey>> orderFieldExpression, OrderRule orderRule, int page, int pageSize, params string[]? includeTable)
        where TEntity : class
        where TResult : class
    {
        PageResult<TResult> pagedResult = new();

        using TDbContext context = CreateDbContext();
        if (expression != null && orderFieldExpression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            var countFuture = context.Set<TEntity>().Where(expression).DeferredLongCount().FutureValue();
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);

            if (orderRule == OrderRule.Desc)
            {
                query = query.OrderByDescending(orderFieldExpression);
            }
            else
            {
                query = query.OrderBy(orderFieldExpression);
            }

            var listFuture = query.Select(selectExpression).Skip(pageSize * (page - 1)).Take(pageSize).Future();

            pagedResult.Total = await countFuture.ValueAsync();
            pagedResult.Data = await listFuture.ToListAsync();
        }

        return pagedResult;
    }

    public PageResult<TEntity> GetPage<TEntity>(Expression<Func<TEntity, bool>> expression, string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TEntity : class
    {
        PageResult<TEntity> pagedResult = new();

        using TDbContext context = CreateDbContext();
        if (expression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            var countFuture = context.Set<TEntity>().DeferredLongCount(expression).FutureValue();
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);

            if (!string.IsNullOrWhiteSpace(orderField))
            {
                if (orderRule == OrderRule.Asc)
                {
                    query = query.OrderByDynamic(e => $"e.{orderField}");
                }
                else
                {
                    query = query.OrderByDescendingDynamic(e => $"e.{orderField}");
                }
            }

            var listFuture = query.Skip(pageSize * (page - 1)).Take(pageSize).Future();

            pagedResult.Total = countFuture.Value;
            pagedResult.Data = listFuture.ToList();
        }

        return pagedResult;
    }

    public async Task<PageResult<TEntity>> GetPageAsync<TEntity>(Expression<Func<TEntity, bool>> expression, string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable) where TEntity : class
    {
        PageResult<TEntity> pagedResult = new();

        using TDbContext context = CreateDbContext();
        if (expression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            var countFuture = context.Set<TEntity>().Where(expression).DeferredLongCount().FutureValue();
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);

            if (!string.IsNullOrWhiteSpace(orderField))
            {
                if (orderRule == OrderRule.Asc)
                {
                    query = query.OrderByDynamic(e => $"e.{orderField}");
                }
                else
                {
                    query = query.OrderByDescendingDynamic(e => $"e.{orderField}");
                }
            }

            var listFuture = query.Skip(pageSize * (page - 1)).Take(pageSize).Future();

            pagedResult.Total = await countFuture.ValueAsync();
            pagedResult.Data = await listFuture.ToListAsync();
        }

        return pagedResult;
    }

    public PageResult<TResult> GetPage<TEntity, TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable)
        where TEntity : class
        where TResult : class
    {
        PageResult<TResult> pagedResult = new();

        using TDbContext context = CreateDbContext();
        if (expression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            var countFuture = context.Set<TEntity>().Where(expression).DeferredLongCount().FutureValue();
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);

            if (string.IsNullOrWhiteSpace(orderField)) { orderField = GetFirstDefaultPropertyName(typeof(TEntity)); }

            if (orderRule == OrderRule.Asc)
            {
                query = query.OrderByDynamic(e => $"e.{orderField}");
            }
            else
            {
                query = query.OrderByDescendingDynamic(e => $"e.{orderField}");
            }

            var listFuture = query.Select(selectExpression).Skip(pageSize * (page - 1)).Take(pageSize).Future();

            pagedResult.Total = countFuture.Value;
            pagedResult.Data = listFuture.ToList();
        }

        return pagedResult;
    }

    public async Task<PageResult<TResult>> GetPageAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selectExpression, string? orderField, OrderRule orderRule, int page, int pageSize, params string[]? includeTable)
        where TEntity : class
        where TResult : class
    {
        PageResult<TResult> pagedResult = new();

        using TDbContext context = CreateDbContext();
        if (expression != null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(expression);
            var countFuture = context.Set<TEntity>().Where(expression).DeferredLongCount().FutureValue();
            query = DbContextAdapter<TDbContext>.CreateJoinQuery(query, includeTable);

            if (string.IsNullOrWhiteSpace(orderField)) { orderField = GetFirstDefaultPropertyName(typeof(TEntity)); }

            if (orderRule == OrderRule.Asc)
            {
                query = query.OrderByDynamic(e => $"e.{orderField}");
            }
            else
            {
                query = query.OrderByDescendingDynamic(e => $"e.{orderField}");
            }

            var listFuture = query.Select(selectExpression).Skip(pageSize * (page - 1)).Take(pageSize).Future();

            pagedResult.Total = await countFuture.ValueAsync();
            pagedResult.Data = await listFuture.ToListAsync();
        }

        return pagedResult;
    }

    #endregion

    #region 其他扩展操作

    public TDbContext GetDbContext()
    {
        return CreateDbContext()!;
    }

    public int ExecuteSqlCommand(string sql, params DbParameter[] parameters)
    {
        if (!string.IsNullOrWhiteSpace(sql))
        {
            using TDbContext context = CreateDbContext();
            var tran = context.Database.BeginTransaction();
            try
            {
                int count = context.Database.ExecuteSqlRaw(sql, parameters);
                tran.Commit();
                return count;
            }
            catch { tran.Rollback(); throw; }
        }
        return 0;
    }

    public Task<int> ExecuteSqlCommandAsync(string sql, params DbParameter[] parameters)
    {
        return Task.Run(() => ExecuteSqlCommand(sql, parameters));
    }

    public DataTable SqlQueryToDataTable(string sql, params DbParameter[] parameters)
    {
        if (!string.IsNullOrWhiteSpace(sql))
        {
            using TDbContext context = CreateDbContext();
            DbConnection conn = context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) conn.Open();

            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            FillCommandParam(cmd, parameters);

            try
            {
                DbDataReader reader = cmd.ExecuteReader();
                DataTable dt = new();
                dt.Load(reader);
                reader.Close();

                return dt;
            }
            finally
            {
                conn.Close();
            }
        }
        return new DataTable();
    }

    public Task<DataTable> SqlQueryToDataTableAsync(string sql, params DbParameter[] parameters)
    {
        return Task.Run(() => SqlQueryToDataTable(sql, parameters));
    }

    public DataSet SqlQueryToDataSet(string sql, params DbParameter[] parameters)
    {
        if (!string.IsNullOrWhiteSpace(sql))
        {
            using TDbContext context = CreateDbContext();
            DbConnection conn = context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) conn.Open();

            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            FillCommandParam(cmd, parameters);

            try
            {
                DataSet ds = new();
                var sql_comm = cmd as SqlCommand;
                using (var adapter = new SqlDataAdapter(sql_comm))
                {
                    adapter.Fill(ds);
                }

                return ds;
            }
            finally
            {
                conn.Close();
            }
        }
        return new DataSet();
    }

    public Task<DataSet> SqlQueryToDataSetAsync(string sql, params DbParameter[] parameters)
    {
        return Task.Run(() => SqlQueryToDataSet(sql, parameters));
    }

    public object? SqlQuerySingleValue(string sql, params DbParameter[] parameters)
    {
        DataTable dt = SqlQueryToDataTable(sql, parameters);
        if (dt?.Rows.Count > 0)
        {
            return dt.Rows[0][0];
        }
        return null;
    }

    public Task<object?> SqlQuerySingleValueAsync(string sql, params DbParameter[] parameters)
    {
        return Task.Run(() => SqlQuerySingleValue(sql, parameters));
    }

    public TModel? SqlQuerySingle<TModel>(string sql, params DbParameter[] parameters) where TModel : class, new()
    {
        using var db = CreateDbContext();
        IDbConnection conn = db.Database.GetDbConnection();
        if (conn.State != ConnectionState.Open) conn.Open();

        if (parameters == null) return conn.QueryFirst<TModel>(sql);
        else return conn.QueryFirst<TModel>(sql, DbContextAdapter<TDbContext>.ToObject(parameters));
    }

    public async Task<TModel?> SqlQuerySingleAsync<TModel>(string sql, params DbParameter[] parameters) where TModel : class, new()
    {
        using var db = CreateDbContext();
        IDbConnection conn = db.Database.GetDbConnection();
        if (conn.State != ConnectionState.Open) conn.Open();

        if (parameters == null) return await conn.QueryFirstAsync<TModel>(sql).NotWaitBack();
        else return await conn.QueryFirstAsync<TModel>(sql, DbContextAdapter<TDbContext>.ToObject(parameters)).NotWaitBack();
    }

    public List<TModel> SqlQueryList<TModel>(string sql, params DbParameter[] parameters) where TModel : class, new()
    {
        using var db = CreateDbContext();

        IDbConnection conn = db.Database.GetDbConnection();
        if (conn.State != ConnectionState.Open) conn.Open();

        if (parameters == null) return conn.Query<TModel>(sql).ToList();
        else return conn.Query<TModel>(sql, DbContextAdapter<TDbContext>.ToObject(parameters)).ToList();
    }

    public async Task<List<TModel>> SqlQueryListAsync<TModel>(string sql, params DbParameter[] parameters) where TModel : class, new()
    {
        using var db = CreateDbContext();

        IDbConnection conn = db.Database.GetDbConnection();
        if (conn.State != ConnectionState.Open) conn.Open();

        if (parameters == null) return (await conn.QueryAsync<TModel>(sql).NotWaitBack()).ToList();
        else return (await conn.QueryAsync<TModel>(sql, DbContextAdapter<TDbContext>.ToObject(parameters)).NotWaitBack()).ToList();
    }

    private static object? ToObject(DbParameter[] param)
    {
        if (param == null || param.Length <= 0) return null;
        else
        {
            ExpandoObject result = new();
            IDictionary<string, object?> dictionary = result;

            foreach (var item in param)
            {
                dictionary[item.ParameterName] = item.Value;
            }

            return result;
        }
    }

    #endregion
}