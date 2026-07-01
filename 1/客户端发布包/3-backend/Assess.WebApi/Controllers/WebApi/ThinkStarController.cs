using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Entity;
using Repository;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 思维之星相关接口
/// </summary>
public class ThinkStarController : WebApiController<ThinkStar, IThinkStarService>
{
    public ThinkStarController(IThinkStarService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 获取思维之星列表
    /// </summary>
    /// <param name="num">获取前多少个</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<ThinkStarBaseItem[]>> List(int num)
    {
        RequestResult<ThinkStarBaseItem[]> result = new();

        var list = await entityService.GetTopListAsync(
            e => true, e => e.CreationTime, OrderRule.Desc, num).NotWaitBack();

        result.Data = list.Select(e => new ThinkStarBaseItem
        {
            Id = e.Id,
            Type = e.Type,
            Title = e.Title,
            PreviewImg = WebFileTool.GetImageUrl(e.PreviewImg, ImageType.Preview)
        }).ToArray();
        result.Success();

        return result;
    }

    /// <summary>
    /// 获取思维之星详情
    /// </summary>
    /// <param name="id">思维之星ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<ThinkStarDetail>> Detail(int id)
    {
        RequestResult<ThinkStarDetail> result = new();

        var entity = await entityService.GetEntityAsync(e => e.Id == id, e => new ThinkStarDetail
        {
            Id = e.Id,
            Type = e.Type,
            Title = e.Title,
            Remark = e.Remark,
            Content = e.Content,
            MemberId = e.MemberId,
            PreviewImg = e.PreviewImg,
            CommentNum = e.CommentNum,
            CreationTime = e.CreationTime,
            Name = e.Member!.RealName,
            Avatar = e.Member!.Avatar,
            Account = e.Member!.MemberAccount!.Account,
        }).NotWaitBack();

        if (entity == null) { result.Fail("您要请求的数据不存在"); }
        else
        {
            entity.PreviewImg = WebFileTool.GetImageUrl(entity.PreviewImg, ImageType.Preview);
            entity.Avatar = WebFileTool.GetAvatarUrl(entity.Avatar);
            result.Data = entity;
            result.Success();
        }

        return result;
    }
}
