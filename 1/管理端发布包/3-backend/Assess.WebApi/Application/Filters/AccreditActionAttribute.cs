using Microsoft.AspNetCore.Mvc.Filters;
using Assess.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Assess.Model.Result;

namespace Assess.WebApi;

/// <summary>
/// 接口授权验证过滤
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class AccreditActionAttribute : ActionFilterAttribute
{
    /// <summary>
    /// 是否忽略授权
    /// </summary>
    public bool IgnoreAuth { get; set; } = false;

    /// <summary>
    /// 授权账户类型集合
    /// </summary>
    public MemberType[]? Types { get; private set; }

    /// <summary>
    /// 接口授权验证过滤
    /// </summary>
    /// <param name="types">授权账户类型集合</param>
    public AccreditActionAttribute(params MemberType[]? types)
    {
        Types = types;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //如果不忽略授权 且 当前控制器必须是继承于权限验证控制器
        if (!IgnoreAuth && context.Controller is ICheckAuthController controller)
        {
            RequestResult<MemberType> result = new();
            MemberType? memberType = controller.GetMemberType();
            if (memberType == null)
            {
                result = await controller.CheckAuthAsync();
                memberType = result.Data;
            }
            else { result.Success(); }

            if (Types?.Length > 0 && result.IsSucc)
            {
                if (memberType == null || !Types.Contains(memberType.Value))
                {
                    result.Msg = "您没有访问权限";
                    result.Code = (uint)ResultCode.Noauthorized;
                }
            }

            if (!result.IsSucc) { context.Result = new JsonResult(result); }
        }

        await base.OnActionExecutionAsync(context, next);
    }
}
