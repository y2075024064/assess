using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface ILetterService : IBusinessServiceBase<ConsultLetter>
{
}

public class LetterService : BusinessServiceBase<ConsultLetter>, ILetterService
{
    public LetterService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
