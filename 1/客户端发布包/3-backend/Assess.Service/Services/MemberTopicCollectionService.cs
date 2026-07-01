using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IMemberTopicCollectionService : IBusinessServiceBase<MemberTopicCollection>
{
}

public class MemberTopicCollectionService : BusinessServiceBase<MemberTopicCollection>, IMemberTopicCollectionService
{
    public MemberTopicCollectionService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
