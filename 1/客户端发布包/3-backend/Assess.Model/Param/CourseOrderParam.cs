using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Repository;

namespace Assess.Model.Param;

/// <summary>
/// 订单分页参数
/// </summary>
public class CourseOrderPageParam : DefaultEntityPageParam
{
    /// <summary>
    /// 用户ID筛选
    /// </summary>
    [DisplayName("用户ID")]
    public uint? MemberId { get; set; }

    /// <summary>
    /// 课程ID筛选
    /// </summary>
    [DisplayName("课程ID")]
    public int? CourseId { get; set; }

    /// <summary>
    /// 支付状态筛选 (0:未支付, 1:已支付, 2:已退款)
    /// </summary>
    [DisplayName("支付状态")]
    [Range(0, 2, ErrorMessage = "状态值无效")]
    public PaymentStatusType? PaymentStatus { get; set; }
}

/// <summary>
/// 创建订单参数 这里不需要支付方式
/// </summary>
public class CourseOrderCreateParam
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [DisplayName("用户ID")]
    public uint MemberId { get; set; }

    /// <summary>
    /// 课程ID
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [DisplayName("课程ID")]
    public int CourseId { get; set; }
}

/// <summary>
/// 更新订单支付状态参数
/// </summary>
public class CourseOrderUpdateParam
{
    /// <summary>
    /// 订单ID
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [DisplayName("订单ID")]
    public long Id { get; set; }

    /// <summary>
    /// 支付状态 (1:已支付, 2:已退款)
    /// </summary>
    [DisplayName("支付状态")]
    public PaymentStatusType PaymentStatus { get; set; }

    /// <summary>
    /// 实际支付金额
    /// </summary>
    [Range(0, 999999.99, ErrorMessage = "{0}范围0-999999.99")]
    [DisplayName("实付金额")]
    public decimal? PaidAmount { get; set; }

    /// <summary>
    /// 支付方式
    /// </summary>
    [DisplayName("支付方式")]
    public PaymentMethodType? PaymentMethod { get; set; }
}

/// <summary>
/// 查询订单详情参数
/// </summary>
public class CourseOrderDetailParam
{
    /// <summary>
    /// 订单ID
    /// </summary>
    [DisplayName("订单ID")]
    public long Id { get; set; }

    /// <summary>
    /// 订单编号
    /// </summary>
    [StringLength(50)]
    public string OrderNo { get; set; } = string.Empty;
}