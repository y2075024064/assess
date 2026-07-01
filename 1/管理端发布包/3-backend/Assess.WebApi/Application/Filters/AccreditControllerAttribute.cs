using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Assess.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Assess.Model.Result;
using System.Reflection;

namespace Assess.WebApi;

/// <summary>
/// 控制器授权验证过滤器
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AccreditControllerAttribute : ActionFilterAttribute
{
    /// <summary>
    /// 授权账户类型集合
    /// </summary>
    public MemberType[]? Types { get; private set; }

    /// <summary>
    /// 控制器授权验证过滤（默认zhi）
    /// </summary>
    /// <param name="types">授权账户类型集合</param>
    public AccreditControllerAttribute(params MemberType[]? types)
    {
        Types = types;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.Controller is ICheckAuthController controller)
        {
            ControllerActionDescriptor actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            var accredit = actionDescriptor.MethodInfo.GetCustomAttribute<AccreditActionAttribute>();
            if (accredit == null || accredit.IgnoreAuth == false)
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
        }

        await base.OnActionExecutionAsync(context, next);
    }
}
