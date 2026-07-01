using Microsoft.AspNetCore.Mvc;
using Assess.Service;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// Admin接口控制器 基控制器
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TService">实体服务</typeparam>
[Route("admin/[controller]/[action]")]
[ApiExplorerSettings(GroupName = "admin_v1")]
[AccreditController(MemberType.组织, MemberType.管理员)]
public class AdminApiController<TEntity, TService> : CheckAuthController<TEntity, TService> where TEntity : class, new() where TService : IBusinessServiceBase<TEntity>
{
    /// <summary>
    /// Admin接口基控制器
    /// </summary>
    /// <param name="entityService">实体服务</param>
    protected AdminApiController(TService entityService)
        : base(entityService)
    { }
}