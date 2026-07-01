using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

/// <summary>
/// 考试记录表
/// </summary>
[Table("ta_examine_record")]
public class ExamineRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// 考试ID
    /// </summary>
    public int ExaminationId { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 是否考试结束
    /// </summary>
    public bool IsOver { get; set; }

    /// <summary>
    /// 当前答题位置
    /// </summary>
    public int CurrPostion { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 答题记录
    /// </summary>
    [Column(TypeName = "json")]
    public string? AnswerRecord { get; set; }

    /// <summary>
    /// 错误题目索引数组
    /// </summary>
    [Column(TypeName = "json")]
    public string? ErrorIndexs { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 答题结束时间
    /// </summary>
    public DateTime? OverTime { get; set; }

    [ForeignKey(nameof(MemberId))]
    public virtual Member? Member { get; set; }

    [ForeignKey(nameof(ExaminationId))]
    public virtual Examination? Examination { get; set; }
}
