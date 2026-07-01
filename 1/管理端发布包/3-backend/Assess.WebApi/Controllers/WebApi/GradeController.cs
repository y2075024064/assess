using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Entity;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 推荐年级相关
/// </summary>
public class GradeController : WebApiController<GradeInfo, IGradeService>
{
    public GradeController(IGradeService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 获取练习界面年级选项列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<string[]>> List()
    {
        RequestResult<string[]> result = new();

        var list = await entityService.GetListAsync(e => e.IsShow,
            e => new GradeInfo { Name = e.Name, SortLevel = e.SortLevel }).NotWaitBack();

        if (list?.Count > 0)
        {
            result.Data = list.OrderBy(e => e.SortLevel).Select(e => e.Name).ToArray();
        }
        else { result.Data = Array.Empty<string>(); }
        result.Success();

        return result;
    }

    /// <summary>
    /// 获取在使用中的年级选项列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AccreditAction(IgnoreAuth = true)]
    public async Task<RequestResult<string[]>> UsedList()
    {
        RequestResult<string[]> result = new();

        var list = await entityService.GetListAsync(e => e.IsUse,
            e => new GradeInfo { Name = e.Name, SortLevel = e.SortLevel }).NotWaitBack();

        if (list?.Count > 0)
        {
            result.Data = list.OrderBy(e => e.SortLevel).Select(e => e.Name).ToArray();
        }
        else { result.Data = Array.Empty<string>(); }
        result.Success();

        return result;
    }
}
