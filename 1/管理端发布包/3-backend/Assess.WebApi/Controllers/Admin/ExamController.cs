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
/// 考试管理相关接口
/// </summary>
public class ExamController : AdminApiController<Examination, IExaminationService>
{
    public ExamController(IExaminationService entityService) : base(entityService)
    {
    }

    #region 分页

    /// <summary>
    /// 获取数据列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<ExaminationPageItem>> Page(ExaminationPageParam param)
    {
        RequestPageResult<ExaminationPageItem> result = new();

        EntityPageModel<Examination> entityParam = new(param)
        {
            Where = e =>
                (param.IsPublic == null || e.IsPublic == param.IsPublic) &&
                (string.IsNullOrEmpty(param.Number) || e.Number == param.Number) &&
                (param.EndTime == null || (e.EndDate <= param.EndTime || e.StartDate <= param.EndTime)) &&
                (param.StartTime == null || (e.StartDate >= param.StartTime || e.EndDate >= param.StartTime)) &&
                (string.IsNullOrEmpty(param.Keyword) ||
                 e.Name.Contains(param.Keyword) ||
                 e.Remark!.Contains(param.Keyword) ||
                 e.Purpose!.Contains(param.Keyword) ||
                 e.ReferenceObj!.Contains(param.Keyword))
        };

        var page = await entityService.GetPageAsync(e => new ExaminationPageItem
        {
            Id = e.Id,
            Name = e.Name,
            Remark = e.Remark,
            Number = e.Number,
            EndDate = e.EndDate,
            Purpose = e.Purpose,
            Duration = e.Duration,
            IsPublic = e.IsPublic,
            StartDate = e.StartDate,
            StartTime = e.StartTime,
            Organizer = e.Organizer,
            TotalScore = e.TotalScore,
            CoOrganizer = e.CoOrganizer,
            ReferenceObj = e.ReferenceObj,
            Count = e.ApplyInfos.Count(),
        }, entityParam).NotWaitBack();

        if (page?.Total > 0) { result.LoadPage(page); }
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
    public async Task<RequestResult> Insert(ExaminationInsertParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();

        Examination entity = param.Adapt<Examination>();
        entity.Number = CommonTool.CreateNumber("SE");
        _ = await db.AddAsync(entity);

        int count = await db.SaveChangesAsync().NotWaitBack();
        if (count > 0) { result.Success("添加成功"); }
        else { result.Fail("添加失败"); }

        return result;
    }

    #endregion

    #region 修改

    /// <summary>
    /// 更改信息
    /// </summary>
    /// <param name="param">更改参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update(ExaminationUpdateParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();

        var entity = await db.Examinations.FindAsync(param.Id);
        if (entity == null) { result.Fail("您要更改的对象已删除"); }
        else
        {
            if (entity.ChangeField(param, nameof(entity.Number)))
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
    /// 删除信息
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete([Required] int id)
    {
        RequestResult result = new();

        long count = await entityService.Adapter.GetCountAsync<ExamApplyInfo>(
            e => e.ExaminationId == id).NotWaitBack();

        if (count > 0)
        { result.Fail("该考试已有人报名，不予删除"); }
        else
        {
            using var db = entityService.GetDbContext();
            var entity = await db.Examinations.Include(e => e.Topics)
                .SingleOrDefaultAsync(e => e.Id == id);

            if (entity == null) { result.Fail("您要删除的对象不存在"); }
            else
            {
                db.RemoveRange(entity.Topics);
                _ = db.Remove(entity);

                count = await db.SaveChangesAsync().NotWaitBack();
                if (count > 0) { result.Success("删除成功"); }
                else { result.Fail("删除失败"); }
            }
        }

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
        [Required] int id, [Required] string field, string? value)
    {
        RequestResult result = new();

        var entity = await entityService.GetEntityAsync(id).NotWaitBack();

        if (entity == null)
        { result.Fail("不存在您要更改的项"); }
        else if (!entity.ChangeField(field, value))
        { result.Fail("不存在您要更改的字段"); }
        else
        {
            var isSuccess = await entityService.UpdateAsync(entity, field).NotWaitBack();
            if (isSuccess) { result.Success("更改成功"); }
            else { result.Fail("更改失败"); }
        }

        return result;
    }

    #endregion
}
