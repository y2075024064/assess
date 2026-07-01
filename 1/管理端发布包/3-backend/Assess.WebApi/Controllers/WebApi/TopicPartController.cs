using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using System.Linq.Expressions;
using Assess.Model.Result;
using Assess.Entity;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 题目类别相关
/// </summary>
public class TopicPartController : WebApiController<TopicPart, ITopicPartService>
{
    public TopicPartController(ITopicPartService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 获取练习类别选项列表
    /// </summary>
    /// <param name="grade">推荐年级</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<SelectResult> List(string? grade)
    {
        SelectResult result = new();

        Expression<Func<TopicPart, bool>> where = e => e.ParentId == null;
        if (grade.IsNotEmpty()) { where = where.AndAlso(e => e.RcmdGrade!.Contains(grade!)); }

        var list = await entityService.GetListAsync(where,
            e => new TopicPart { Id = e.Id, Name = e.Name, SortLevel = e.SortLevel }).NotWaitBack();

        if (list?.Count > 0)
        {
            result.Data = list
                .OrderBy(e => e.SortLevel)
                .Select(e => new SelectNode { Text = e.Name, Value = e.Id.ToString() })
                .ToArray();
        }
        else { result.Data = Array.Empty<SelectNode>(); }
        result.Success();

        return result;
    }

    /// <summary>
    /// 通过父级ID获取子专项列表
    /// </summary>
    /// <param name="parentId">父级ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<SelectResult> ListByParentId(int parentId)
    {
        SelectResult result = new();

        var list = await entityService.GetListAsync(
            e => e.ParentId == parentId,
            e => new TopicPart { Id = e.Id, Name = e.Name, SortLevel = e.SortLevel }).NotWaitBack();
        if (list?.Count > 0)
        {
            result.Data = list
                .OrderBy(e => e.SortLevel)
                .Select(e => new SelectNode { Value = e.Id.ToString(), Text = e.Name })
                .ToArray();
        }
        else { result.Data = Array.Empty<SelectNode>(); }
        result.Success();

        return result;
    }
}
