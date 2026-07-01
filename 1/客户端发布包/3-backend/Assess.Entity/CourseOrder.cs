
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;


/// <summary>
/// 订单表
/// </summary>
[Table("ta_course_order")]
public class CourseOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// 订单编号
    /// </summary>
    [Required]
    [StringLength(50)]
    public string OrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 课程ID
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// 订单金额
    /// </summary>
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }

    /// <summary>
    /// 实际支付金额
    /// </summary>
    [Column(TypeName = "decimal(10, 2)")]
    public decimal? PaidAmount { get; set; }

    /// <summary>
    /// 支付状态 (0:未支付, 1:已支付, 2:已退款)
    /// </summary>
    public PaymentStatusType PaymentStatus { get; set; } = PaymentStatusType.Unpaid;

    /// <summary>
    /// 支付时间
    /// </summary>
    public DateTime? PaymentTime { get; set; }

    /// <summary>
    /// 支付方式
    /// </summary>
    public PaymentMethodType? PaymentMethod { get; set; }

    /// <summary>
    /// 订单创建时间
    /// </summary>
    public DateTime CreateTime { get; set; } = DateTime.Now;

    [ForeignKey(nameof(MemberId))]
    public virtual Member? Member { get; set; }

    [ForeignKey(nameof(CourseId))]
    public virtual Course? Course { get; set; }
}