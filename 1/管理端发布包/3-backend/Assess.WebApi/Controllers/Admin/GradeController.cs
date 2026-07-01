using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Z.EntityFramework.Plus;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Repository;
using Mapster;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 年级管理
/// </summary>
public class GradeController : AdminApiController<GradeInfo, IGradeService>
{
    public GradeController(
        IGradeService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 分页获取年级列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<GradePageResult>> Page(DefaultPageParam param)
    {
        RequestPageResult<GradePageResult> result = new();
        EntityPageModel<GradeInfo> pageModel = new(param);
        param.OrderField = nameof(GradeInfo.SortLevel);
        param.OrderRule = OrderRule.Asc;

        var page = await entityService.GetPageAsync<GradePageResult>(pageModel).NotWaitBack();
        if (page != null) { result.LoadPage(page); }

        return result;
    }

    /// <summary>
    /// 新增年级信息
    /// </summary>
    /// <param name="param">新增的年级信息参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert(GradeInsertParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();
        int count = await db.GradeInfos.CountAsync(e => e.Name == param.Name).NotWaitBack();
        if (count > 0) { result.Fail("该名称重复"); }
        else
        {
            GradeInfo entity = param.Adapt<GradeInfo>();
            entity.IsShow = false;
            entity.IsUse = false;

            db.GradeInfos.Add(entity);

            count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("新增成功"); }
            else { result.Fail("新增失败"); }
        }

        return result;
    }

    /// <summary>
    /// 更改年级信息
    /// </summary>
    /// <param name="param">更改的年级信息参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update(GradeUpdateParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();
        int count = await db.GradeInfos.CountAsync(e => e.Id != param.Id && e.Name == param.Name).NotWaitBack();
        if (count > 0) { result.Fail("该名称重复"); }
        else
        {
            count = await db.GradeInfos.Where(e => e.Id == param.Id).UpdateAsync(e => new GradeInfo()
            {
                Name = param.Name,
                Remark = param.Remark,
                SortLevel = param.SortLevel,
            }).NotWaitBack();

            if (count > 0) { result.Success("更改成功"); }
            else { result.Fail("更改失败"); }
        }

        return result;
    }

    /// <summary>
    /// 更改年级信息单个字段值
    /// </summary>
    /// <param name="id">年级ID</param>
    /// <param name="type">更改目标字段，0: IsShow、1: IsUse</param>
    /// <param name="value">更改目标值</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult> UpdateField(int id, byte type, bool value)
    {
        int count = 0;
        RequestResult result = new();

        switch (type)
        {
            case 0:
                count = await entityService.UpdateAsync(
                    e => e.Id == id, e => new GradeInfo() { IsShow = value }).NotWaitBack();
                break;
            case 1:
                count = await entityService.UpdateAsync(
                    e => e.Id == id, e => new GradeInfo() { IsUse = value }).NotWaitBack();
                break;
            default: result.Fail("目标字段不合法"); return result;
        }

        if (count > 0) { result.Success("更改成功"); }
        else { result.Fail("更改失败"); }

        return result;
    }

    /// <summary>
    /// 删除年级信息
    /// </summary>
    /// <param name="id">年级ID</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete(int id)
    {
        RequestResult result = new();

        int count = await entityService.DeleteAsync(e => e.Id == id).NotWaitBack();
        if (count > 0) { result.Success("删除成功"); }
        else { result.Fail("删除失败"); }

        return result;
    }

    /// <summary>
    /// 获取 Select 下拉框数据源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<SelectResult> Select()
    {
        SelectResult result = new();

        var list = await entityService.GetListAsync(e => e.IsUse, e => new TopicPart() { Name = e.Name }).NotWaitBack();
        if (list?.Count > 0)
        {
            result.Data = list.Select(e => new SelectNode { Text = e.Name, Value = e.Name }).ToArray();
            result.Success();
        }

        return result;
    }
}
