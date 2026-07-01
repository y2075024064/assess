using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Assess.Service.Services;
using System.Linq.Expressions;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 模拟测评记录相关接口
/// </summary>
public class MockTestController : WebApiController<MockTestRecord, IMockTestRecordService>
{
    private readonly ITopicService topicService;

    public MockTestController(
        ITopicService topicService,
        IMockTestRecordService entityService) : base(entityService)
    {
        this.topicService = topicService;
    }

    /// <summary>
    /// 获取当前测评过的最高分
    /// </summary>
    /// <param name="partId">选择的类别</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<decimal>> MaxScore(int? partId)
    {
        RequestResult<decimal> result = new();

        Expression<Func<MockTestRecord, bool>> where = e => e.MemberId == CurrentMemberId;
        if (partId > 0) { where = where.AndAlso(e => e.PartId == partId); }

        using AssessDBContext db = entityService.GetDbContext();
        result.Data = await db.MockTestRecords.Where(where).Select(e => e.CurrScore).DefaultIfEmpty().MaxAsync().NotWaitBack();
        result.Success();

        return result;
    }

    /// <summary>
    /// 获取模拟测评题目列表
    /// </summary>
    /// <param name="partId">练习类别ID</param>
    /// <param name="grade">年级</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<TopicTestRecordModel>> TopicList(int partId, string? grade)
    {
        uint memberId = (uint)CurrentMemberId!;
        RequestResult<TopicTestRecordModel> result = new();

        using AssessDBContext db = entityService.GetDbContext();
        var entity = await db.MockTestRecords.FirstOrDefaultAsync(e => e.MemberId == memberId && e.IsOver == false).NotWaitBack();
        if (entity != null)
        {
            int[] answeredIndexs = entity.AnsweredIndexs.ToModel<int[]>() ?? Array.Empty<int>();
            string[] answers = entity.AnswerRecord.ToModel<string[]>() ?? Array.Empty<string>();
            string[] numbers = entity.TopicInfos.ToModel<string[]>() ?? Array.Empty<string>();

            List<TopicTestRecordItem> items = new();
            for (int i = 0; i < numbers.Length; i++)
            {
                TopicTestRecordItem item = new()
                {
                    Number = numbers[i],
                    IsTested = answeredIndexs.Contains(i),
                };
                if (item.IsTested) { item.Answer = answers[i]; }

                items.Add(item);
            }

            result.Data = new TopicTestRecordModel
            {
                TestId = entity.Id,
                Items = items.ToArray(),
                RemainTime = entity.RemainTime,
                CurrPostion = entity.CurrPostion,
            };
            result.Success();
        }
        else
        {
            var numbers = await CreateMockTestRecordAsync(partId, grade, db).NotWaitBack();
            if (numbers == null) { result.Fail("该类别未设置组卷规则"); }
            else
            {
                var items = numbers.Value.Item2.Select(e => new TopicTestRecordItem { Number = e, IsTested = false });
                result.Data = new TopicTestRecordModel
                {
                    CurrPostion = 0,
                    Items = items.ToArray(),
                    TestId = numbers.Value.Item1,
                    RemainTime = numbers.Value.Item3,
                };
                result.Success();
            }
        }

        return result;
    }

    /// <summary>
    /// 用户答题
    /// </summary>
    /// <param name="param">答题参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Answer(MockTestAnswerParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();
        var entity = await db.MockTestRecords.FindAsync(param.TestId);
        if (entity == null) { result.Fail("您答题的试卷不存在"); }
        else
        {
            List<string> answers = entity.AnswerRecord.ToModel<List<string>>() ?? new List<string>();
            List<string> numbers = entity.TopicInfos.ToModel<List<string>>() ?? new List<string>();
            List<int> answerIndexs = entity.AnsweredIndexs.ToModel<List<int>>() ?? new List<int>();

            int index = numbers.IndexOf(param.Number);
            entity.CurrPostion = index;

            answerIndexs.Add(index);
            answerIndexs.Sort();
            entity.AnsweredIndexs = answerIndexs.ToJson();

            for (int i = answers.Count; i <= index; i++)
            {
                answers.Add("");
            }
            answers[index] = string.Join("_", param.Answer);
            entity.AnswerRecord = answers.ToJson();

            _ = await db.SaveChangesAsync().NotWaitBack();

            result.Success();
        }

