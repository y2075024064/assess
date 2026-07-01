using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IAdModuleService : IBusinessServiceBase<AdModule>
{
}

public class AdModuleService : BusinessServiceBase<AdModule>, IAdModuleService
{
    public AdModuleService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
