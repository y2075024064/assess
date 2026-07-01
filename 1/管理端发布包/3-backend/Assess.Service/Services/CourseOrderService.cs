using Assess.Entity;
using Microsoft.EntityFrameworkCore;

namespace Assess.Service.Services;

public interface ICourseOrderService : IBusinessServiceBase<CourseOrder>
{
}

public class CourseOrderService : BusinessServiceBase<CourseOrder>, ICourseOrderService
{
    public CourseOrderService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
