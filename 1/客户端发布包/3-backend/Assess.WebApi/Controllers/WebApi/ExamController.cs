using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Entity;
using Mapster;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 比赛或考试相关接口
/// </summary>
public class ExamController : WebApiController<Examination, IExaminationService>
{
    public ExamController(IExaminationService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 获取近期比赛或考试列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<ExaminationListItem[]>> Latest()
    {
        RequestResult<ExaminationListItem[]> result = new();

        DateTime now = DateTime.Now;
        DateOnly todey = new(now.Year, now.Month, now.Day);
        DateOnly last = todey.AddDays(30);

        var list = await entityService.GetListAsync(e => e.IsPublic == true &&
            (e.StartDate <= last || e.EndDate >= todey),
            e => new ExaminationListItem
            {
                Id = e.Id,
                Name = e.Name,
                Number = e.Number,
                EndDate = e.EndDate,
                StartDate = e.StartDate,
                Organizer = e.Organizer,
            }).NotWaitBack();

        result.Data = list.ToArray();
        result.Success();

        return result;
    }

    /// <summary>
    /// 获取比赛或考试详情信息
    /// </summary>
    /// <param name="id">比赛或考试ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<ExaminationDetail>> Detail(int id)
    {
        RequestResult<ExaminationDetail> result = new();

        var entity = await entityService.GetEntityAsync(id).NotWaitBack();
        if (entity == null) { result.Fail("您要获取的信息不存在"); }
        else
        {
            result.Data = entity.Adapt<ExaminationDetail>();
            result.Success();
        }

        return result;
    }

    /// <summary>
    /// 用户报名该比赛或考试
    /// </summary>
    /// <param name="examId">比赛或考试ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult> Apply(int examId)
    {
        RequestResult result = new();
        uint memberId = CurrentMemberId!.Value;

        long count = await entityService.Adapter.GetCountAsync<ExamApplyInfo>(
            e => e.ExaminationId == examId && e.MemberId == memberId).NotWaitBack();

        if (count > 0) { result.Fail("您已报名该项目"); }
        else
        {
            ExamApplyInfo applyInfo = new()
            {
                IsPay = false,
                MemberId = memberId,
                ExaminationId = examId,
                ApplyTime = DateTime.Now,
            };
            bool isSucc = await entityService.Adapter.InsertAsync(applyInfo).NotWaitBack();
            if (isSucc) { result.Success("报名成功"); }
            else { result.Fail("报名失败"); }
        }

        return result;
    }
}
