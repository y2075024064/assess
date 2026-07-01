using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Repository;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 课程视频管理相关接口
/// </summary>
public class CourseVideoController : WebApiController<CourseVideo, ICourseVideoService>
{
    public CourseVideoController(ICourseVideoService entityService) : base(entityService)
    {
    }

    #region 分页

    /// <summary>
    /// 获取课程视频分页列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<CourseVideoItem>> Page(CourseVideoPageParam param)
    {
        RequestPageResult<CourseVideoItem> result = new();

        EntityPageModel<CourseVideo> entityParam = new(param)
        {
            // 构造查询条件
            Where = e =>
                (param.CourseId == null || e.CourseId == param.CourseId) &&
                (param.IsFree == null || e.IsFree == param.IsFree) &&
                (string.IsNullOrEmpty(param.Keyword) || e.Title.Contains(param.Keyword))
        };

        // 查询数据库
        var page = await entityService.GetPageAsync(e => new CourseVideoItem
        {
            Id = e.Id,
            CourseId = e.CourseId,
            Title = e.Title,
            VideoUrl = e.VideoUrl,
            Duration = e.Duration,
            SortOrder = e.SortOrder,
            IsFree = e.IsFree,
            CreateTime = e.CreateTime,
        }, entityParam).NotWaitBack();

        if (page?.Total > 0) { result.LoadPage(page); }
        else { result.Success(); }

        return result;
    }

    #endregion

    #region 获取课程视频详情
    /// <summary>
    /// 获取课程视频详情(会返回用户的观看记录的)
    /// </summary>
    /// <param name="videoRecordParam">videoRecordParam查询参数</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<CourseVideoDetail>> GetDetail( VideoRecordParam videoRecordParam)
    {
        RequestResult<CourseVideoDetail> result = new();

        using AssessDBContext db = entityService.GetDbContext();

        // 1. 获取视频基本信息
        var video = await db.CourseVideos
            .Include(v => v.Course)
            .Where(v => v.Id == videoRecordParam.CourseId)
            .Select(v => new CourseVideoItem
            {
                Id = v.Id,
                CourseId = v.CourseId,
                Title = v.Title,
                VideoUrl = v.VideoUrl,
                Duration = v.Duration,
                SortOrder = v.SortOrder,
                IsFree = v.IsFree,
                CreateTime = v.CreateTime
            })
            .FirstOrDefaultAsync();

        if (video == null)
        {
            result.Fail("视频不存在");
            return result;
        }

        // 2. 构建返回对象
        var response = new CourseVideoDetail
        {
            VideoInfo = video,
            WatchRecord = null
        };

        // 3. 如果传入了用户ID，查询观看记录
        
        var watchRecord = await db.CouserVideoRecords
                .Where(r => r.VideoId == videoRecordParam.VideoId && r.MemberId == videoRecordParam.MemberId)
                .Select(r => new VideoWatchRecordItem
                {
                    LastPlayPosition = r.LastPlayPosition,
                    IsCompleted = r.IsCompleted,
                })        
                .FirstOrDefaultAsync();

        response.WatchRecord = watchRecord;

        result.Data = response;
        result.Success("查询成功");
        return result;
    }
    #endregion

}