using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

/// <summary>
/// 考试报名信息表
/// </summary>
[Table("ta_exam_apply_info")]
public class ExamApplyInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 考试ID
    /// </summary>
    public int ExaminationId { get; set; }

    /// <summary>
    /// 考试记录ID
    /// </summary>
    public long? ExamineRecordId { get; set; }

    /// <summary>
    /// 是否支付报名费
    /// </summary>
    public bool IsPay { get; set; }

    /// <summary>
    /// 报名时间
    /// </summary>
    public DateTime ApplyTime { get; set; }

    [ForeignKey(nameof(MemberId))]
    public virtual Member? Member { get; set; }

    [ForeignKey(nameof(ExaminationId))]
    public virtual Examination? Examination { get; set; }

    [ForeignKey(nameof(ExamineRecordId))]
    public virtual ExamineRecord? ExamineRecord { get; set; }
}
