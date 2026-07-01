using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;


/// <summary>
/// 课程视频表
/// </summary>
[Table("ta_course_video")]
public class CourseVideo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// 所属课程ID
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// 视频标题
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 视频URL
    /// </summary>
    [Required]
    [StringLength(255)]
    public string VideoUrl { get; set; } = string.Empty;

    /// <summary>
    /// 视频时长(秒)
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// 排序序号
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// 是否免费
    /// </summary>
    public bool IsFree { get; set; } = false;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; } = DateTime.Now;

    [ForeignKey(nameof(CourseId))]
    public virtual Course? Course { get; set; }
}
