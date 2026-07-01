using System.ComponentModel.DataAnnotations;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Repository;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 动态相关接口
/// </summary>
public class DynamicController : WebApiController<DynamicInfo, IDynamicInfoService>
{
    /// <summary>
    /// 动态接口控制器
    /// </summary>
    /// <param name="entityService"></param>
    public DynamicController(IDynamicInfoService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 分页获取动态列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <param name="type">动态类型</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestPageResult<DynamicListItem>> GetDynamicList([FromQuery] PageBaseParam param, DynamicType? type)
    {
        RequestPageResult<DynamicListItem> result = new();

        var page = await entityService.GetPageAsync(e =>
            e.IsDelete == false && (type == null || e.Type == type),
            e => new DynamicListItem
            {
                Id = e.Id,
                Title = e.Title,
                MemberId = e.MemberId,
                Avatar = e.Member!.Avatar,
                PreviewImg = e.PreviewImg,
                PublishTime = e.PublishTime,
                NickName = e.Member!.Nickname,
                Content = e.Content.Substring(0, 50),
            },
            e => e.PublishTime, OrderRule.Desc, param.Page, param.Take).NotWaitBack();

        if (page?.Total > 0)
        {
            page.Data.ForEach(item =>
            {
                item.PreviewImg = WebFileTool.GetImageUrl(item.PreviewImg, ImageType.Preview);
            });
            result.LoadPage(page);
        }
        else { result.Success(); }

        return result;
    }

    /// <summary>
    /// 获取动态详情
    /// </summary>
    /// <param name="id">动态ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<DynamicListDetail>> Detail(
        [Required(ErrorMessage = "动态ID必传")] uint id)
    {
        RequestResult<DynamicListDetail> result = new();

        var data = await entityService.GetEntityAsync(e => e.Id == id && e.IsDelete == false,
            e => new DynamicListDetail
            {
                Id = e.Id,
                Title = e.Title,
                Content = e.Content,
                MemberId = e.MemberId,
                PreviewImg = e.PreviewImg,
                Avatar = e.Member!.Avatar,
                PublishTime = e.PublishTime,
                NickName = e.Member!.Nickname,
                OriginalHref = e.OriginalHref,
                ModificationTime = e.ModificationTime,
            }).NotWaitBack();

        if (data == null) { result.Fail("您要获取的对象已删除"); }
        else
        {
            data.Avatar = WebFileTool.GetAvatarUrl(data.Avatar);
            result.Data = data;
            result.Success();
        }

        return result;
    }
}
