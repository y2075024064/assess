using Repository;

namespace Assess.Model.Result;

/// <summary>
/// 返回基础模型
/// </summary>
public class RequestResult
{
    /// <summary>
    /// 业务结果代码
    /// </summary>
    /// <example>200</example>
    public uint Code { get; set; } = (uint)ResultCode.Failed;

    /// <summary>
    /// 请求是否成功
    /// </summary>
    public bool IsSucc => Code == (uint)ResultCode.Ok;

    /// <summary>
    /// 业务处理结果信息
    /// </summary>
    /// <example>处理成功</example>
    public string Msg { get; set; } = string.Empty;

    /// <summary>
    /// 处理成功结果
    /// </summary>
    /// <param name="msg">成功信息</param>
    public void Success(string msg = "请求成功")
    {
        Msg = msg;
        Code = (uint)ResultCode.Ok;
    }

    /// <summary>
    /// 处理失败结果
    /// </summary>
    /// <param name="msg"></param>
    public void Fail(string msg = "请求失败")
    {
        Msg = msg;
        Code = (uint)ResultCode.Failed;
    }

    /// <summary>
    /// 处理异常结果
    /// </summary>
    /// <param name="msg">异常信息</param>
    public void Exception(string msg = "程序异常")
    {
        Msg = msg;
        Code = (uint)ResultCode.Exception;
    }
}

/// <summary>
/// 返回基础模型
/// </summary>
/// <typeparam name="T">返回数据数据类型</typeparam>
public class RequestResult<T> : RequestResult
{
    /// <summary>
    /// 返回的目标数据
    /// </summary>
    public T? Data { get; set; }
}

/// <summary>
/// 返回基础模型
/// </summary>
public class RequestStrResult : RequestResult<string>
{
}

/// <summary>
/// 分页请求结果模型
/// </summary>
public class RequestPageResult<TPageModel> : RequestResult
{
    /// <summary>
    /// 数据总数
    /// </summary>
    /// <example>100</example>
    public long Total { get; set; }

    /// <summary>
    /// 数据列表
    /// </summary>
    public TPageModel[]? Data { get; set; }

    /// <summary>
    /// 载入分页结果数据信息并执行处理成功结果
    /// </summary>
    /// <param name="page">分页结果</param>
    /// <param name="msg">成功信息</param>
    public void LoadPage(PageResult<TPageModel> page, string msg = "获取成功")
    {
        Data = page.Data.ToArray();
        Total = page.Total;
        Success(msg);
    }
}

/// <summary>
/// 下拉框Node数据模型
/// </summary>
public class SelectNode
{
    /// <summary>
    /// 选项值
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// 选项显示文本
    /// </summary>
    public string? Text { get; set; }
}

/// <summary>
/// 下拉框结果模型
/// </summary>
public class SelectResult : RequestResult<SelectNode[]>
{
}