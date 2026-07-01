using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface ITopicOptionsService : IBusinessServiceBase<TopicOptions>
{
}

public class TopicOptionsService : BusinessServiceBase<TopicOptions>, ITopicOptionsService
{
    public TopicOptionsService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
