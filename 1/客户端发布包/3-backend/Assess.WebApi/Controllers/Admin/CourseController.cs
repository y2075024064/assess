using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using System.Data;
using Repository;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 课程管理相关接口
/// </summary>
public class CourseController : AdminApiController<Course, ICourseService>
{
    public CourseController(ICourseService entityService) : base(entityService)
    {
    }

    #region 分页

    /// <summary>
    /// 获取课程分页列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<CoursePageItem>> Page(CoursePageParam param)
    {
        RequestPageResult<CoursePageItem> result = new();

        EntityPageModel<Course> entityParam = new(param)
        {
            // 构造查询条件  
            Where = e =>
                (param.CategoryId == null || e.CategoryId == param.CategoryId) &&
                (param.TeacherId == null || e.TeacherId == param.TeacherId) &&
                (param.Status == null || e.Status == param.Status) &&
                (string.IsNullOrEmpty(param.Keyword) ||
                 e.Title.Contains(param.Keyword) ||
                 e.Description!.Contains(param.Keyword))
        };

        // 查询数据库 第一个参数是查询结果并且做了规则映射   第二个传查询条件
        var page = await entityService.GetPageAsync(e => new CoursePageItem
        {
            Id = e.Id,
            Title = e.Title,
            Price = e.Price,
            DiscountPrice = e.DiscountPrice,
            Status = e.Status,
            CategoryId = e.CategoryId,
            CategoryName = e.Category!.Name,
            TeacherId = e.TeacherId,
            CreateTime = e.CreateTime,
            CoverImageUrl = e.CoverImageUrl
        }, entityParam).NotWaitBack();

        if (page?.Total > 0) { result.LoadPage(page); }
        else { result.Success(); }

        return result;
    }

    #endregion

    #region 新增

    /// <summary>
    /// 新增课程
    /// </summary>
    /// <param name="param">新增参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert(CourseInsertParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();

        Course course = new()
        {
            Title = param.Title,
            Description = param.Description,
            CoverImageUrl = param.CoverImageUrl,
            Price = param.Price,
            DiscountPrice = param.DiscountPrice,
            CategoryId = param.CategoryId,
            TeacherId = param.TeacherId,
            Status = param.Status,
            CreateTime = DateTime.Now,
            UpdateTime = DateTime.Now
        };

        _ = db.Courses.Add(course); // 将 course 对象加入 DbContext 的跟踪列表 _忽略返回值
        int count = await db.SaveChangesAsync().NotWaitBack();

        if (count > 0) { result.Success("课程添加成功"); }
        else { result.Fail("课程添加失败"); }

        return result;
    }

    #endregion

    #region 修改

    /// <summary>
    /// 修改课程信息
    /// </summary>
    /// <param name="param">修改参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update(CourseUpdateParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();

        Course? course = await db.Courses.FindAsync(param.Id);
        if (course == null)
        {
            result.Fail("课程不存在");
        }
        else
        {
            course.Title = param.Title;
            course.Description = param.Description;
            course.CoverImageUrl = param.CoverImageUrl;
            course.Price = param.Price;
            course.DiscountPrice = param.DiscountPrice;
            course.CategoryId = param.CategoryId;
            course.TeacherId = param.TeacherId;
            course.Status = param.Status;
            course.UpdateTime = DateTime.Now;

            int count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("课程修改成功"); }
            else { result.Fail("课程修改失败"); }
        }

        return result;
    }

    #endregion

    #region 删除

    /// <summary>
    /// 删除课程
    /// </summary>
    /// <param name="ids">课程ID集合</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete(int[] ids)
    {
        RequestResult result = new();

        try
        {
            using AssessDBContext db = entityService.GetDbContext();

            var courses = await db.Courses
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();

            db.Courses.RemoveRange(courses);

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
}