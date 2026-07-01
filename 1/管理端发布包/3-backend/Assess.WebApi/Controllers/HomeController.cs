using Microsoft.AspNetCore.Mvc;
using Assess.Model.Result;

namespace Assess.WebApi.Controllers;

/// <summary>
/// 首选项控制器
/// </summary>
[Produces("application/json")]
public class HomeController : CommonBaseController
{
    /// <summary>
    /// 获取接口访问凭证，访问所有接口时都应在 Headers 中携带该凭证
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public RequestStrResult GetToken()
    {
        RequestStrResult result = new()
        {
            Code = ((uint)ResultCode.Ok),
            Data = Session.SessionId,
            Msg = "请求成功",
        };

        return result;
    }
}