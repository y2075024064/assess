using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Entity;
using Repository;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 我的考试相关接口
/// </summary>
public class ExamApplyController : WebApiController<ExamApplyInfo, IExamApplyService>
{
    public ExamApplyController(IExamApplyService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 分页获取我的比赛或者考试列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<MyExamInfo>> MyExam(DefaultPageParam param)
    {
        var userId = CurrentMemberId!;
        RequestPageResult<MyExamInfo> result = new();

        var page = await entityService.GetPageAsync(e => e.MemberId == userId,
            e => new MyExamInfo
            {
                IsPay = e.IsPay,
                ApplyTime = e.ApplyTime,
                Name = e.Examination!.Name,
                ExaminationId = e.ExaminationId,
                ExamineRecordId = e.ExamineRecordId,
                Score = e.ExamineRecord!.TotalScore,
                StartDate = e.Examination.StartDate,
                StartTime = e.Examination.StartTime,
                IsTested = e.ExamineRecordId != null,
            },
            e => e.ApplyTime, OrderRule.Desc, param.Page, param.Take).NotWaitBack();

        if (page?.Total > 0)
        {
            DateTime now = DateTime.Now;
            DateOnly today = new(now.Year, now.Month, now.Day);
            page.Data.ForEach(item =>
            {
                if (item.StartDate.HasValue)
                {
                    item.Days = item.StartDate.Value.DayNumber - today.DayNumber;
                }
            });
            result.Data = page.Data.ToArray();
        }
        result.Success();

        return result;
    }

    /// <summary>
    /// 获取考试答题详细信息
    /// </summary>
    /// <param name="recordId">考试记录ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<ExamRecordDetail>> AnswerInfo(long recordId)
    {
        RequestResult<ExamRecordDetail> result = new();

        var record = await entityService.Adapter.GetEntityAsync<ExamineRecord, ExamineRecord>(
            e => e.Id == recordId, e => new ExamineRecord
            {
                OverTime = e.OverTime,
                TotalScore = e.TotalScore,
                ErrorIndexs = e.ErrorIndexs,
                CreationTime = e.CreationTime,
                AnswerRecord = e.AnswerRecord,
                ExaminationId = e.ExaminationId,
            }).NotWaitBack();

        if (record != null)
        {
            var nodes = await entityService.Adapter.GetListAsync<ExaminationTopic, SelectNode>(
                e => e.ExaminationId == record.ExaminationId,
                e => new SelectNode
                {
                    Value = e.Id,
                    Text = e.Topic!.Number,
                }).NotWaitBack();

            result.Data = new ExamRecordDetail
            {
                AnswerRecord = record.AnswerRecord,
                CreationTime = record.CreationTime,
                ErrorIndexs = record.ErrorIndexs,
                TotalScore = record.TotalScore,
                OverTime = record.OverTime,
            };

            if (nodes.Count > 0)
            {
                result.Data.Numbers = nodes
                    .OrderBy(x => x.Value)
                    .Select(e => e.Text!)
                    .ToArray();
            }
            result.Success();
        }
        else { result.Fail("您请求的数据不存在"); }

        return result;
    }
}
