using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Repository;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 课程分类管理相关接口
/// </summary>
public class CourseCategoryController : AdminApiController<CourseCategory, ICourseCategoryService>
{
    public CourseCategoryController(ICourseCategoryService entityService) : base(entityService)
    {
    }

    #region 分页

    /// <summary>
    /// 获取课程分类分页列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<CourseCategoryItem>> Page(CourseCategoryPageParam param)
    {
        RequestPageResult<CourseCategoryItem> result = new();

        EntityPageModel<CourseCategory> entityParam = new(param)
        {
            // 构造查询条件
            Where = e =>
                (param.ParentId == null || e.ParentId == param.ParentId) &&
                (string.IsNullOrEmpty(param.Keyword) ||
                 e.Name.Contains(param.Keyword) ||
                 e.Description!.Contains(param.Keyword))
        };

        // 查询数据库
        var page = await entityService.GetPageAsync(e => new CourseCategoryItem
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            ParentId = e.ParentId,
            ParentName = e.ParentCategory != null ? e.ParentCategory.Name : null,
            SortWeight = e.SortWeight,
            CreateTime = e.CreateTime,
        }, entityParam).NotWaitBack();

        if (page?.Total > 0) { result.LoadPage(page); }
        else { result.Success(); }

        return result;
    }

    #endregion

    #region 新增

    /// <summary>
    /// 新增课程分类
    /// </summary>
    /// <param name="param">新增参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert(CourseCategoryInsertParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();

        // 检查是否已存在同名分类（同级别下）
        bool exists = await db.CourseCategorys
            .AnyAsync(e => e.Name == param.Name &&
                           e.ParentId == param.ParentId);

        if (exists)
        {
            result.Fail("同级分类中已存在相同名称的分类");
            return result;
        }

        CourseCategory category = new()
        {
            Name = param.Name,
            Description = param.Description,
            ParentId = param.ParentId,
            SortWeight = param.SortWeight,
            CreateTime = DateTime.Now
        };

        _ = db.CourseCategorys.Add(category);
        int count = await db.SaveChangesAsync().NotWaitBack();

        if (count > 0) { result.Success("分类添加成功"); }
        else { result.Fail("分类添加失败"); }

        return result;
    }

    #endregion

    #region 修改

    /// <summary>
    /// 修改课程分类信息
    /// </summary>
    /// <param name="param">修改参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update(CourseCategoryUpdateParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();

        CourseCategory? category = await db.CourseCategorys.FindAsync(param.Id);
        if (category == null)
        {
            result.Fail("分类不存在");
        }
        else
        {
            category.Name = param.Name;
            category.Description = param.Description;
            category.ParentId = param.ParentId;
            category.SortWeight = param.SortWeight;

            int count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("分类修改成功"); }
            else { result.Fail("分类修改失败"); }
        }

        return result;
    }

    #endregion

    #region 删除

    /// <summary>
    /// 删除课程分类
    /// </summary>
    /// <param name="ids">分类ID集合</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete(int[] ids)
    {
        RequestResult result = new();

        try
        {
            using AssessDBContext db = entityService.GetDbContext();

            // 检查是否有子分类或关联课程
            var hasChildren = await db.CourseCategorys
                .AnyAsync(e => ids.Contains(e.ParentId ?? 0));
            if (hasChildren )
            {
                result.Fail("存在子分类，无法删除");
                return result;
            }

            var hasCourses = await db.Courses
                .AnyAsync(e => ids.Contains(e.CategoryId));

            if (hasCourses)
            {
                result.Fail("存在关联课程，无法删除");
                return result;
            }

            var categories = await db.CourseCategorys
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();

            db.CourseCategorys.RemoveRange(categories);

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