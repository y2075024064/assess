using Assess.Entity;
using Microsoft.EntityFrameworkCore;

namespace Assess.Service.Services;
/// <summary>
/// 课程视频服务接口及实现
/// </summary>
public interface ICourseVideoService : IBusinessServiceBase<CourseVideo>
{
}
public class CourseVideoService : BusinessServiceBase<CourseVideo>, ICourseVideoService
{
    public CourseVideoService(DbContextOptions<AssessDBContext> options) : base(options) { }
}
   
