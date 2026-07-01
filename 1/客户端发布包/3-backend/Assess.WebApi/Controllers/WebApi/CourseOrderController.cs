using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Infrastructure.Extensions;
using Repository;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 订单管理相关接口
/// </summary>
public class CourseOrderController : WebApiController<CourseOrder, ICourseOrderService>
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

    #region 创建订单 

    /// <summary>
    /// 创建新订单(还未支付)
    /// </summary>
    /// <param name="param">订单参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult<CourseOrderDetail>> Create([FromBody] CourseOrderCreateParam param)
    {
        RequestResult<CourseOrderDetail> result = new();

        using var db = entityService.GetDbContext();

        // 获取课程价格
        var course = await db.Courses.FindAsync(param.CourseId);
        if (course == null)
        {
            result.Fail("课程不存在");
            return result;
        }
            
        var order = new CourseOrder
        {
            OrderNo = GenerateOrderNo(),
            MemberId = param.MemberId,
            CourseId = param.CourseId,
            Amount = course.DiscountPrice ?? course.Price,
            CreateTime = DateTime.Now
        };

        _ = db.CourseOrders.Add(order);
        int count = await db.SaveChangesAsync().NotWaitBack();

        if (count > 0) {
            // 应该返回订单的信息
            result.Data = new CourseOrderDetail
            {
                Id = order.Id,
                OrderNo = order.OrderNo,
                MemberId = order.MemberId,
                CourseId = order.CourseId,
                CourseTitle = course.Title,
                CourseCoverImage = course.CoverImageUrl, // 封面图URL
                CourseDescription = course.Description,
                Amount = order.Amount,
                PaymentStatus = order.PaymentStatus, // 默认是没有支付
                CreateTime = order.CreateTime,
            };
            result.Success("订单创建成功");
        }
        else {
            result.Fail("订单创建失败");
        }

        return result;
    }

    #endregion  

    #region 更新支付状态

    /// <summary>
    /// 更新订单支付状态(支付和退款接口,暂时这么写着)
    /// </summary>
    /// <param name="param">支付参数</param>
    /// <returns></returns>
    [HttpPut]
    public async Task<RequestResult> UpdatePayment([FromBody] CourseOrderUpdateParam param)
    {
        RequestResult result = new();

        using var db = entityService.GetDbContext();

        var order = await db.CourseOrders.FindAsync(param.Id);
        if (order == null)
        {
            result.Fail("订单不存在");
            return result;
        }
            

        // 验证状态转换
        if (order.PaymentStatus == PaymentStatusType.Paid && param.PaymentStatus == PaymentStatusType.Unpaid)
        {
            result.Fail("已支付订单不能回退到未支付状态");
            return result;
        }
            
        order.PaymentStatus = param.PaymentStatus;
        order.PaidAmount = param.PaidAmount;
        order.PaymentMethod = param.PaymentMethod;
        order.PaymentTime = param.PaymentStatus == PaymentStatusType.Paid ? DateTime.Now : order.PaymentTime;

        int count = await db.SaveChangesAsync().NotWaitBack();
        if (count > 0) { result.Success("支付状态更新成功"); }
        else { result.Fail("支付状态更新失败"); }

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


    #region 生成唯一的订单编号
    /// <summary>
    /// 生成唯一的订单编号（基于 Unix 时间戳 + 随机数）
    /// </summary>
    /// <returns>格式: 秒级时间戳 + 4位随机数（如 17119872051234）</returns>
    private static string GenerateOrderNo()
    {
        // 获取当前 Unix 时间戳（秒）
        long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // 使用线程安全的方式生成4位随机数
        var randomBytes = new byte[2]; // 2字节足够生成0-65535之间的随机数
        System.Security.Cryptography.RandomNumberGenerator.Fill(randomBytes);
        int randomNumber = BitConverter.ToUInt16(randomBytes, 0) % 9000 + 1000; // 确保范围在 [1000, 9999]

        // 组合成订单号
        return $"{unixTimestamp}{randomNumber}";
    }
    #endregion
}