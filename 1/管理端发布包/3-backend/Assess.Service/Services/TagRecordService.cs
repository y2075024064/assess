using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface ITagRecordService : IBusinessServiceBase<TagRecord>
{
}

public class TagRecordService : BusinessServiceBase<TagRecord>, ITagRecordService
{
    public TagRecordService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
