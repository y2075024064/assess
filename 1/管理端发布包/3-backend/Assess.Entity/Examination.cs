using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

[Table("ta_examination")]
public class Examination
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// 考试编号
    /// </summary>
    [Column(TypeName = "char(12)")]
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// 考试名称
    /// </summary>
    [MaxLength(64)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 考试目的
    /// </summary>
    [MaxLength(128)]
    public string? Purpose { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 考试开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 考试时长
    /// </summary>
    public uint Duration { get; set; }

    /// <summary>
    /// 参考对象
    /// </summary>
    [MaxLength(128)]
    public string? ReferenceObj { get; set; }

    /// <summary>
    /// 考试开始日期
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// 考试结束日期
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// 是否开放考试
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// 主办方
    /// </summary>
    [MaxLength(128)]
    public string? Organizer { get; set; }

    /// <summary>
    /// 协办方
    /// </summary>
    [MaxLength(128)]
    public string? CoOrganizer { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Remark { get; set; }

    /// <summary>
    /// 考试试题集合
    /// </summary>
    public List<ExaminationTopic> Topics { get; set; } = new List<ExaminationTopic>();

    /// <summary>
    /// 报名信息集合
    /// </summary>
    public List<ExamApplyInfo> ApplyInfos { get; set; } = new List<ExamApplyInfo>();
}
