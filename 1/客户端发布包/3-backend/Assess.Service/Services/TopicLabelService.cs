using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface ITopicLabelService : IBusinessServiceBase<TopicLabel>
{
}

public class TopicLabelService : BusinessServiceBase<TopicLabel>, ITopicLabelService
{
    public TopicLabelService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
