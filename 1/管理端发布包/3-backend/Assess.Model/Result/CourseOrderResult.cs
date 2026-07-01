namespace Assess.Model.Result;

/// <summary>
/// 订单分页项
/// </summary>
public class CourseOrderItem
{
    /// <summary>
    /// 订单ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 订单编号
    /// </summary>
    public string OrderNo { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    public string MemberName { get; set; }

    /// <summary>
    /// 课程ID
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// 课程标题
    /// </summary>
    public string CourseTitle { get; set; }

    /// <summary>
    /// 订单金额（元）
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 实际支付金额（元）
    /// </summary>
    public decimal? PaidAmount { get; set; }

    /// <summary>
    /// 支付状态（0：未支付，1：已支付，2：已退款）
    /// </summary>
    public PaymentStatusType PaymentStatus { get; set; }

    /// <summary>
    /// 支付时间
    /// </summary>
    public DateTime? PaymentTime { get; set; }

    /// <summary>
    /// 支付方式（如：wechat, alipay）
    /// </summary>
    public PaymentMethodType? PaymentMethod { get; set; }

    /// <summary>
    /// 订单创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}

/// <summary>
/// 订单详情
/// </summary>
public class CourseOrderDetail : CourseOrderItem
{
    /// <summary>
    /// 课程封面图URL
    /// </summary>
    public string? CourseCoverImage { get; set; }

    /// <summary>
    /// 课程描述
    /// </summary>
    public string? CourseDescription { get; set; }

    /// <summary>
    /// 用户头像URL
    /// </summary>
    public string? MemberAvatar { get; set; }
}