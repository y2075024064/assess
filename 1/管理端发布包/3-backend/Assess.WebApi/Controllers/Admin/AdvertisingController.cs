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
/// 广告管理接口
/// </summary>
public class AdvertisingController : AdminApiController<Advertising, IAdvertisingService>
{
    public AdvertisingController(IAdvertisingService entityService) : base(entityService)
    {
    }

    #region 分页

    /// <summary>
    /// 分页获取广告列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<AdvertisingPageItem>> Page(AdvertisingPageParam param)
    {
        RequestPageResult<AdvertisingPageItem> result = new();

        EntityPageModel<Advertising> entityParam = new(param)
        {
            Where = e =>
                (param.Type == null || e.Type == param.Type) &&
                (param.ModuleId == null || e.ModuleId == param.ModuleId) &&
                (string.IsNullOrEmpty(param.Keyword) ||
                 e.Title.Contains(param.Keyword) ||
                 e.Remark!.Contains(param.Keyword) ||
                 e.Company!.Contains(param.Keyword))
        };

        var page = await entityService.GetPageAsync(e => new AdvertisingPageItem
        {
            Id = e.Id,
            Type = e.Type,
            Title = e.Title,
            Remark = e.Remark,
            Company = e.Company,
            FolderUrl = e.FolderUrl,
            Hyperlink = e.Hyperlink,
            ModuleName = e.Module!.Name,
        }, entityParam).NotWaitBack();

        if (page?.Total > 0) { result.LoadPage(page); }
        else { result.Success(); }

        return result;
    }

    #endregion

    #region 新增

    /// <summary>
    /// 修改广告信息
    /// </summary>
    /// <param name="param">新增信息</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert([FromForm] AdvertisingInsertParam param)
    {
        RequestResult result = new();

        if (param.PreviewFile != null && !param.PreviewFile.ContentType.StartsWith("image"))
        { result.Fail("预览图必须是图片"); }
        else
        {
            using AssessDBContext db = entityService.GetDbContext();

            int count = await db.Advertisings.CountAsync(e => e.Title == param.Title).NotWaitBack();
            if (count > 0) { result.Fail("标题重复"); }
            else
            {
                Advertising entity = param.Adapt<Advertising>();

                if (param.PreviewFile?.Length > 0)
                {
                    entity.PreviewImg = WebFileTool.SaveImage(param.PreviewFile, ImageType.Preview);
                }

                bool isSuccess = false;
                switch (param.Type)
                {
                    case AdvertisingType.富文本:
                        if (string.IsNullOrWhiteSpace(param.Content))
                        { result.Fail("富文本内容不能为空"); }
                        else
                        {
                            entity.FolderUrl = null;
                            entity.Hyperlink = null;
                            isSuccess = true;
                        }
                        break;
                    case AdvertisingType.外链接:
                        if (string.IsNullOrWhiteSpace(param.Hyperlink))
                        { result.Fail("超链接不能为空"); }
                        else
                        {
                            entity.FolderUrl = null;
                            entity.Content = null;
                            isSuccess = true;
                        }
                        break;
                    case AdvertisingType.Html文件:
                        if (param.HtmlFiles?.Length > 0)
                        {
                            entity.Content = null;
                            entity.Hyperlink = null;

                            entity.FolderUrl = WebFileTool.SaveHtmlFolder(param.HtmlFiles, out string? msg);

                            if (msg != null) { result.Fail(msg); }
                            else { isSuccess = true; }
                        }
                        else if (string.IsNullOrEmpty(entity.FolderUrl))
                        { result.Fail("Html及资源文件必传"); }
                        else { isSuccess = true; }
                        break;
                }

                if (isSuccess)
                {
                    _ = db.Advertisings.Add(entity);
                    count = await db.SaveChangesAsync().NotWaitBack();
                    if (count > 0) { result.Success("添加成功"); }
                    else { result.Fail("添加失败"); }
                }

                if (!result.IsSucc)
                {
                    WebFileTool.DeleteHtmlFolder(entity.FolderUrl);
                    WebFileTool.DeleteImage(entity.PreviewImg, ImageType.Preview);
                }
            }
        }

        return result;
    }

    #endregion

    #region 修改

