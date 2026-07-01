using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Repository;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 课程视频管理相关接口
/// </summary>
public class CourseVideoController : AdminApiController<CourseVideo, ICourseVideoService>
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

    #region 新增

    /// <summary>
    /// 新增课程视频
    /// </summary>
    /// <param name="param">新增参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert(CourseVideoInsertParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();

        // 检查课程是否存在
        var courseExists = await db.Courses.AnyAsync(c => c.Id == param.CourseId);
        if (!courseExists)
        {
            result.Fail("所属课程不存在");
            return result;
        }

        // 检查视频标题是否重复
        var titleExists = await db.CourseVideos
            .AnyAsync(v => v.CourseId == param.CourseId && v.Title == param.Title);

        if (titleExists)
        {
            result.Fail("该课程下已存在相同标题的视频");
            return result;
        }

        CourseVideo video = new()
        {
            CourseId = param.CourseId,
            Title = param.Title,
            VideoUrl = param.VideoUrl,
            Duration = param.Duration,
            SortOrder = param.SortOrder,
            IsFree = param.IsFree,
            CreateTime = DateTime.Now
        };

        _ = db.CourseVideos.Add(video);
        int count = await db.SaveChangesAsync().NotWaitBack();

        if (count > 0) { result.Success("视频添加成功"); }
        else { result.Fail("视频添加失败"); }

        return result;
    }

    #endregion

    #region 修改

    /// <summary>
    /// 修改课程视频信息
    /// </summary>
    /// <param name="param">修改参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update(CourseVideoUpdateParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();

        CourseVideo? video = await db.CourseVideos.FindAsync(param.Id);
        if (video == null)
        {
            result.Fail("视频不存在");
            return result;
        }

        // 检查课程是否存在
        if (video.CourseId != param.CourseId)
        {
            var courseExists = await db.Courses.AnyAsync(c => c.Id == param.CourseId);
            if (!courseExists)
            {
                result.Fail("所属课程不存在");
                return result;
            }
        }

        // 检查视频标题是否重复（排除自己）
        var titleExists = await db.CourseVideos
            .AnyAsync(v => v.CourseId == param.CourseId &&
                           v.Title == param.Title &&
                           v.Id != param.Id);

        if (titleExists)
        {
            result.Fail("该课程下已存在相同标题的视频");
            return result;
        }

        video.CourseId = param.CourseId;
        video.Title = param.Title;
        video.VideoUrl = param.VideoUrl;
        video.Duration = param.Duration;
        video.SortOrder = param.SortOrder;
        video.IsFree = param.IsFree;

        int count = await db.SaveChangesAsync().NotWaitBack();
        if (count > 0) { result.Success("视频修改成功"); }
        else { result.Fail("视频修改失败"); }

        return result;
    }

    #endregion

    #region 删除

    /// <summary>
    /// 删除课程视频
    /// </summary>
    /// <param name="ids">视频ID集合</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete(int[] ids)
    {
        RequestResult result = new();

        try
        {
            using AssessDBContext db = entityService.GetDbContext();

            var videos = await db.CourseVideos
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();

            db.CourseVideos.RemoveRange(videos);

            int count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("删除成功"); }
            else { result.Fail("删除失败"); }
        }
        catch (Exception ex)
        {
            result.Fail(ex.Message);
        }

        return result;
    }

    #endregion

    /// <summary>
    /// 获取课程视频详情
    /// </summary>
    /// <param name="id">视频ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<CourseVideoItem>> GetDetail(int id)
    {
        RequestResult<CourseVideoItem> result = new();

        using AssessDBContext db = entityService.GetDbContext();

        var video = await db.CourseVideos
            .Include(v => v.Course)
            .Where(v => v.Id == id)
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

        if (video != null)
        {
            result.Data = video;  // 直接赋值Data属性
            result.Success();
        }
        else
        {
            result.Fail("视频不存在");
        }

        return result;
    }



}