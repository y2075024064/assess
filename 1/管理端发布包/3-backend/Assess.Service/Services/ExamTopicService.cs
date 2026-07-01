using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IExamTopicService : IBusinessServiceBase<ExaminationTopic>
{
}

public class ExamTopicService : BusinessServiceBase<ExaminationTopic>, IExamTopicService
{
    public ExamTopicService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
