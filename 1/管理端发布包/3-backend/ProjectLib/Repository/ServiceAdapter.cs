using Microsoft.EntityFrameworkCore;

namespace Repository;

/// <summary>
/// 服务适配器
/// </summary>
/// <typeparam name="TDbContext">数据库上下文类型</typeparam>
public class ServiceAdapter<TDbContext> where TDbContext : DbContext
{
    internal DbContextAdapter<TDbContext> _Adapter;

    /// <summary>
    /// 实例服务适配器
    /// </summary>
    public ServiceAdapter(DbContextOptions<TDbContext> options)
    {
        _Adapter = new DbContextAdapter<TDbContext>(options);
    }
}