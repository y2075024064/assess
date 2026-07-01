using Microsoft.EntityFrameworkCore;
using Assess.Service;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IMemberAccountService : IBusinessServiceBase<MemberAccount>
{
}

public class MemberAccountService : BusinessServiceBase<MemberAccount>, IMemberAccountService
{
    public MemberAccountService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}