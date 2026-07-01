using Assess.Model.Result;

namespace Assess.WebApi.Controllers;

/// <summary>
/// 权限验证控制器接口
/// </summary>
public interface ICheckAuthController
{
    /// <summary>
    /// 获取用户类型
    /// </summary>
    /// <returns></returns>
    MemberType? GetMemberType();

    /// <summary>
    /// 验证授权
    /// </summary>
    /// <returns></returns>
    Task<RequestResult<MemberType>> CheckAuthAsync();
}