using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Infrastructure.Extensions;
using Repository;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 订单管理相关接口
/// </summary>
public class CourseOrderController : AdminApiController<CourseOrder, ICourseOrderService>
{
    public CourseOrderController(ICourseOrderService entityService) : base(entityService)
    {
    }

    #region 分页查询 

    /// <summary>
    /// 获取订单分页列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<CourseOrderItem>> Page([FromBody] CourseOrderPageParam param)
    {
        RequestPageResult<CourseOrderItem> result = new();

        EntityPageModel<CourseOrder> entityParam = new(param)
        {
            Where = e =>
                (param.MemberId == null || e.MemberId == param.MemberId) &&
                (param.CourseId == null || e.CourseId == param.CourseId) &&
                (param.PaymentStatus == null || e.PaymentStatus == param.PaymentStatus) &&
                (string.IsNullOrEmpty(param.Keyword) ||
                 e.OrderNo.Contains(param.Keyword) ||
                 e.Member!.RealName!.Contains(param.Keyword))
        };

        var page = await entityService.GetPageAsync(e => new CourseOrderItem
        {
            Id = e.Id,
            OrderNo = e.OrderNo,
            MemberId = e.MemberId,
            MemberName = e.Member!.RealName,
            CourseId = e.CourseId,
            CourseTitle = e.Course!.Title,
            Amount = e.Amount,
            PaidAmount = e.PaidAmount,
            PaymentStatus = e.PaymentStatus,
            PaymentTime = e.PaymentTime,
            PaymentMethod = e.PaymentMethod,
            CreateTime = e.CreateTime
        }, entityParam).NotWaitBack();

        if (page?.Total > 0) { result.LoadPage(page); }
        else { result.Success(); }

        return result;
    }

    #endregion

    #region 订单详情 

    /// <summary>
    /// 根据ID或者订单编号OrderNo获取订单详情
    /// </summary>
    /// <param name="param">查询订单详情参数</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<CourseOrderDetail>> GetDetail([FromBody] CourseOrderDetailParam param)
    {
        RequestResult<CourseOrderDetail> result = new();
        using var db = entityService.GetDbContext();

        // 显式加载关联数据（使用Include）
        var courseOrder = await db.CourseOrders
            .Include(o => o.Course)    // 加载课程
            .Include(o => o.Member)    // 加载用户
            .FirstOrDefaultAsync(o =>
                o.Id == param.Id ||
                o.OrderNo == param.OrderNo);

        if (courseOrder == null)
        {
            result.Fail("订单不存在");
        }
        else
        {
            // 将查询结果映射到 CourseOrderDetail 对象
            var orderDetail = new CourseOrderDetail
            {
                Id = courseOrder.Id,
                OrderNo = courseOrder.OrderNo,
                MemberId = courseOrder.MemberId,
                CourseId = courseOrder.CourseId,
                Amount = courseOrder.Amount,
                PaymentMethod = courseOrder.PaymentMethod,
                CreateTime = courseOrder.CreateTime,
                PaymentStatus = courseOrder.PaymentStatus,

                CourseCoverImage = courseOrder.Course!.CoverImageUrl,
                CourseDescription = courseOrder.Course.Description,
                CourseTitle = courseOrder.Course.Title,
                MemberAvatar = courseOrder.Member!.Avatar,

            };

            result.Data = orderDetail; // 设置返回的数据
            result.Success("查询成功");
        }
    
        return result;
    }

    #endregion

}