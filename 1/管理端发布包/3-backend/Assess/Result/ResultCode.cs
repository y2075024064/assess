namespace Assess;

/// <summary>
/// 请求返回状态码
/// </summary>
public enum ResultCode : ushort
{
    /// <summary>
    /// 请求失败
    /// </summary>
    Failed = 0,

    /// <summary>
    /// 成功
    /// </summary>
    Ok = 200,

    /// <summary>
    /// 请求不合法
    /// </summary>
    BadRequest = 400,

    /// <summary>
    /// 未经授权
    /// </summary>
    Unauthorized = 401,

    /// <summary>
    /// 没有权限
    /// </summary>
    Noauthorized = 402,

    /// <summary>
    /// 前置条件失败
    /// </summary>
    PreconditionFailed = 412,

    /// <summary>
    /// 服务端不支持所需表示
    /// </summary>
    NotAcceptable = 406,

    /// <summary>
    /// 未注册
    /// </summary>
    NotRegist = 420,

    /// <summary>
    /// 程序异常
    /// </summary>
    Exception = 500,
}