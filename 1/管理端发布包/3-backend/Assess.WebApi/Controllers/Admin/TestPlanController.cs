using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Mapster;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 测评方案相关
/// </summary>
public class TestPlanController : AdminApiController<TestPlan, ITestPlanService>
{
    public TestPlanController(
        ITestPlanService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 给专项添加测评方案
    /// </summary>
    /// <param name="param">方案参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert(TestPlanInsertParam param)
    {
        RequestResult result = new();

        TestPlan entity = param.Adapt<TestPlan>();
        var code = await entityService.InsertAsync(entity).NotWaitBack();
        if (code) { result.Success("添加成功"); }
        else { result.Fail("添加失败"); }

        return result;
    }

    /// <summary>
    /// 给专项更新测评方案
    /// </summary>
    /// <param name="param">方案参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update(TestPlanUpdateParam param)
    {
        RequestResult result = new();

        TestPlan entity = param.Adapt<TestPlan>();
        var code = await entityService.UpdateAsync(entity).NotWaitBack();
        if (code) { result.Success("更新成功"); }
        else { result.Fail("更新失败"); }

        return result;
    }


    /// <summary>
    /// 获取专项测评方案
    /// </summary>
    /// <param name="partId">专项ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<object>> Detail(int partId)
    {
        RequestResult<object> result = new();

        var entity = await entityService.GetEntityAsync(e => e.PartId == partId).NotWaitBack();
        if (entity == null) { result.Fail("该专项未设置测评方案"); }
        else
        {
            result.Data = new
            {
                entity.Id,
                entity.TestTime,
                entity.TotalScore,
                entity.TopicNumber,
            };
            result.Success();
        }

        return result;
    }
}
