using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Assess.Model.Result;
using Serilog;

namespace Assess.WebApi;

/// <summary>
/// 自定义异常过滤器
/// </summary>
public class CustomErrorFilterAttribute : ExceptionFilterAttribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public override void OnException(ExceptionContext context)
    {
        RecordException(context);

        base.OnException(context);
    }

    private static void RecordException(ExceptionContext context)
    {
        Exception ex = context.Exception;

        try
        {
            if (ex.InnerException != null) ex = ex.InnerException;

            Log.Error(ex, "{Path} {Message}", context.HttpContext.Request.Path, ex.Message);
        }
        finally
        {
            RequestResult result = new();
#if RELEASE
            result.Exception();
#else
            result.Exception(ex.ToString());
#endif
            context.Result = new JsonResult(result);
        }
    }
}