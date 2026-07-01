using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Repository;
using Mapster;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 思维之星后台管理接口相关
/// </summary>
public class ThinkStarController : AdminApiController<ThinkStar, IThinkStarService>
{
    public ThinkStarController(IThinkStarService entityService) : base(entityService)
    {
    }

    #region 分页

    /// <summary>
    /// 分页获取思维之星列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<ThinkStarPageItem>> Page(ThinkStarPageParam param)
    {
        RequestPageResult<ThinkStarPageItem> result = new();

        EntityPageModel<ThinkStar> entityParam = new(param)
        {
            Where = e =>
                (string.IsNullOrEmpty(param.Type) || e.Type == param.Type) &&
                (param.StartTime == null || e.CreationTime >= param.StartTime) &&
                (param.EndTime == null || e.CreationTime <= param.EndTime) &&
                (string.IsNullOrEmpty(param.Keyword) ||
                 e.Title.Contains(param.Keyword) ||
                 e.Remark!.Contains(param.Keyword))
        };

        var page = await entityService.GetPageAsync(e => new ThinkStarPageItem
        {
            Id = e.Id,
            Type = e.Type,
            Title = e.Title,
            Remark = e.Remark,
            CreationTime = e.CreationTime,
            Content = e.Content.Substring(0, 300),
            Name = e.Member!.RealName,
            Account = e.Member!.MemberAccount!.Account,
        }, entityParam).NotWaitBack();

        if (page?.Total > 0) { result.LoadPage(page); }
        else { result.Success(); }

        return result;
    }

    #endregion

    #region 新增

    /// <summary>
    /// 修改思维之星信息
    /// </summary>
    /// <param name="param">新增信息</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert([FromForm] ThinkStarInsertParam param)
    {
        RequestResult result = new();

        if (param.Image != null && !param.Image.ContentType.StartsWith("image"))
        { result.Fail("预览图必须是图片"); }
        else
        {
            using AssessDBContext db = entityService.GetDbContext();

            int count = await db.ThinkStars.CountAsync(e => e.Title == param.Title).NotWaitBack();
            if (count > 0) { result.Fail("标题重复"); }
            else
            {
                ThinkStar entity = param.Adapt<ThinkStar>();
                entity.CreationTime = DateTime.Now;

                if (param.Image?.Length > 0)
                {
                    entity.PreviewImg = WebFileTool.SaveImage(param.Image, ImageType.Preview);
                }
                _ = await db.ThinkStars.AddAsync(entity);

                count = await db.SaveChangesAsync().NotWaitBack();
                if (count > 0) { result.Success("添加成功"); }
                else
                {
                    result.Fail("添加失败");
                    WebFileTool.DeleteImage(entity.PreviewImg, ImageType.Preview);
                }
            }
        }

        return result;
    }

    #endregion

    #region 修改

    /// <summary>
    /// 修改思维之星信息
    /// </summary>
    /// <param name="param">修改参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update([FromForm] ThinkStarUpdateParam param)
    {
        RequestResult result = new();

        if (param.Image != null && !param.Image.ContentType.StartsWith("image"))
        { result.Fail("预览图必须是图片"); }
        else
        {
            using AssessDBContext db = entityService.GetDbContext();

            List<ThinkStar> entities = await db.ThinkStars.Where(e =>
                e.Id == param.Id || e.Title == param.Title).ToListAsync();

            if (entities.Count > 1) { result.Fail("标题重复"); }
            else if (entities.Count <= 0) { result.Fail("您要修改的对象已删除"); }
            else
            {
                ThinkStar entity = entities[0];

                bool hasChanged = entity.ChangeField(param, nameof(param.Image));

                string? old_image = null;
                if (param.Image?.Length > 0)
                {
                    hasChanged = true;
                    old_image = entity.PreviewImg;
                    entity.PreviewImg = WebFileTool.SaveImage(param.Image, ImageType.Preview);
                }

                if (hasChanged)
                {
                    int count = await db.SaveChangesAsync().NotWaitBack();
                    if (count > 0)
                    {
                        result.Success("更改成功");
                        WebFileTool.DeleteImage(old_image, ImageType.Preview);
                    }
                    else
                    {
                        result.Fail("更改失败");
                        WebFileTool.DeleteImage(entity.PreviewImg, ImageType.Preview);
                    }
                }
            }
        }

        return result;
    }

    #endregion

    #region 详情

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
            CreationTime = e.CreationTime,

            Name = e.Member!.RealName,
            Account = e.Member!.MemberAccount!.Account,
        }).NotWaitBack();

        if (entity == null) { result.Fail("您要请求的数据不存在"); }
        else
        {
            entity.PreviewImg = WebFileTool.GetImageUrl(entity.PreviewImg, ImageType.Preview);
            result.Data = entity;
            result.Success();
        }

        return result;
    }

    #endregion

    #region 删除

    /// <summary>
    /// 删除思维之星
    /// </summary>
    /// <param name="id">思维之星ID</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete([Required] uint id)
    {
        RequestResult result = new();

        var entity = await entityService.GetEntityAsync(id).NotWaitBack();

        if (entity == null) { result.Fail("不存在您要删除的项"); }
        else
        {
            bool isSuccess = await entityService.DeleteAsync(entity).NotWaitBack();
            if (isSuccess)
            {
                result.Success("删除成功");
                WebFileTool.DeleteImage(entity.PreviewImg, ImageType.Preview);
            }
            else { result.Fail("删除失败"); }
        }

        return result;
    }

    #endregion
}
