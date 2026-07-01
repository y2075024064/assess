using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IDynamicInfoService : IBusinessServiceBase<DynamicInfo>
{
}

public class DynamicInfoService : BusinessServiceBase<DynamicInfo>, IDynamicInfoService
{
    public DynamicInfoService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
