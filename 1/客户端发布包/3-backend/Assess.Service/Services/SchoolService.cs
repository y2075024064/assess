using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface ISchoolService : IBusinessServiceBase<SchoolInfo>
{
}

public class SchoolService : BusinessServiceBase<SchoolInfo>, ISchoolService
{
    public SchoolService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
