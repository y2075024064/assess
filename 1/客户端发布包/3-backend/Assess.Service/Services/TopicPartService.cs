using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface ITopicPartService : IBusinessServiceBase<TopicPart>
{
}

public class TopicPartService : BusinessServiceBase<TopicPart>, ITopicPartService
{
    public TopicPartService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
