using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IStudentService : IBusinessServiceBase<StudentInfo>
{
}

public class StudentService : BusinessServiceBase<StudentInfo>, IStudentService
{
    public StudentService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