        return result;
    }

    /// <summary>
    /// 更新剩余答题时间
    /// </summary>
    /// <param name="testId">模拟测评ID</param>
    /// <param name="remainTime">剩余答题时间</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult> UpdateRemainTime(long testId, int remainTime)
    {
        RequestResult result = new();

        if (remainTime <= 0) { result.Fail("剩余时间不合法"); }
        else
        {
            int count = await entityService.UpdateAsync(e => e.Id == testId,
                e => new MockTestRecord { RemainTime = remainTime }).NotWaitBack();
            if (count > 0) { result.Success("更新成功"); }
            else { result.Fail("更新失败"); }
        }

        return result;
    }

    /// <summary>
    /// 提交当前模拟测评
    /// </summary>
    /// <param name="testId">测评记录Id</param>
    /// <returns>Data 为 成绩</returns>
    [HttpGet]
    public async Task<RequestResult<TopicMockResult>> SubmitTest(long testId)
    {
        RequestResult<TopicMockResult> result = new();

        using AssessDBContext db = entityService.GetDbContext();
        var entity = await db.MockTestRecords.FindAsync(testId);
        if (entity == null) { result.Fail("您提交的试卷不存在"); }
        else
        {
            List<string> answers = entity.AnswerRecord.ToModel<List<string>>() ?? new List<string>();
            List<string> numbers = entity.TopicInfos.ToModel<List<string>>() ?? new List<string>();

            decimal currScore = 0;
            List<int> rights = new();
            List<int> errors = new();
            decimal itemScore = entity.TotalScore / numbers.Count;
            for (int i = 0; i < answers.Count; i++)
            {
                if (answers[i].IsEmpty()) { continue; }
                var topic = await db.TopicInfos.AsNoTracking().FirstOrDefaultAsync(e => e.Number == numbers[i]).NotWaitBack();
                if (topic != null)
                {
                    string[] _answers = answers[i].Split('_');
                    var checkResult = topicService.CheckAnswer(topic, _answers);
                    if (checkResult == null) { currScore += itemScore; rights.Add(i); }
                    else { errors.Add(i); }
                }
            }

            entity.ErrorIndexs = errors.ToJson();
            entity.OverTime = DateTime.Now;
            entity.CurrScore = currScore;
            entity.RemainTime = 0;
            entity.IsOver = true;

            _ = await db.SaveChangesAsync().NotWaitBack();

            result.Data = new TopicMockResult
            {
                Score = currScore,
                RightCount = rights.Count,
                TotalScore = entity.TotalScore,
                ErrorCount = numbers.Count - rights.Count,
            };
            result.Success();
        }

        return result;
    }

    /// <summary>
    /// 获取模拟测评成绩Top榜单
    /// </summary>
    /// <param name="top">前几强</param>
    /// <param name="partId">类别ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<MockTestRankInfo[]>> ScoreRank(uint top = 10, int? partId = null)
    {
        RequestResult<MockTestRankInfo[]> result = new();

        result.Data = await entityService.GetScoreRankAsync(top, partId, (uint?)CurrentMemberId).NotWaitBack();
        foreach (var item in result.Data)
        {
            item.Avatar = WebFileTool.GetAvatarUrl(item.Avatar);
        }
        result.Success();

        return result;
    }

    /// <summary>
    /// 用户查询历史模拟测评成绩
    /// </summary>
    /// <param name="page">当前页数</param>
    /// <param name="take">每页数据数</param>
    /// <param name="partId">类别ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<MockTestRecordModel[]>> ScoreList(int page, int take, int? partId = null)
    {
        uint memberId = (uint)CurrentMemberId!;
        RequestResult<MockTestRecordModel[]> result = new();

        using AssessDBContext db = entityService.GetDbContext();
        List<MockTestRecordModel> list = await db.MockTestRecords
            .Where(e => e.MemberId == memberId && e.IsOver && (partId == null || e.PartId == partId))
            .Select(e => new MockTestRecordModel
            {
                Id = e.Id,
                Grade = e.Grade,
                OverTime = e.OverTime,
                CurrScore = e.CurrScore,
                PartName = e.TopicPart!.Name,
                CreationTime = e.CreationTime,
                AvgDifficulty = e.AvgDifficulty,
            })
            .OrderByDescending(e => e.CreationTime)
            .Skip(take * (page - 1)).Take(take)
            .ToListAsync().NotWaitBack();

        result.Data = list.ToArray();
        result.Success();

        return result;
    }

    /// <summary>
    /// 创建模拟测评试卷
    /// </summary>
    /// <param name="partId">类别ID</param>
    /// <param name="grade">年级</param>
    /// <param name="db">数据库上下文</param>
    /// <returns></returns>
    private async Task<(long, string[], int?)?> CreateMockTestRecordAsync(int partId, string? grade, AssessDBContext db)
    {
        var plan = await db.TestPlans.FirstOrDefaultAsync(e => e.PartId == partId).NotWaitBack();
        if (plan == null) { return null; }

        List<TopicInfo> list = await db.TopicInfos
            .Where(e => e.IsDelete == false && e.PartId == partId && e.Type == TopicType.单选 &&
                   e.UseType != TopicUseType.评测 && (string.IsNullOrEmpty(grade) || e.RcmdGrade!.Contains(grade)))
            .Select(e => new TopicInfo { Number = e.Number, Level = e.Level })
            .ToListAsync().NotWaitBack();

        decimal totalLevel = 0;
        List<string> numbers = new();
        int number = plan.TopicNumber;
        Random random = new(DateTime.Now.Millisecond);
        while (list.Count > 0 && number > 0)
        {
            int index = random.Next(0, list.Count);
            totalLevel += list[index].Level;
            numbers.Add(list[index].Number);
            list.RemoveAt(index);
            --number;
        }

        MockTestRecord record = new()
        {
            CurrScore = 0,
            Grade = grade,
            IsOver = false,
            CurrPostion = 0,
            PartId = partId,
            ErrorIndexs = "[]",
            AnswerRecord = "[]",
            AnsweredIndexs = "[]",
            RemainTime = plan.TestTime,
            CreationTime = DateTime.Now,
            TotalScore = plan.TotalScore,
            TopicInfos = numbers.ToJson(),
            MemberId = (uint)CurrentMemberId!,
            Number = CommonTool.CreateNumber("MT"),
            AvgDifficulty = Math.Round(totalLevel / numbers.Count, 1),
        };
        _ = db.MockTestRecords.Add(record);

        _ = await db.SaveChangesAsync().NotWaitBack();

        return new(record.Id, numbers.ToArray(), record.RemainTime);
    }
}
