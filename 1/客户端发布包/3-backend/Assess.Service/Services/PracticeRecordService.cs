using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IPracticeRecordService : IBusinessServiceBase<PracticeRecord>
{
}

public class PracticeRecordService : BusinessServiceBase<PracticeRecord>, IPracticeRecordService
{
    public PracticeRecordService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
