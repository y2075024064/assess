using Microsoft.EntityFrameworkCore;
using Assess.Entity;
using Repository;

namespace Assess.Service;

/// <summary>
/// 基础业务服务接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IBusinessServiceBase<TEntity> : IServiceBase<AssessDBContext, TEntity>
    where TEntity : class, new()
{
}

/// <summary>
/// 基础业务服务类
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public class BusinessServiceBase<TEntity> : ServiceBase<AssessDBContext, TEntity>, IBusinessServiceBase<TEntity>
    where TEntity : class, new()
{
    public BusinessServiceBase(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}