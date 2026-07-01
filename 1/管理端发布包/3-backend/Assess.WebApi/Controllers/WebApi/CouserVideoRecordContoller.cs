using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Assess.Service.Services;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 视频观看记录管理相关接口(无)
/// </summary>
public class CourseVideoRecordController : WebApiController<CouserVideoRecord, ICouserVideoRecordService>
{
    public CourseVideoRecordController(ICouserVideoRecordService entityService) : base(entityService)
    {
    }

}