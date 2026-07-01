using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assess.Entity;

/// <summary>
/// 年级信息表
/// </summary>
[Table("ta_grade_info")]
[Index(nameof(Name), Name = "UQ_GradeInfo_Name", IsUnique = true)]
public class GradeInfo
{
    /// <summary>
    /// 年级ID
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [StringLength(10)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 排序级别
    /// </summary>
    public int SortLevel { get; set; }

    /// <summary>
    /// 是否使用
    /// </summary>
    public bool IsUse { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool IsShow { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Remark { get; set; }
}
