using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IMemberDisabledRecordService : IBusinessServiceBase<MemberDisabledRecord>
{
}

public class MemberDisabledRecordService : BusinessServiceBase<MemberDisabledRecord>, IMemberDisabledRecordService
{
    public MemberDisabledRecordService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
