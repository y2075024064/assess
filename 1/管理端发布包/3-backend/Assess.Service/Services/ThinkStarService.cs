using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IThinkStarService : IBusinessServiceBase<ThinkStar>
{
}

public class ThinkStarService : BusinessServiceBase<ThinkStar>, IThinkStarService
{
    public ThinkStarService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
