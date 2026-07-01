using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Entity;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 考试题目管理相关接口
/// </summary>
public class ExamTopicController : AdminApiController<ExaminationTopic, IExamTopicService>
{
    public ExamTopicController(IExamTopicService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 获取考试题目列表
    /// </summary>
    /// <param name="examId">考试ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<ExamTopicListItem[]>> List(int examId)
    {
        RequestResult<ExamTopicListItem[]> result = new();

        var list = await entityService.GetListAsync(
            e => e.ExaminationId == examId,
            e => new ExamTopicListItem
            {
                Id = e.Id,
                Score = e.Score,
                TopicId = e.TopicId,
                Label = e.Topic!.Label,
                Level = e.Topic!.Level,
                Title = e.Topic!.Title,
                RcmdGrade = e.Topic!.RcmdGrade,
                Content = e.Topic!.Content.Substring(0, 200)
            }).NotWaitBack();

        result.Data = list.ToArray();
        result.Success();

        return result;
    }

    /// <summary>
    /// 批量添加题目
    /// </summary>
    /// <param name="examId">考试ID</param>
    /// <param name="topicIds">题目ID集合</param>
    /// <param name="score">题目分数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Add(int examId, int score, [FromBody] uint[] topicIds)
    {
        RequestResult result = new();

        if (topicIds?.Length > 0)
        {
            using var db = entityService.GetDbContext();

            var list = await db.ExaminationTopics.Where(
                e => e.ExaminationId == examId && topicIds.Contains(e.TopicId))
                .ToListAsync().NotWaitBack();

            foreach (uint id in topicIds)
            {
                var item = list.FirstOrDefault(e => e.TopicId == id);
                if (item != null) { item.Score = score; }
                else
                {
                    ExaminationTopic entity = new()
                    {
                        ExaminationId = examId,
                        Score = score,
                        TopicId = id,
                    };
                    _ = await db.AddAsync(entity);
                }
            }

            int count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("添加成功"); }
            else { result.Fail("添加失败"); }
        }
        else { result.Fail("题目集合不能为空"); }

        return result;
    }

    /// <summary>
    /// 批量设置题目分数
    /// </summary>
    /// <param name="score">题目分数</param>
    /// <param name="ids">试题题目ID集合</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> SetScore(int score, [FromBody] long[] ids)
    {
        RequestResult result = new();

        if (ids?.Length > 0)
        {
            int count = await entityService.UpdateAsync(e => ids.Contains(e.Id),
                e => new ExaminationTopic { Score = score }).NotWaitBack();

            if (count > 0) { result.Success("设置成功"); }
            else { result.Fail("设置失败"); }
        }
        else { result.Fail("题目集合不能为空"); }

        return result;
    }

    /// <summary>
    /// 批量移除题目
    /// </summary>
    /// <param name="ids">考试题目ID集合</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Remove([FromBody] long[] ids)
    {
        RequestResult result = new();

        long count = await entityService.DeleteAsync(e => ids.Contains(e.Id)).NotWaitBack();
        if (count > 0) { result.Success("移除成功"); }
        else { result.Fail("移除失败"); }

        return result;
    }
}
