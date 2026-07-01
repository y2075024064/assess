using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Entity;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 用户题目收藏相关
/// </summary>
public class TopicCollecteController : WebApiController<MemberTopicCollection, IMemberTopicCollectionService>
{
    public TopicCollecteController(IMemberTopicCollectionService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 用户收藏题目
    /// </summary>
    /// <param name="topicId">题目ID</param>
    /// <param name="groupName">分组名称</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult> Collecte(uint topicId, string groupName = "默认")
    {
        RequestResult result = new();
        uint memberId = (uint)CurrentMemberId!;

        using AssessDBContext db = entityService.GetDbContext();
        int count = await db.MemberTopicCollections
            .CountAsync(e => e.MemberId == memberId && e.TopicId == topicId).NotWaitBack();
        if (count <= 0)
        {
            MemberTopicCollection entity = new()
            {
                TopicId = topicId,
                MemberId = memberId,
                GroupName = groupName,
                CreationTime = DateTime.Now,
            };
            _ = db.MemberTopicCollections.Add(entity);

            count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("收藏成功"); }
            else { result.Fail("收藏失败"); }
        }
        else { result.Success("收藏成功"); }

        return result;
    }

    /// <summary>
    /// 用户取消收藏的题目
    /// </summary>
    /// <param name="topicId">题目ID</param>
    /// <param name="number">题目编号</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult> UnCollecte(uint? topicId = null, string? number = null)
    {
        int count = 0;
        RequestResult result = new();
        uint memberId = (uint)CurrentMemberId!;

        if (topicId > 0)
        {
            count = await entityService.DeleteAsync(e => e.MemberId == memberId && e.TopicId == topicId).NotWaitBack();
        }
        else if (number.IsNotEmpty())
        {
            count = await entityService.DeleteAsync(e => e.MemberId == memberId && e.TopicInfo!.Number == number).NotWaitBack();
        }
        else
        {
            result.Fail("参数不合法");
            return result;
        }

        if (count > 0) { result.Success("取消成功"); }
        else { result.Fail("取消失败"); }

        return result;
    }

    /// <summary>
    /// 用户获取收藏总量信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<TopicCollecteResult>> TotalInfo()
    {
        TopicCollecteResult model = new();
        uint memberId = (uint)CurrentMemberId!;
        RequestResult<TopicCollecteResult> result = new();

        List<SelectNode> list = await entityService.GetListAsync(e => e.MemberId == memberId,
            e => new SelectNode { Text = e.GroupName, Value = e.TopicInfo!.Number }).NotWaitBack();

        model.Total = list.Count;
        model.Groups = list.GroupBy(e => e.Text).Select(g => new TopicCollecteNode()
        {
            GroupName = g.Key,
            TopicNumbers = g.Select(e => (string?)e.Value).OrderBy(e => e).ToArray()
        }).ToArray();

        result.Data = model;
        result.Success();

        return result;
    }

    /// <summary>
    /// 用户通过分组名称获取收藏题目编号列表
    /// </summary>
    /// <param name="groupName">分组名称</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<string?[]>> GetList(string? groupName)
    {
        RequestResult<string?[]> result = new();
        uint memberId = (uint)CurrentMemberId!;

        List<SelectNode> list = await entityService.GetListAsync(
            e => e.MemberId == memberId && (string.IsNullOrEmpty(groupName) || e.GroupName == groupName),
            e => new SelectNode { Value = e.TopicId, Text = e.TopicInfo!.Number }).NotWaitBack();

        result.Data = list.Select(e => e.Text).ToArray();
        result.Success();

        return result;
    }
}
