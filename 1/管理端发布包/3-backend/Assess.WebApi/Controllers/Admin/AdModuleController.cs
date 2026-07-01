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
/// 广告模块管理接口
/// </summary>
public class AdModuleController : AdminApiController<AdModule, IAdModuleService>
{
    public AdModuleController(IAdModuleService entityService) : base(entityService)
    {
    }

    #region 分页

    /// <summary>
    /// 获取数据列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<AdModuleDetail>> Page(DefaultEntityPageParam param)
    {
        RequestPageResult<AdModuleDetail> result = new();

        EntityPageModel<AdModule> entityParam = new(param)
        {
            Where = e =>
                string.IsNullOrEmpty(param.Keyword) ||
                e.Name.Contains(param.Keyword) ||
                e.Remark!.Contains(param.Keyword) ||
                e.Location.Contains(param.Keyword) ||
                e.ModuleName.Contains(param.Keyword)
        };

        var page = await entityService.GetPageAsync(e => new AdModuleDetail
        {
            Id = e.Id,
            Name = e.Name,
            Remark = e.Remark,
            IsEnable = e.IsEnable,
            Location = e.Location,
            ModuleName = e.ModuleName,
            IsCarousel = e.IsCarousel,
        }, entityParam).NotWaitBack();

        if (page?.Total > 0) { result.LoadPage(page); }
        else { result.Success(); }

        return result;
    }

    #endregion

    #region 新增

    /// <summary>
    /// 新增广告模块
    /// </summary>
    /// <param name="param">新增信息</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert(AdModuleInsertParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();

        List<AdModule> entities = await db.AdModules.Where(e =>
            e.Name == param.Name || e.ModuleName == param.ModuleName)
            .ToListAsync().NotWaitBack();

        List<string> msgs = new();
        if (entities.Exists(e => e.Name == param.Name))
        { msgs.Add("模块名称重复"); }
        if (entities.Exists(e => e.ModuleName == param.ModuleName))
        { msgs.Add("标识名称重复"); }

        if (msgs.Count > 0) { result.Fail(string.Join('、', msgs)); }
        else
        {
            AdModule entity = param.Adapt<AdModule>();
            _ = db.AdModules.Add(entity);

            int count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("添加成功"); }
            else { result.Fail("添加失败"); }
        }

        return result;
    }

    #endregion

    #region 修改

    /// <summary>
    /// 修改广告模块信息
    /// </summary>
    /// <param name="param">修改信息</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update(AdModuleUpdateParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();

        List<AdModule> entities = await db.AdModules.Where(e => e.Id == param.Id ||
            e.Name == param.Name || e.ModuleName == param.ModuleName).ToListAsync();

        var entity = entities.Find(e => e.Id == param.Id);
        if (entity == null) { result.Fail("您要更改的对象已删除"); }
        else
        {
            _ = entities.Remove(entity);

            List<string> msgs = new();
            if (entities.Exists(e => e.Name == param.Name))
            { msgs.Add("模块名称重复"); }
            if (entities.Exists(e => e.ModuleName == param.ModuleName))
            { msgs.Add("标识名称重复"); }

            if (msgs.Count > 0) { result.Fail(string.Join('、', msgs)); }
            else if (entity.ChangeField(param))
            {
                int count = await db.SaveChangesAsync().NotWaitBack();
                if (count > 0) { result.Success("修改成功"); }
                else { result.Fail("修改失败"); }
            }
            else { result.Success("修改成功"); }
        }

        return result;
    }

    #endregion

    #region 删除

    /// <summary>
    /// 删除某个广告模块
    /// </summary>
    /// <param name="id">模块ID</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete(
        [Required(ErrorMessage = "模块ID必传")] int id)
    {
        RequestResult result = new();

        var entity = await entityService.GetEntityAsync(e => e.Id == id, e => new AdModule
        {
            Name = e.Name,
            IsCarousel = e.Advertisings.Any(),
        }).NotWaitBack();

        if (entity == null)
        { result.Fail("不存在您要删除的项"); }
        else if (entity.IsCarousel)
        { result.Fail("有广告绑定了该模块不予删除"); }
        else
        {
            int count = await entityService.DeleteAsync(e => e.Id == id).NotWaitBack();
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
    /// <returns></returns>
    [HttpGet]
    public async Task<SelectResult> Select()
    {
        SelectResult result = new();

        var list = await entityService.GetListAsync(e => e.IsEnable).NotWaitBack();

        if (list?.Count > 0)
        {
            result.Data = list.Select(e => new SelectNode
            {
                Value = e.Id,
                Text = e.Name + "_" + e.ModuleName,
            }).ToArray();
        }
        result.Success();

        return result;
    }

    #endregion

    #region 修改单个属性值

    /// <summary>
    /// 修改单个属性值
    /// </summary>
    /// <param name="id">模块ID</param>
    /// <param name="field">字段名称</param>
    /// <param name="value">修改值</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult> ModifySingleField(
        [Required(ErrorMessage = "模块ID必传")] int id,
        [Required(ErrorMessage = "字段名称必传")] string field,
        string? value)
    {
        RequestResult result = new();

        var entity = await entityService.GetEntityAsync(id).NotWaitBack();

        if (entity == null)
        { result.Fail("不存在您要修改的项"); }
        else if (!entity.ChangeField(field, value))
        { result.Fail("不存在您要修改的字段"); }
        else
        {
            var isSuccess = await entityService.UpdateAsync(entity, field).NotWaitBack();
            if (isSuccess) { result.Success("修改成功"); }
            else { result.Fail("修改失败"); }
        }

        return result;
    }

    #endregion
}
