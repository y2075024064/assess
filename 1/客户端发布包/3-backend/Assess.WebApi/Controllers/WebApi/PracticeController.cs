using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using System.Linq.Expressions;
using Assess.Model.Result;
using Assess.Entity;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 练习记录相关接口
/// </summary>
public class PracticeController : WebApiController<PracticeRecord, IPracticeRecordService>
{
    public PracticeController(IPracticeRecordService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 获取当前练习过的数量
    /// </summary>
    /// <param name="grade">选择的年级</param>
    /// <param name="partId">选择的类别</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<long>> CurrNumber(string? grade, int? partId)
    {
        RequestResult<long> result = new();

        Expression<Func<PracticeRecord, bool>> where = e => e.MemberId == CurrentMemberId;
        if (partId > 0) { where = where.AndAlso(e => e.TopicInfo!.PartId == partId); }
        if (grade.IsNotEmpty()) { where = where.AndAlso(e => e.TopicInfo!.RcmdGrade!.Contains(grade!)); }

        result.Data = await entityService.GetCountAsync(where).NotWaitBack();
        result.Success();

        return result;
    }
}
