using Microsoft.AspNetCore.Mvc;
using Assess.Service;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 业务接口控制器 基控制器
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TService">实体服务</typeparam>
[AccreditController]
[Route("api/[controller]/[action]")]
[ApiExplorerSettings(GroupName = "api_v1")]
public class WebApiController<TEntity, TService> : CheckAuthController<TEntity, TService> where TEntity : class, new() where TService : IBusinessServiceBase<TEntity>
{
    /// <summary>
    /// 业务接口基控制器
    /// </summary>
    /// <param name="entityService">实体服务</param>
    protected WebApiController(TService entityService) : base(entityService)
    { }
}