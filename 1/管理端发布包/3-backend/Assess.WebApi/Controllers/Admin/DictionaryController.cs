using System.ComponentModel.DataAnnotations;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Repository;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 字典操作相关接口
/// </summary>
public class DictionaryController : AdminApiController<DictionaryInfo, IDictionaryInfoService>
{
    public DictionaryController(IDictionaryInfoService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 分页获取字典列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<DictionaryDetail>> Page(DictionaryPageParam param)
    {
        RequestPageResult<DictionaryDetail> result = new();

        EntityPageModel<DictionaryInfo> pageParam = new(param)
        {
            Where = e =>
                (param.Key == null || e.Key == param.Key) &&
                (string.IsNullOrEmpty(param.Keyword) || e.Value.Contains(param.Keyword))
        };

        var page = await entityService.GetPageAsync(e => new DictionaryDetail
        {
            Id = e.Id,
            Key = e.Key,
            Value = e.Value
        }, pageParam);

        if (page?.Total > 0) { result.LoadPage(page); }
        else { result.Success(); }

        return result;
    }

    /// <summary>
    /// 添加新字典信息
    /// </summary>
    /// <param name="key">字典键</param>
    /// <param name="value">字典值</param>
    /// <returns></returns>
    [HttpGet]
    public Task<RequestResult> Insert([Required] DictionaryType key, [Required] string value)
    {
        return entityService.InsertAsync(key, value.Trim());
    }

    /// <summary>
    /// 更改字典信息
    /// </summary>
    /// <param name="id">字典标识</param>
    /// <param name="value">字典值</param>
    /// <returns></returns>
    [HttpGet]
    public Task<RequestResult> Update([Required] long id, [Required] string value)
    {
        return entityService.UpdateAsync(id, value.Trim());
    }

    /// <summary>
    /// 删除某个字典信息
    /// </summary>
    /// <param name="id">字典标识</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete(long id)
    {
        RequestResult result = new();

        int count = await entityService.DeleteAsync(e => e.Id == id).NotWaitBack();

        if (count > 0) { result.Success("删除成功"); }
        else { result.Fail("删除失败"); }

        return result;
    }

    /// <summary>
    /// 通过字典键获取字典值列表
    /// </summary>
    /// <param name="key">字典键</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<SelectResult> Select(DictionaryType key)
    {
        SelectResult result = new();

        List<SelectNode> nodes = await entityService.GetListAsync(e => e.Key == key,
            e => new SelectNode
            {
                Text = e.Value,
                Value = e.Value,
            }).NotWaitBack();

        result.Data = nodes.ToArray();
        result.Success();

        return result;
    }
}
