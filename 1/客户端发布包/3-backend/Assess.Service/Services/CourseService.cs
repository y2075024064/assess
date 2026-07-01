using Assess.Entity;
using Microsoft.EntityFrameworkCore;

namespace Assess.Service.Services;

public interface ICourseService : IBusinessServiceBase<Course>
{
}

public class CourseService : BusinessServiceBase<Course>, ICourseService
{
    public CourseService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
