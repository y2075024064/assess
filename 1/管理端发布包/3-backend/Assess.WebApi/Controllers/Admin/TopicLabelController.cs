using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Repository;
using Mapster;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 题目标签管理
/// </summary>
public class TopicLabelController : AdminApiController<TopicLabel, ITopicLabelService>
{
    public TopicLabelController(
        ITopicLabelService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 分页获取题目标签列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<TopicLabelPageResult>> Page(DefaultEntityPageParam param)
    {
        RequestPageResult<TopicLabelPageResult> result = new();
        EntityPageModel<TopicLabel> pageModel = new(param);

        if (!string.IsNullOrWhiteSpace(param.Keyword))
        {
            pageModel.Where = e => e.Name.Contains(param.Keyword) || e.Remark!.Contains(param.Keyword);
        }

        var page = await entityService.GetPageAsync<TopicLabelPageResult>(pageModel).NotWaitBack();
        if (page != null) { result.LoadPage(page); }

        return result;
    }

    /// <summary>
    /// 新增题目标签
    /// </summary>
    /// <param name="param">新增的题目标签参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert(TopicLabelInsertParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();
        int count = await db.TopicLabels.CountAsync(e => e.Name == param.Name).NotWaitBack();
        if (count > 0) { result.Fail("该名称重复"); }
        else
        {
            TopicLabel entity = param.Adapt<TopicLabel>();
            db.TopicLabels.Add(entity);

            count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("新增成功"); }
            else { result.Fail("新增失败"); }
        }

        return result;
    }

    /// <summary>
    /// 更改题目标签
    /// </summary>
    /// <param name="param">更改的题目标签参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update(TopicLabelUpdateParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();
        var entity = await db.TopicLabels.FindAsync(param.Id).ConfigureAwait(false);
        if (entity == null) { result.Fail("您要更改的对象不存在"); }
        else
        {
            int count = 0;
            bool hasChange = false;
            if (entity.Name != param.Name)
            {
                count = await db.TopicLabels.CountAsync(e => e.Name == param.Name).NotWaitBack();
                if (count > 0)
                {
                    result.Fail("该名称重复");
                    return result;
                }
                else
                {
                    entity.OldName = entity.Name;
                    entity.Name = param.Name;
                    hasChange = true;
                }
            }

            if (entity.Remark != param.Remark)
            {
                entity.Remark = param.Remark;
                hasChange = true;
            }

            if (hasChange)
            {
                entity.ModificationTime = DateTime.Now;

                count = await db.SaveChangesAsync().NotWaitBack();
                if (count > 0) { result.Success("更改成功"); }
                else { result.Fail("更改失败"); }
            }
            else { result.Fail("无更改项"); }
        }

        return result;
    }

    /// <summary>
    /// 删除题目标签
    /// </summary>
    /// <param name="ids">标签ID集合</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete(int[] ids)
    {
        RequestResult result = new();

        int count = await entityService.DeleteAsync(e => ids.Contains(e.Id)).NotWaitBack();
        if (count > 0) { result.Success("删除成功"); }
        else { result.Fail("删除失败"); }

        return result;
    }

    /// <summary>
    /// 获取 Select 数据源
    /// </summary>
    /// <param name="q">模糊查询关键字</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<SelectResult> Select(string? q)
    {
        SelectResult result = new();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var list = await entityService.GetListAsync(e => e.Name.Contains(q),
                e => new SelectNode { Text = e.Name, Value = e.Name }).NotWaitBack();
            if (list?.Count > 0)
            {
                result.Success();
                result.Data = list.ToArray();
            }
        }

        return result;
    }
}
