using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Entity;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 用户错误题目相关
/// </summary>
public class TopicErrorController : WebApiController<MemberErrorRecord, IMemberErrorRecordService>
{
    public TopicErrorController(IMemberErrorRecordService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 分类获取我的错题列表
    /// </summary>
    /// <param name="partId">分类ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<SelectNode[]>> List(int? partId)
    {
        uint memberId = (uint)CurrentMemberId!;
        RequestResult<SelectNode[]> result = new();

        var list = await entityService.GetListAsync(
            e => e.MemberId == memberId && !e.IsPass && (partId == null || e.TopicInfo!.PartId == partId),
            e => new SelectNode { Text = e.TopicInfo!.Number, Value = e.LastErrorTime }).NotWaitBack();

        result.Data = list.ToArray();
        result.Success();

        return result;
    }

    /// <summary>
    /// 用户获取所有错误题目信息
    /// </summary>
    /// <param name="partId">分类ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<string?[]>> TotalInfo(int? partId)
    {
        uint memberId = (uint)CurrentMemberId!;
        RequestResult<string?[]> result = new();

        List<SelectNode> list = await entityService.GetListAsync(e =>
            e.MemberId == memberId && !e.IsPass && (partId == null || e.TopicInfo!.PartId == partId),
            e => new SelectNode { Text = e.TopicInfo!.Number }).NotWaitBack();

        result.Data = list.Select(e => e.Text).ToArray();
        result.Success();

        return result;
    }
}
