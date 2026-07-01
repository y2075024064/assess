using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Repository;
using System.Collections.Generic;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 课程分类管理相关接口
/// </summary>
public class CourseCategoryController : WebApiController<CourseCategory, ICourseCategoryService>
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

    #region 获取全部课程分类列表
    /// <summary>
    /// 获取全部课程分类列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<List<CourseCategoryItem>>> GetCategoryTree()
    {
        RequestResult < List < CourseCategoryItem>> result = new();
        using var db = entityService.GetDbContext();

        // 1. 单次查询获取所有数据
        var allItems = await db.CourseCategorys
            .OrderBy(c => c.SortWeight)
            .Select(c => new CourseCategoryItem
            {
                Id = c.Id,
                Name = c.Name,
                SortWeight = c.SortWeight,
                Description = c.Description,
                ParentId = c.ParentId,
                CreateTime = c.CreateTime,
            })
            .ToListAsync();
        result.Data = allItems;
        result.Success("查询成功");
        return result;
    }
    #endregion

}