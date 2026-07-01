using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using System.Data;
using Repository;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 课程管理相关接口
/// </summary>
public class CourseController : WebApiController<Course, ICourseService>
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

}