using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IExamineRecordService : IBusinessServiceBase<ExamineRecord>
{
}

public class ExamineRecordService : BusinessServiceBase<ExamineRecord>, IExamineRecordService
{
    public ExamineRecordService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
