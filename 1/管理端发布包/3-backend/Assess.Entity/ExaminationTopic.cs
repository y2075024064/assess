using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

/// <summary>
/// 考试试题信息
/// </summary>
[Table("ta_examination_topic")]
public class ExaminationTopic
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// 考试ID
    /// </summary>
    public int ExaminationId { get; set; }

    /// <summary>
    /// 题目ID
    /// </summary>
    public uint TopicId { get; set; }

    /// <summary>
    /// 分数
    /// </summary>
    public int Score { get; set; }

    [ForeignKey(nameof(TopicId))]
    public virtual TopicInfo? Topic { get; set; }

    [ForeignKey(nameof(ExaminationId))]
    public virtual Examination? Examination { get; set; }
}
