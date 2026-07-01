using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IGradeService : IBusinessServiceBase<GradeInfo>
{
}

public class GradeService : BusinessServiceBase<GradeInfo>, IGradeService
{
    public GradeService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
