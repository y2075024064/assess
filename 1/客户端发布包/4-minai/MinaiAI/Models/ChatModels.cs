namespace MinaiAI.Models;

/// <summary>
/// 聊天消息
/// </summary>
public class ChatMessage
{
    public string Role { get; set; } = "user";
    public string Content { get; set; } = "";
}

/// <summary>
/// 聊天请求
/// </summary>
public class ChatRequest
{
    public List<ChatMessage> Messages { get; set; } = new();
}

/// <summary>
/// 统一响应（匹配前端 HttpResult 格式）
/// </summary>
public class ApiResult
{
    public bool IsSucc { get; set; } = true;
    public uint Code { get; set; } = 200;
    public string Msg { get; set; } = "";
    public object? Data { get; set; }

    public static ApiResult Success(object data) =>
        new() { IsSucc = true, Code = 200, Data = data };

    public static ApiResult Fail(string msg) =>
        new() { IsSucc = false, Code = 400, Msg = msg };
}

/// <summary>
/// 聊天回复数据
/// </summary>
public class ReplyData
{
    public string Reply { get; set; } = "";
}
