using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

/// <summary>
/// 学生信息表
/// </summary>
[Table("ta_student_info")]
public class StudentInfo
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [Key]
    public uint MemberId { get; set; }

    /// <summary>
    /// 学校ID
    /// </summary>
    public int SchoolId { get; set; }

    /// <summary>
    /// 学级
    /// </summary>
    public int GradeNum { get; set; }

    /// <summary>
    /// 班级
    /// </summary>
    public int ClassNum { get; set; }

    /// <summary>
    /// 当前年级
    /// </summary>
    [StringLength(10)]
    public string CurrentGrade { get; set; } = string.Empty;

    /// <summary>
    /// 注册年级
    /// </summary>
    [StringLength(10)]
    public string RegistGrade { get; set; } = string.Empty;

    [ForeignKey(nameof(MemberId))]
    public virtual Member? Member { get; set; }

    [ForeignKey(nameof(SchoolId))]
    public virtual SchoolInfo? School { get; set; }
}
