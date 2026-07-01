using Assess.Entity;
using Microsoft.EntityFrameworkCore;

namespace Assess.Service.Services;

/// <summary>
/// 课程分类服务接口及实现
/// </summary>
public interface ICourseCategoryService : IBusinessServiceBase<CourseCategory>
{
}

public class CourseCategoryService : BusinessServiceBase<CourseCategory>, ICourseCategoryService
{
    public CourseCategoryService(DbContextOptions<AssessDBContext> options) : base(options) { }

}
