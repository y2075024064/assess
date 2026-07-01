using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;


/// <summary>
/// 课程分类表
/// </summary>
[Table("ta_course_category")]
public class CourseCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// 分类名称
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 分类描述
    /// </summary>
    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// 父分类ID
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// 排序权重
    /// </summary>
    public int SortWeight { get; set; } = 0;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; } = DateTime.Now;

    [ForeignKey(nameof(ParentId))]
    public virtual CourseCategory? ParentCategory { get; set; }

    /// <summary>
    /// 课程集合 
    /// </summary>
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
