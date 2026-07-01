using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Z.EntityFramework.Plus;
using Assess.Model.Result;
using Assess.Service;

namespace Assess.WebApi.Controllers;

/// <summary>
/// 抽象权限验证控制器
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TService">实体服务</typeparam>
public abstract class CheckAuthController<TEntity, TService> : BaseController, ICheckAuthController
    where TEntity : class, new()
    where TService : IBusinessServiceBase<TEntity>
{
    protected static readonly string memberIdKey = "MemberId";
    protected static readonly string memberTypeKey = "MemberType";

    /// <summary>
    /// 实体服务
    /// </summary>
    protected readonly TService entityService;

    /// <summary>
    /// 实体类型
    /// </summary>
    protected readonly Type entityType = typeof(TEntity);

    /// <summary>
    /// 当前用户Id
    /// </summary>
    protected uint? CurrentMemberId => Session.Get<uint>(memberIdKey);

    /// <summary>
    /// 当前用户类型
    /// </summary>
    protected MemberType? CurrentMemberType => (MemberType?)Session.Get<byte>(memberTypeKey);

    /// <summary>
    /// 权限验证控制器
    /// </summary>
    /// <param name="entityService">实体服务</param>
    protected CheckAuthController(TService entityService)
    {
        this.entityService = entityService;
    }

    /// <summary>
    /// 获取用户类型
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public MemberType? GetMemberType()
    {
        return CurrentMemberType;
    }

    /// <summary>
    /// 验证授权
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public async Task<RequestResult<MemberType>> CheckAuthAsync()
    {
        string? ip = ClientIP;
        string? token = CurrentToken;
        ClientType type = ClientType;
        uint tokenDuration = AppSettings.Settings.TokenDuration;
        RequestResult<MemberType> result = new() { Code = (uint)ResultCode.Unauthorized };

        if (token.IsEmpty()) { result.Msg = "请先完成登录！"; }
        else
        {
            DateTime nowTime = DateTime.Now;
            using var db = entityService.GetDbContext();
            var tokenInfo = await db.MemberTokens.Where(e => e.Token == token && e.ClientType == type)
                .Select(e => new { e.MemberId, e.Member!.Type, e.IsRemember, e.ExpirationTime })
                .AsNoTracking().FirstOrDefaultAsync().NotWaitBack();

            if (tokenInfo == null) { result.Msg = "登录已失效，请重新登录！"; }
            else if (tokenInfo.IsRemember || tokenInfo.ExpirationTime >= nowTime)
            {
                result.Success();
                result.Data = tokenInfo.Type;

                Session.Set(new Dictionary<string, object?> {
                    { memberIdKey, tokenInfo.MemberId },
                    { memberTypeKey, tokenInfo.Type }
                });
            }
            else { result.Msg = "登录已过期，请重新登录！"; }
        }

        return result;
    }
}