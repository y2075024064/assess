using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IMemberLoginLogService : IBusinessServiceBase<MemberLoginLog>
{
}

public class MemberLoginLogService : BusinessServiceBase<MemberLoginLog>, IMemberLoginLogService
{
    public MemberLoginLogService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
