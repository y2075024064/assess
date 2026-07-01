using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IAdvertisingService : IBusinessServiceBase<Advertising>
{
}

public class AdvertisingService : BusinessServiceBase<Advertising>, IAdvertisingService
{
    public AdvertisingService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
