using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IExamApplyService : IBusinessServiceBase<ExamApplyInfo>
{
}

public class ExamApplyService : BusinessServiceBase<ExamApplyInfo>, IExamApplyService
{
    public ExamApplyService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
