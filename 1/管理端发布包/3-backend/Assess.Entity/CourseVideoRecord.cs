using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

/// <summary>
/// 视频观看进度记录表
/// </summary>
[Table("ta_couser_video__record")]
public class CouserVideoRecord
{
    /// <summary>
    /// 主键ID
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    [Required]
    public uint MemberId { get; set; }

    /// <summary>
    /// 课程ID
    /// </summary>
    [Required]
    public int CourseId { get; set; }

    /// <summary>
    /// 视频ID
    /// </summary>
    [Required]
    public int VideoId { get; set; }

    /// <summary>
    /// 最后观看时间点（单位：秒）
    /// </summary>
    [Column(TypeName = "decimal(10, 2)")]
    public double LastPlayPosition { get; set; }

    /// <summary>
    /// 是否已看完（0:未看完 1:已看完）
    /// </summary>
    public VideoCompleteType IsCompleted { get; set; } = VideoCompleteType.Uncompleted;

    /// <summary>
    /// 最后更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; } = DateTime.Now;

    // 导航属性
    [ForeignKey(nameof(MemberId))]
    public virtual Member? Member { get; set; }

    [ForeignKey(nameof(CourseId))]
    public virtual Course? Course { get; set; }

    [ForeignKey(nameof(VideoId))]
    public virtual CourseVideo? Video { get; set; }
}

