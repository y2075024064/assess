using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Repository;

namespace Assess.Model.Param;

/// <summary>
/// 学生分页参数信息模型
/// </summary>
public class StudentPageParam : DefaultEntityPageParam
{
    /// <summary>
    /// 学校ID
    /// </summary>
    public int? SchoolId { get; set; }

    /// <summary>
    /// 当前年级
    /// </summary>
    public string? Grade { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public GenderType? Gender { get; set; }
}

/// <summary>
/// 添加学生信息模型
/// </summary>
public class StudentInsertParam
{
    /// <summary>
    /// 学生姓名
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DisplayName("姓名")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 学校ID
    /// </summary>
    [Required]
    [DisplayName("学校ID")]
    public int SchoolId { get; set; }

    /// <summary>
    /// 学生学级
    /// </summary>
    [Required]
    [DisplayName("学级")]
    public int GradeNum { get; set; }

    /// <summary>
    /// 学生班级
    /// </summary>
    [Required]
    [DisplayName("班级")]
    public int ClassNum { get; set; }

    /// <summary>
    /// 班级序号
    /// </summary>
    [Required]
    [DisplayName("序号")]
    public int No { get; set; }

    /// <summary>
    /// 当前年级
    /// </summary>
    [Required]
    [StringLength(10)]
    [DisplayName("年级")]
    public string CurrentGrade { get; set; } = string.Empty;

    /// <summary>
    /// 性别
    /// </summary>
    [Required]
    [DisplayName("性别")]
    public GenderType Gender { get; set; }
}

/// <summary>
/// 更改学生信息模型
/// </summary>
public class StudentUpdateParam : StudentInsertParam
{
    /// <summary>
    /// 学生ID
    /// </summary>
    [Required]
    public uint Id { get; set; }
}
