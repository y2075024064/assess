using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using System.Linq.Expressions;
using Assess.Model.Result;
using Assess.Entity;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 题目相关
/// </summary>
public class TopicController : WebApiController<TopicInfo, ITopicService>
{
    public TopicController(ITopicService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 给当前题目打标记
    /// </summary>
    /// <param name="topicId">题目ID</param>
    /// <param name="remark">备注信息</param>
    /// <returns></returns>
    [HttpGet]
    [AccreditAction(MemberType.管理员)]
    public async Task<RequestResult> MarkInfo(uint topicId, string remark)
    {
        RequestResult result = new();

        if (remark.IsEmpty()) { result.Fail("备注信息不能为空"); }
        else
        {
            int count = await entityService.UpdateAsync(e => e.Id == topicId,
                e => new TopicInfo { Remark = remark }).NotWaitBack();
            if (count > 0) { result.Success("标记成功"); }
            else { result.Fail("标记失败"); }
        }

        return result;
    }

    /// <summary>
    /// 获取练习类别选项列表
    /// </summary>
    /// <param name="grade">推荐年级</param>
    /// <param name="partId">类别ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<long>> TotalNumber(string grade, int? partId)
    {
        RequestResult<long> result = new();

        Expression<Func<TopicInfo, bool>> where = e => e.IsDelete == false && e.UseType != TopicUseType.评测;

        if (partId != null) { where = where.AndAlso(e => e.PartId == partId); }
        if (grade.IsNotEmpty()) { where = where.AndAlso(e => e.RcmdGrade!.Contains(grade)); }

        result.Data = await entityService.GetCountAsync(where).NotWaitBack();
        result.Success();

        return result;
    }

    /// <summary>
    /// 获取当前练习题目列表（支持顺序练题与专项练题）
    /// </summary>
    /// <param name="grade">选择的年级</param>
    /// <param name="partId">选择的类别</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<TopicPracticeRecord[]>> OrderList(string? grade, int? partId)
    {
        uint memberId = (uint)CurrentMemberId!;
        RequestResult<TopicPracticeRecord[]> result = new();

        //获取符合条件的所有题目
        Expression<Func<TopicInfo, bool>> where1 = e => e.IsDelete == false && e.UseType != TopicUseType.评测;
        if (partId > 0) { where1 = where1.AndAlso(e => e.PartId == partId); }
        if (grade.IsNotEmpty()) { where1 = where1.AndAlso(e => e.RcmdGrade!.Contains(grade!)); }

        List<TopicInfo> list = await entityService.GetListAsync(where1, e => new TopicInfo { Id = e.Id, Number = e.Number }).NotWaitBack();
        if (list.Count > 0)
        {
            //获取当前练习过的记录
            Expression<Func<PracticeRecord, bool>> where2 = e => e.MemberId == memberId;
            if (partId > 0) { where2 = where2.AndAlso(e => e.TopicInfo!.PartId == partId); }
            if (grade.IsNotEmpty()) { where2 = where2.AndAlso(e => e.TopicInfo!.RcmdGrade!.Contains(grade!)); }

            List<PracticeRecord> practices = await entityService.Adapter
                .GetListAsync(where2, e => new PracticeRecord() { IsTrue = e.IsTrue, TopicId = e.TopicId }).NotWaitBack();

            var _list = list.OrderBy(e => e.Number);
            List<TopicPracticeRecord> records = new();
            foreach (TopicInfo item in _list)
            {
                var record = practices.FirstOrDefault(e => e.TopicId == item.Id);
                records.Add(new TopicPracticeRecord()
                {
                    Number = item.Number,
                    IsTrue = record?.IsTrue,
                    IsTested = record != null,
                });
            }
            result.Data = records.ToArray();
        }
        else { result.Data = Array.Empty<TopicPracticeRecord>(); }
        result.Success();

        return result;
    }

    /// <summary>
    /// 获取当前随机练习题目列表
    /// </summary>
    /// <param name="grade">选择的年级</param>
    /// <param name="partId">选择的类别</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<string[]>> RandomList(string? grade, int? partId)
    {
        uint memberId = (uint)CurrentMemberId!;
        RequestResult<string[]> result = new();

        //获取符合条件的所有题目
        Expression<Func<TopicInfo, bool>> where = e => e.IsDelete == false && e.UseType != TopicUseType.评测;
        if (partId > 0) { where = where.AndAlso(e => e.PartId == partId); }
        if (grade.IsNotEmpty()) { where = where.AndAlso(e => e.RcmdGrade!.Contains(grade!)); }

        where = where.AndAlso(e =>
            !e.PracticeRecords
            .Where(p => p.MemberId == memberId)
            .Select(p => p.TopicId)
            .Contains(e.Id)
        );

        List<TopicInfo> list = await entityService.GetListAsync(where, e => new TopicInfo { Number = e.Number }).NotWaitBack();
        if (list.Count > 0)
        {
            List<string> numbers = new();
            Random random = new(DateTime.Now.Millisecond);
            while (list.Count > 0)
            {
                int index = random.Next(0, list.Count);
                numbers.Add(list[index].Number);
                list.RemoveAt(index);
            }
            result.Data = numbers.ToArray();
        }
        else { result.Data = Array.Empty<string>(); }
        result.Success();

        return result;
    }

    /// <summary>
    /// 获取题目练习信息
    /// </summary>
    /// <param name="number">题目编号</param>
    /// <param name="isTested">是否答题过</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<TopicTestModel>> TestInfo(string number, bool? isTested = null)
    {
        RequestResult<TopicTestModel> result = new();

        using AssessDBContext db = entityService.GetDbContext();
        var entity = await db.TopicInfos.Where(e => e.Number == number).Select(e => new TopicTestModel
        {
            Id = e.Id,
            Type = e.Type,
            Level = e.Level,
            Label = e.Label,
            Number = e.Number,
            Origin = e.Origin,
            Content = e.Content,
            Analysis = e.Analysis,
            RcmdGrade = e.RcmdGrade,
            IsOriginal = e.IsOriginal,
            CommentNum = e.CommentNum,
            RightAnswer = e.RightAnswer,
            OriginNumber = e.OriginNumber,
            CollectionNum = e.CollectionNum,
            Options = e.TopicOptions!.Options,
        }).SingleOrDefaultAsync().NotWaitBack();

        if (entity != null)
        {
            uint memberId = (uint)CurrentMemberId!;
            entity.IsCollected = await db.MemberTopicCollections.CountAsync(
                e => e.MemberId == memberId && e.TopicId == entity.Id).NotWaitBack() > 0;

            if (isTested == true)
            {
                entity.Answer = await db.PracticeRecords.Where(e => e.MemberId == memberId && e.TopicId == entity.Id)
                    .Select(e => e.Answer).SingleOrDefaultAsync().NotWaitBack();
            }
            else
            {
                entity.Analysis = null;
                entity.RightAnswer = null;
            }

            result.Data = entity;
            result.Success();
        }
        else { result.Fail("您要获取的题目不存在"); }

        return result;
    }

    /// <summary>
    /// 验证题目答案是否正确
    /// </summary>
    /// <param name="topicId">题目ID</param>
    /// <param name="answer">题目答案（字符串数组，例如：["A"]、["A","B"]）</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult<AnswerResult>> Check(uint topicId, [Required][FromBody] string[] answer)
    {
        uint memberId = (uint)CurrentMemberId!;
        RequestResult<AnswerResult> result = new();

        if (answer == null || answer.Length == 0)
        {
            result.Fail("答案不合法");
            return result;
        }

        using AssessDBContext db = entityService.GetDbContext();
        var entity = await db.TopicInfos.Where(e => e.Id == topicId)
            .Select(e => new TopicInfo { Type = e.Type, RightAnswer = e.RightAnswer, Analysis = e.Analysis })
            .FirstOrDefaultAsync().NotWaitBack();

        if (entity == null) { result.Fail("您要验证的题目不存在"); }
        else
        {
            var checkResult = entityService.CheckAnswer(entity, answer);
            if (checkResult.IsNotEmpty()) { result.Fail(checkResult!); }
            else
            {
                var record = await db.PracticeRecords.FirstOrDefaultAsync(e => e.MemberId == memberId && e.TopicId == topicId);
                bool isRight = checkResult == null;
                if (record == null)
                {
                    record = new PracticeRecord()
                    {
                        Score = 1,
                        IsTrue = isRight,
                        TopicId = topicId,
                        MemberId = memberId,
                        Answer = answer.ToJson(),
                        PracticeTime = DateTime.Now,
                    };
                    _ = db.PracticeRecords.Add(record);
                }
                else
                {
                    record.IsTrue = isRight;
                    record.Answer = answer.ToJson();
                    record.PracticeTime = DateTime.Now;
                }

                var errRecord = await db.MemberErrorRecords.FirstOrDefaultAsync(
                    e => e.MemberId == memberId && e.TopicId == topicId);
                if (errRecord != null && !errRecord.IsPass)
                {
                    errRecord.IsPass = isRight;
                    if (isRight)
                    {
                        errRecord.ErrorAnswer = record.Answer;
                        errRecord.LastErrorTime = DateTime.Now;
                    }
                    else
                    {
                        errRecord.PassTime = DateTime.Now;
                    }
                }

                if (!isRight && errRecord == null)
                {
                    errRecord = new MemberErrorRecord()
                    {
                        LastErrorTime = DateTime.Now,
                        ErrorAnswer = record.Answer,
                        MemberId = memberId,
                        TopicId = topicId,
                        IsPass = false,
                    };
                    _ = db.MemberErrorRecords.Add(errRecord);
                }

                int count = await db.SaveChangesAsync().NotWaitBack();
                if (count > 0)
                {
                    result.Data = new AnswerResult
                    {
                        IsTrue = isRight,
                        Analysis = entity.Analysis,
                        RightAnswer = entity.RightAnswer
                    };
                    result.Success();
                }
                else { result.Fail("答题失败"); }
            }
        }

        return result;
    }
}