    /// <summary>
    /// 修改广告信息
    /// </summary>
    /// <param name="param">修改参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update([FromForm] AdvertisingUpdateParam param)
    {
        RequestResult result = new();

        if (param.PreviewFile != null && !param.PreviewFile.ContentType.StartsWith("image"))
        { result.Fail("预览图必须是图片"); }
        else
        {
            using AssessDBContext db = entityService.GetDbContext();

            List<Advertising> entities = await db.Advertisings.Where(e =>
                e.Id == param.Id || e.Title == param.Title).ToListAsync();

            if (entities.Count > 1) { result.Fail("标题重复"); }
            else if (entities.Count <= 0) { result.Fail("您要修改的对象已删除"); }
            else
            {
                Advertising entity = entities[0];

                bool hasChanged = entity.ChangeField(param,
                        nameof(param.HtmlFiles), nameof(param.PreviewFile));

                string? old_image = null;
                if (param.PreviewFile?.Length > 0)
                {
                    hasChanged = true;
                    old_image = entity.PreviewImg;
                    entity.PreviewImg = WebFileTool.SaveImage(param.PreviewFile, ImageType.Preview);
                }

                bool isSuccess = false;
                string? old_htmlPath = null;
                switch (param.Type)
                {
                    case AdvertisingType.富文本:
                        if (string.IsNullOrWhiteSpace(param.Content))
                        { result.Fail("富文本内容不能为空"); }
                        else
                        {
                            entity.FolderUrl = null;
                            entity.Hyperlink = null;
                            isSuccess = true;
                        }
                        break;
                    case AdvertisingType.外链接:
                        if (string.IsNullOrWhiteSpace(param.Hyperlink))
                        { result.Fail("超链接不能为空"); }
                        else
                        {
                            entity.FolderUrl = null;
                            entity.Content = null;
                            isSuccess = true;
                        }
                        break;
                    case AdvertisingType.Html文件:
                        if (param.HtmlFiles?.Length > 0)
                        {
                            entity.Content = null;
                            entity.Hyperlink = null;

                            old_htmlPath = entity.FolderUrl;
                            entity.FolderUrl = WebFileTool.SaveHtmlFolder(param.HtmlFiles, out string? msg);

                            if (msg != null) { result.Fail(msg); }
                            else { isSuccess = true; hasChanged = true; }
                        }
                        else if (string.IsNullOrEmpty(entity.FolderUrl))
                        { result.Fail("Html及资源文件必传"); }
                        else { isSuccess = true; }
                        break;
                }

                if (isSuccess && hasChanged)
                {
                    int count = await db.SaveChangesAsync().NotWaitBack();
                    if (count > 0)
                    {
                        result.Success("更新成功");
                        WebFileTool.DeleteHtmlFolder(old_htmlPath);
                        WebFileTool.DeleteImage(old_image, ImageType.Preview);
                    }
                    else { result.Fail("更新失败"); }
                }

                if (!result.IsSucc)
                {
                    WebFileTool.DeleteHtmlFolder(entity.FolderUrl);
                    WebFileTool.DeleteImage(entity.PreviewImg, ImageType.Preview);
                }
            }
        }

        return result;
    }

    #endregion

    #region 详情

    /// <summary>
    /// 获取广告详情
    /// </summary>
    /// <param name="id">广告ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<AdvertisingInfo>> Detail(int id)
    {
        RequestResult<AdvertisingInfo> result = new();

        var entity = await entityService.GetEntityAsync(e => e.Id == id, e => new AdvertisingInfo
        {
            Id = e.Id,
            Type = e.Type,
            Title = e.Title,
            Remark = e.Remark,
            Company = e.Company,
            Content = e.Content,
            ModuleId = e.ModuleId,
            FolderUrl = e.FolderUrl,
            Hyperlink = e.Hyperlink,
            PreviewImg = e.PreviewImg,
            ModuleName = e.Module!.Name,
        }).NotWaitBack();

        if (entity == null) { result.Fail("您要请求的数据不存在"); }
        else
        {
            entity.FolderUrl = WebFileTool.GetHtmlFolderUrl(entity.FolderUrl);
            entity.PreviewImg = WebFileTool.GetImageUrl(entity.PreviewImg, ImageType.Preview);
            result.Data = entity;
            result.Success();
        }

        return result;
    }

    #endregion

    #region 删除

    /// <summary>
    /// 删除广告
    /// </summary>
    /// <param name="id">广告ID</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete(
        [Required(ErrorMessage = "广告ID必传")] int id)
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
                WebFileTool.DeleteHtmlFolder(entity.FolderUrl);
            }
            else { result.Fail("删除失败"); }
        }

        return result;
    }

    #endregion
}
