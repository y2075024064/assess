using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IMemberErrorRecordService : IBusinessServiceBase<MemberErrorRecord>
{
}

public class MemberErrorRecordService : BusinessServiceBase<MemberErrorRecord>, IMemberErrorRecordService
{
    public MemberErrorRecordService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
