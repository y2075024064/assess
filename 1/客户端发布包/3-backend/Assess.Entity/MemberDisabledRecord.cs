using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

/// <summary>
/// 用户封禁记录表
/// </summary>
[Table("ta_member_disabled_record")]
public class MemberDisabledRecord
{
    /// <summary>
    /// 记录Id
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 账户禁用开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 账户禁用结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 禁用原因
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Reason { get; set; }

    /// <summary>
    /// 是否发起过申诉
    /// </summary>
    public bool IsAppeal { get; set; }

    /// <summary>
    /// 申诉描述
    /// </summary>
    [StringLength(400)]
    public string? AppealDesc { get; set; }

    /// <summary>
    /// 申诉图片数组
    /// </summary>
    [Column(TypeName = "json")]
    public string? AppealImgs { get; set; }

    /// <summary>
    /// 申诉结果
    /// </summary>
    public AppealResult AppealResult { get; set; }

    /// <summary>
    /// 申诉失败原因
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? FailReason { get; set; }

    /// <summary>
    /// 真正封禁结束时间
    /// </summary>
    public DateTime? RealEndTime { get; set; }

    /// <summary>
    /// 封禁错误补偿信息
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? CompensationInfo { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Remark { get; set; }

    [ForeignKey(nameof(MemberId))]
    public virtual Member? Member { get; set; }
}
