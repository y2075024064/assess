using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Z.EntityFramework.Plus;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Repository;
using Mapster;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 题目科目控制器
/// </summary>
public class TopicPartController : AdminApiController<TopicPart, ITopicPartService>
{
    public TopicPartController(ITopicPartService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 分页获取班级列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<TopicPartPageResult>> Page(DefaultEntityPageParam param)
    {
        RequestPageResult<TopicPartPageResult> result = new();
        EntityPageModel<TopicPart> pageModel = new(param)
        {
            Where = e => e.ParentId == null &&
                (string.IsNullOrEmpty(param.Keyword) || e.Name.Contains(param.Keyword))
        };

        var page = await entityService.GetPageAsync(e => new TopicPartPageResult
        {
            Id = e.Id,
            Name = e.Name,
            Remark = e.Remark,
            OldName = e.OldName,
            ParentId = e.ParentId,
            RcmdGrade = e.RcmdGrade,
            SortLevel = e.SortLevel,
            ModificationTime = e.ModificationTime,
            HasChildren = e.Childrens.Any(),
        }, pageModel).NotWaitBack();
        if (page != null) { result.LoadPage(page); }

        return result;
    }

    /// <summary>
    /// 通过父级ID获取所有子节点
    /// </summary>
    /// <param name="id">父级ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<TopicPartPageResult[]>> Childrens(int id)
    {
        RequestResult<TopicPartPageResult[]> result = new();

        var list = await entityService.GetListAsync(e => e.ParentId == id, e => new TopicPartPageResult
        {
            Id = e.Id,
            Name = e.Name,
            Remark = e.Remark,
            OldName = e.OldName,
            ParentId = e.ParentId,
            RcmdGrade = e.RcmdGrade,
            SortLevel = e.SortLevel,
            ModificationTime = e.ModificationTime,
            HasChildren = e.Childrens.Any(),
        }).NotWaitBack();

        result.Data = list?.ToArray() ?? Array.Empty<TopicPartPageResult>();
        result.Success();

        return result;
    }

    /// <summary>
    /// 新增题目科目信息
    /// </summary>
    /// <param name="param">新增的题目科目信息参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert(TopicPartInsertParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();
        int count = await db.TopicParts.CountAsync(e => e.ParentId == param.ParentId && e.Name == param.Name).NotWaitBack();
        if (count > 0) { result.Fail("该名称重复"); }
        else
        {
            TopicPart entity = param.Adapt<TopicPart>();
            if (param.RcmdGrade != null)
            {
                entity.RcmdGrade = param.RcmdGrade.ToJson();
            }
            db.TopicParts.Add(entity);

            count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("新增成功"); }
            else { result.Fail("新增失败"); }
        }

        return result;
    }

    /// <summary>
    /// 更改题目科目信息
    /// </summary>
    /// <param name="param">更改的题目科目信息参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update(TopicPartUpdateParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();
        var entity = await db.TopicParts.FindAsync(param.Id).ConfigureAwait(false);
        if (entity == null) { result.Fail("您要更改的对象不存在"); }
        else
        {
            int count = 0;
            bool hasChange = false;
            if (entity.Name != param.Name)
            {
                count = await db.TopicParts.CountAsync(e => e.ParentId == param.ParentId && e.Name == param.Name).NotWaitBack();
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

            #region 剩余字段判断是否改变

            string? rcmdGrade = null;
            if (param.RcmdGrade != null)
            {
                rcmdGrade = param.RcmdGrade.ToJson();
            }

            if (entity.RcmdGrade != rcmdGrade)
            {
                entity.RcmdGrade = rcmdGrade;
                hasChange = true;
            }

            if (entity.SortLevel != param.SortLevel)
            {
                entity.SortLevel = param.SortLevel;
                hasChange = true;
            }

            if (entity.ParentId != param.ParentId)
            {
                entity.ParentId = param.ParentId;
                hasChange = true;
            }

            if (entity.Remark != param.Remark)
            {
                entity.Remark = param.Remark;
                hasChange = true;
            }

            #endregion

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
    /// 删除题目科目信息
    /// </summary>
    /// <param name="id">题目科目ID</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete(int id)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();
        int count = await DeleteAsync(id, db);

        if (count > 0) { result.Success($"删除成功，共删除 {count} 项"); }
        else { result.Fail("删除失败"); }

        return result;
    }

    /// <summary>
    /// 获取题目专项树数据源
    /// </summary>
    /// <param name="q"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<TopicPartTreeResult[]>> Select(string? q, int? parentId)
    {
        RequestResult<TopicPartTreeResult[]> result = new();

        List<TopicPartTreeResult> list;
        if (parentId == null)
        {
            list = await entityService.GetListAsync(
                e => (string.IsNullOrEmpty(q) || e.Name.Contains(q)) && e.ParentId == null,
                e => new TopicPartTreeResult
                {
                    Id = e.Id,
                    Name = e.Name,
                    IsLeaf = !e.Childrens.Any(),
                }).NotWaitBack();
        }
        else
        {
            list = await entityService.GetListAsync(e => e.ParentId == parentId, e => new TopicPartTreeResult
            {
                Id = e.Id,
                Name = e.Name,
                IsLeaf = !e.Childrens.Any(),
            }).NotWaitBack();
        }

        result.Data = list?.ToArray() ?? Array.Empty<TopicPartTreeResult>();
        result.Success();

        return result;
    }

    /// <summary>
    /// 递归删除本身及所有子项
    /// </summary>
    /// <param name="id"></param>
    /// <param name="db"></param>
    /// <returns></returns>
    private async Task<int> DeleteAsync(int id, AssessDBContext db)
    {
        int count = 0;
        var list = await db.TopicParts.Where(e => e.ParentId == id)
            .Select(e => new { e.Id }).ToListAsync().NotWaitBack();
        if (list?.Count > 0)
        {
            foreach (var item in list)
            {
                count += await DeleteAsync(item.Id, db);
            }
        }

        count += await db.TopicParts.Where(e => e.Id == id).DeleteAsync();

        return count;
    }
}
