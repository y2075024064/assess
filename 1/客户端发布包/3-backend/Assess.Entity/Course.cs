using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;


/// <summary>
/// 课程表
/// </summary>
[Table("ta_course")]
public class Course
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// 课程名称
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 课程描述
    /// </summary>
    [StringLength(2000)]
    public string? Description { get; set; }

    /// <summary>
    /// 封面图片URL
    /// </summary>
    [StringLength(255)]
    public string? CoverImageUrl { get; set; }

    /// <summary>
    /// 课程价格
    /// </summary>
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    /// <summary>
    /// 折扣价格
    /// </summary>
    [Column(TypeName = "decimal(10, 2)")]
    public decimal? DiscountPrice { get; set; }

    /// <summary>
    /// 课程分类ID
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// 讲师ID
    /// </summary>
    public int TeacherId { get; set; }

    /// <summary>
    /// 课程状态 (0:未发布, 1:已发布, 2:已下架)
    /// </summary>
    public int Status { get; set; } = 0;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; } = DateTime.Now;

    [ForeignKey(nameof(CategoryId))]
    public virtual CourseCategory? Category { get; set; }


    /// <summary>
    ///  课程视频集合
    /// </summary>
    public virtual ICollection<CourseVideo> Videos { get; set; } = new List<CourseVideo>();

    /// <summary>
    /// 课程订单集合 
    /// </summary>
    public virtual ICollection<CourseOrder> Orders { get; set; } = new List<CourseOrder>();
}