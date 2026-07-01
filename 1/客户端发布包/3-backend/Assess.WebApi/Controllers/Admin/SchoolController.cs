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
/// 学校管理相关接口
/// </summary>
public class SchoolController : AdminApiController<SchoolInfo, ISchoolService>
{
    public SchoolController(ISchoolService entityService) : base(entityService)
    {
    }

    #region 分页

    /// <summary>
    /// 获取数据列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<SchoolPageItem>> Page(SchoolPageParam param)
    {
        RequestPageResult<SchoolPageItem> result = new();

        EntityPageModel<SchoolInfo> entityParam = new(param)
        {
            Where = e =>
                (param.SchoolType == null || e.SchoolType == param.SchoolType) &&
                (string.IsNullOrEmpty(param.Keyword) ||
                 e.FullName.Contains(param.Keyword) ||
                 e.Remark!.Contains(param.Keyword) ||
                 e.Intro!.Contains(param.Keyword) ||
                 e.Rector!.Contains(param.Keyword))
        };

        var page = await entityService.GetPageAsync(e => new SchoolPageItem
        {
            Id = e.Id,
            Name = e.Name,
            Logo = e.Logo,
            Intro = e.Intro,
            Email = e.Email,
            Rector = e.Rector,
            Remark = e.Remark,
            Address = e.Address,
            FullName = e.FullName,
            SchoolType = e.SchoolType,
            PhoneNumber = e.PhoneNumber,
            CreationTime = e.CreationTime,
            ModificationTime = e.ModificationTime,
        }, entityParam).NotWaitBack();

        if (page?.Total > 0)
        {
            page.Data.ForEach(x =>
            {
                x.Logo = WebFileTool.GetImageUrl(x.Logo, ImageType.SchoolLogo);
            });
            result.LoadPage(page);
        }
        else { result.Success(); }

        return result;
    }

    #endregion

    #region 新增

    /// <summary>
    /// 新增信息
    /// </summary>
    /// <param name="param">新增参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert([FromForm] SchoolInsertParam param)
    {
        RequestResult result = new();

        if (param.LogoFile != null && !param.LogoFile.ContentType.StartsWith("image"))
        { result.Fail("Logo 必须是图片"); }
        else
        {
            using AssessDBContext db = entityService.GetDbContext();

            int count = await db.SchoolInfos.CountAsync(e => e.FullName == param.FullName).NotWaitBack();
            if (count > 0) { result.Fail("学校全称重复"); }
            else
            {
                SchoolInfo entity = param.Adapt<SchoolInfo>();
                entity.CreationTime = DateTime.Now;

                if (param.LogoFile?.Length > 0)
                {
                    entity.Logo = WebFileTool.SaveImage(param.LogoFile, ImageType.SchoolLogo);
                }

                _ = await db.SchoolInfos.AddAsync(entity);

                count = await db.SaveChangesAsync().NotWaitBack();
                if (count > 0) { result.Success("添加成功"); }
                else { result.Fail("添加失败"); }
            }
        }

        return result;
    }

    #endregion

    #region 修改

    /// <summary>
    /// 修改信息
    /// </summary>
    /// <param name="param">修改参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update([FromForm] SchoolUpdateParam param)
    {
        RequestResult result = new();

        if (param.LogoFile != null && !param.LogoFile.ContentType.StartsWith("image"))
        { result.Fail("Logo 必须是图片"); }
        else
        {
            using AssessDBContext db = entityService.GetDbContext();

            List<SchoolInfo> entities = await db.SchoolInfos.Where(e => e.Id != param.Id ||
                e.FullName == param.FullName).ToListAsync();

            if (entities.Count > 1) { result.Fail("学校全称重复"); }
            else if (entities.Count <= 0) { result.Fail("您要修改的对象已删除"); }
            else
            {
                SchoolInfo entity = entities[0];

                bool hasChanged = entity.ChangeField(param, nameof(param.LogoFile));

                string? old_image = null;
                string? new_image = null;
                if (param.LogoFile?.Length > 0)
                {
                    hasChanged = true;
                    old_image = entity.Logo;
                    new_image = WebFileTool.SaveImage(param.LogoFile, ImageType.SchoolLogo);
                    entity.Logo = new_image;
                }

                if (hasChanged)
                {
                    entity.ModificationTime = DateTime.Now;
                    int count = await db.SaveChangesAsync().NotWaitBack();
                    if (count > 0)
                    {
                        result.Success("修改成功");
                        WebFileTool.DeleteImage(old_image, ImageType.SchoolLogo);
                    }
                    else
                    {
                        result.Fail("修改失败");
                        WebFileTool.DeleteImage(new_image, ImageType.SchoolLogo);
                    }
                }
                else { result.Success("修改成功"); }
            }
        }

        return result;
    }

    #endregion

    #region 删除

    /// <summary>
    /// 删除信息
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete([Required] int id)
    {
        RequestResult result = new();

        long count = await entityService.Adapter.GetCountAsync<StudentInfo>(
            e => e.SchoolId == id).NotWaitBack();

        if (count > 0)
        { result.Fail("该学校已绑定学生不予删除"); }
        else
        {
            count = await entityService.DeleteAsync(e => e.Id == id).NotWaitBack();
            if (count > 0) { result.Success("删除成功"); }
            else { result.Fail("删除失败"); }
        }

        return result;
    }

    #endregion

    #region 下拉框

    /// <summary>
    /// 获取 Select 数据源
    /// </summary>
    /// <param name="q"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<SelectResult> Select(string? q)
    {
        SelectResult result = new();

        var list = await entityService.GetTopListAsync(
            e => true, e => e.CreationTime, OrderRule.Desc, 15).NotWaitBack();

        if (list?.Count > 0)
        {
            result.Data = list.Select(e => new SelectNode
            {
                Value = e.Id,
                Text = e.FullName,
            }).ToArray();
        }
        result.Success();

        return result;
    }

    #endregion
}
