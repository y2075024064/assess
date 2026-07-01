using Microsoft.AspNetCore.Mvc;
using MinaiAI.Models;
using MinaiAI.Services;

namespace MinaiAI.Controllers;

/// <summary>
/// 米奈 AI 对话接口
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class ChatController : ControllerBase
{
    private readonly DeepSeekService _deepSeekService;

    public ChatController(DeepSeekService deepSeekService)
    {
        _deepSeekService = deepSeekService;
    }

    /// <summary>
    /// 发送消息，获取米奈的回复
    /// </summary>
    [HttpPost]
    public async Task<ApiResult> Send([FromBody] ChatRequest request)
    {
        if (request.Messages == null || request.Messages.Count == 0)
        {
            return ApiResult.Fail("消息不能为空");
        }

        var result = await _deepSeekService.SendMessageAsync(request.Messages);
        if (result.IsSucc)
        {
            return ApiResult.Success(new ReplyData { Reply = result.Reply });
        }
        return ApiResult.Fail(result.ErrorMsg ?? "米奈走神了~");
    }

    /// <summary>
    /// 获取米奈的自我介绍
    /// </summary>
    [HttpGet]
    public ApiResult Hello()
    {
        return ApiResult.Success(new ReplyData
        {
            Reply = "你好呀！我是「米奈」✨，你的专属AI学习伙伴！有什么想问我的吗？😊"
        });
    }
}
