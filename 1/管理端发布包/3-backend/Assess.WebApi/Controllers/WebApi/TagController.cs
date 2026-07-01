using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Entity;
using Repository;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 标签相关接口
/// </summary>
public class TagController : WebApiController<TagRecord, ITagRecordService>
{
    public TagController(ITagRecordService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 获取热门标签集合
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<string[]>> Hots()
    {
        RequestResult<string[]> result = new();

        var list = await entityService.GetTopListAsync(e => true,
            e => e.UseNum, OrderRule.Desc, 10).NotWaitBack();
        result.Data = list.Select(e => e.Name).ToArray();
        result.Success();

        return result;
    }
}
