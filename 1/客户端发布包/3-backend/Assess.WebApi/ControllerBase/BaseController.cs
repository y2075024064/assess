using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Assess.WebApi.Controllers;

/// <summary>
/// 抽象基类控制器（其它实体控制器都必须继承或间接继承与这个控制器）
/// </summary>
[ApiController]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// 当前会话
    /// </summary>
    protected MySession Session => session ??= new MySession(HttpContext);
    private MySession? session;

    /// <summary>
    /// 当前客户身份令牌Token
    /// </summary>
    protected string? CurrentToken => currentToken ??= Session.SessionId;
    private string? currentToken;

    /// <summary>
    /// 客户端的IP地址
    /// </summary>
    protected string? ClientIP => clientIP ??= GetNetworkIP(HttpContext);
    private string? clientIP = null;

    /// <summary>
    /// 客户端类型
    /// </summary>
    protected ClientType ClientType
    {
        get
        {
            if (clientType == ClientType.Unknown)
            {
                if (Request.Headers.TryGetValue("X-ClientType", out var value))
                {
                    string clientTypeStr = value.ToString();
                    if (Enum.TryParse(clientTypeStr, true, out ClientType _clientType))
                    {
                        clientType = _clientType;
                    }
                }
            }

            return clientType;
        }
    }
    private ClientType clientType;

    /// <summary>
    /// 获取访问者的IP地址
    /// </summary>
    /// <param name="httpContext">Http请求上下文</param>
    /// <returns></returns>
    private static string GetNetworkIP(HttpContext httpContext)
    {
        try
        {
            string? ip;
            IPAddress? address;
            IHeaderDictionary headers = httpContext.Request.Headers;
            if (headers.TryGetValue("X-Forwarded-For", out var value))
            {
                ip = value.ToString();
                if (IPAddress.TryParse(ip, out address)) { goto Result; }
            }

            if (headers.TryGetValue("X-Real-IP", out value))
            {
                ip = value.ToString();
                if (IPAddress.TryParse(ip, out address)) { goto Result; }
            }

            ip = httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            _ = IPAddress.TryParse(ip, out address);

        Result:
            if (address != null)
            {
                return address.MapToIPv4().ToString();
            }
        }
        catch { }

        return "0.0.0.0";
    }
}
