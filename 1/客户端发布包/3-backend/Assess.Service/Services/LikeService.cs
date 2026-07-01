using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface ILikeService : IBusinessServiceBase<LikeRecord>
{
}

public class LikeService : BusinessServiceBase<LikeRecord>, ILikeService
{
    public LikeService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
