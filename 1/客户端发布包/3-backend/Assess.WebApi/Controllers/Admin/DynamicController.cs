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
/// 动态管理接口
/// </summary>
public class DynamicController : AdminApiController<DynamicInfo, IDynamicInfoService>
{
    public DynamicController(IDynamicInfoService entityService) : base(entityService)
    {
    }

    #region 分页

    /// <summary>
    /// 分页获取动态数据列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<DynamicPageItem>> Page(DynamicPageParam param)
    {
        RequestPageResult<DynamicPageItem> result = new();

        EntityPageModel<DynamicInfo> entityParam = new(param)
        {
            Where = e =>
                e.IsDelete == false &&
                (param.Type == null || param.Type == e.Type) &&
                (string.IsNullOrEmpty(param.Keyword) ||
                 e.Title.Contains(param.Keyword) ||
                 e.Remark!.Contains(param.Keyword) ||
                 e.Content.Contains(param.Keyword))
        };

        var page = await entityService.GetPageAsync(e => new DynamicPageItem
        {
            Id = e.Id,
            Type = e.Type,
            Title = e.Title,
            Remark = e.Remark,
            PublishTime = e.PublishTime,
            OriginalHref = e.OriginalHref,
            CreationTime = e.CreationTime,
            ModificationTime = e.ModificationTime,
        }, entityParam).NotWaitBack();

        if (page?.Total > 0) { result.LoadPage(page); }
        else { result.Success(); }

        return result;
    }

    #endregion

    #region 添加

    /// <summary>
    /// 添加动态信息
    /// </summary>
    /// <param name="param">添加参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert([FromForm] DynamicInsertParam param)
    {
        RequestResult result = new();

        if (param.PreviewFile != null && !param.PreviewFile.ContentType.StartsWith("image"))
        { result.Fail("预览图必须为图片"); }
        else
        {
            DynamicInfo entity = param.Adapt<DynamicInfo>();
            entity.MemberId = (uint)CurrentMemberId!;
            entity.CreationTime = DateTime.Now;

            if (param.PreviewFile?.Length > 0)
            {
                entity.PreviewImg = WebFileTool.SaveImage(param.PreviewFile, ImageType.Preview);
            }

            bool isSuccess = await entityService.InsertAsync(entity).NotWaitBack();
            if (isSuccess) { result.Success("添加成功"); }
            else
            {
                result.Fail("添加失败");
                WebFileTool.DeleteImage(entity.PreviewImg, ImageType.Preview);
            }
        }

        return result;
    }

    #endregion

    #region 编辑

    /// <summary>
    /// 编辑动态信息
    /// </summary>
    /// <param name="param">编辑参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update([FromForm] DynamicUpdateParam param)
    {
        RequestResult result = new();

        if (param.PreviewFile != null && !param.PreviewFile.ContentType.StartsWith("image"))
        { result.Fail("预览图必须为图片"); }
        else
        {
            using AssessDBContext db = entityService.GetDbContext();

            var entity = await db.DynamicInfos.FirstOrDefaultAsync(e =>
                e.Id == param.Id && e.IsDelete == false);

            if (entity == null) { result.Fail("您要编辑的对象不存在或已删除"); }
            else
            {
                bool hasChanged = entity.ChangeField(param);

                string? oldImg = null;
                if (param.PreviewFile?.Length > 0)
                {
                    hasChanged = true;
                    oldImg = entity.PreviewImg;
                    entity.PreviewImg = WebFileTool.SaveImage(param.PreviewFile, ImageType.Preview);
                }

                if (hasChanged)
                {
                    entity.ModificationTime = DateTime.Now;

                    int count = await db.SaveChangesAsync().NotWaitBack();
                    if (count > 0)
                    {
                        result.Success("更改成功");
                        WebFileTool.DeleteImage(oldImg, ImageType.Preview);
                    }
                    else
                    {
                        result.Fail("更改失败");
                        WebFileTool.DeleteImage(entity.PreviewImg, ImageType.Preview);
                    }

                }
                else { result.Success("更改成功"); }
            }
        }

        return result;
    }

    #endregion

    #region 详情

    /// <summary>
    /// 获取动态详情
    /// </summary>
    /// <param name="id">动态ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<DynamicDetail>> Detail(
        [Required(ErrorMessage = "动态ID必传")] uint id)
    {
        RequestResult<DynamicDetail> result = new();

        var entity = await entityService.GetEntityAsync(e =>
            e.Id == id && e.IsDelete == false).NotWaitBack();

        if (entity == null) { result.Fail("您要请求的对象不存在或已被删除"); }
        else
        {
            result.Success();
            result.Data = entity.Adapt<DynamicDetail>();
            result.Data.PreviewImg = WebFileTool.GetImageUrl(entity.PreviewImg, ImageType.Preview);
        }

        return result;
    }

    #endregion

    #region 删除

    /// <summary>
    /// 删除某条动态
    /// </summary>
    /// <param name="id">动态ID</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete(
        [Required(ErrorMessage = "动态ID必传")] uint id)
    {
        RequestResult result = new();

        int count = await entityService.UpdateAsync(e => e.Id == id,
            e => new DynamicInfo { IsDelete = true }).NotWaitBack();

        if (count > 0) { result.Success("删除成功"); }
        else { result.Fail("删除失败"); }

        return result;
    }

    #endregion
}
