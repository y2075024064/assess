using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

/// <summary>
/// 练习记录表
/// </summary>
[Table("ta_practice_record")]
public class PracticeRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ulong Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 题目ID
    /// </summary>
    public uint TopicId { get; set; }

    /// <summary>
    /// 练习成绩
    /// </summary>
    public decimal Score { get; set; }

    /// <summary>
    /// 练习时间
    /// </summary>
    [Column(TypeName = "timestamp")]
    public DateTime PracticeTime { get; set; }

    /// <summary>
    /// 用户填写的答案
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Answer { get; set; }

    /// <summary>
    /// 答案是否正确
    /// </summary>
    public bool IsTrue { get; set; }

    [ForeignKey(nameof(TopicId))]
    public virtual TopicInfo? TopicInfo { get; set; }

    [ForeignKey(nameof(MemberId))]
    public virtual Member? Member { get; set; }
}
