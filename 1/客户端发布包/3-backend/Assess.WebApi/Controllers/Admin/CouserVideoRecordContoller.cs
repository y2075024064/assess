using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Repository;
using Assess.Service.Services;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 视频观看记录管理相关接口
/// </summary>
public class CourseVideoRecordController : AdminApiController<CouserVideoRecord, ICouserVideoRecordService>
{
    public CourseVideoRecordController(ICouserVideoRecordService entityService) : base(entityService)
    {
    }

    #region 分页

    /// <summary>
    /// 获取视频观看记录分页列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<CouserVideoRecordItem>> Page(VideoRecordPageParam param)
    {
        RequestPageResult<CouserVideoRecordItem> result = new();

        EntityPageModel<CouserVideoRecord> entityParam = new(param)
        {
            // 构造查询条件  
            Where = e =>
                (param.MemberId == null || e.MemberId == param.MemberId) &&
                (param.CourseId == null || e.CourseId == param.CourseId) &&
                (param.VideoId == null || e.VideoId == param.VideoId) &&
                (param.IsCompleted == null || e.IsCompleted == param.IsCompleted)
        };

        // 查询数据库 第一个参数是查询结果并且做了规则映射   第二个传查询条件
        var page = await entityService.GetPageAsync(e => new CouserVideoRecordItem
        {
            Id = e.Id,
            MemberId = e.MemberId,
            CourseId = e.CourseId,
            VideoId = e.VideoId,
            LastPlayPosition = e.LastPlayPosition,
            IsCompleted = e.IsCompleted,
            UpdateTime = e.UpdateTime,
            CourseTitle = e.Course!.Title,
            VideoTitle = e.Video!.Title   
        }, entityParam).NotWaitBack();

        if (page?.Total > 0) { result.LoadPage(page); }
        else { result.Success(); }

        return result;
    }

    #endregion

    #region 新增

    /// <summary>
    /// 新增视频观看记录
    /// </summary>
    /// <param name="param">新增参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert(VideoRecordInsertParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();

        CouserVideoRecord record = new()
        {
            MemberId = param.MemberId,
            CourseId = param.CourseId,
            VideoId = param.VideoId,
            LastPlayPosition = param.LastPlayPosition,
            IsCompleted = param.IsCompleted,
            UpdateTime = DateTime.Now
        };

        _ = db.CouserVideoRecords.Add(record); // 将 record 对象加入 DbContext 的跟踪列表
        int count = await db.SaveChangesAsync().NotWaitBack();

        if (count > 0) { result.Success("视频观看记录添加成功"); }
        else { result.Fail("视频观看记录添加失败"); }

        return result;
    }

    #endregion

    #region 修改

    /// <summary>
    /// 修改视频观看记录信息
    /// </summary>
    /// <param name="param">修改参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update(VideoRecordUpdateParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();

        CouserVideoRecord? record = await db.CouserVideoRecords.FindAsync(param.Id);
        if (record == null)
        {
            result.Fail("视频观看记录不存在");
        }
        else
        {
            record.LastPlayPosition = param.LastPlayPosition;
            record.IsCompleted = param.IsCompleted;
            record.UpdateTime = DateTime.Now;

            int count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("视频观看记录修改成功"); }
            else { result.Fail("视频观看记录修改失败"); }
        }

        return result;
    }
    #endregion



}