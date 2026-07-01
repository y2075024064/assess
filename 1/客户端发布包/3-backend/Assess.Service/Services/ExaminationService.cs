using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IExaminationService : IBusinessServiceBase<Examination>
{
}

public class ExaminationService : BusinessServiceBase<Examination>, IExaminationService
{
    public ExaminationService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
