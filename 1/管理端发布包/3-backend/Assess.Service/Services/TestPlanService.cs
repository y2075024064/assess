using Microsoft.EntityFrameworkCore;
using Assess.Entity;

namespace Assess.Service.Services;

public interface ITestPlanService : IBusinessServiceBase<TestPlan>
{
}

public class TestPlanService : BusinessServiceBase<TestPlan>, ITestPlanService
{
    public TestPlanService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
