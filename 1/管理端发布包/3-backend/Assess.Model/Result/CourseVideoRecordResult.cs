namespace Assess.Model.Result;

/// <summary>
/// 视频观看进度记录结果模型
/// </summary>
public class CouserVideoRecordItem
{
    /// <summary>
    /// 主键ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 课程ID
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// 视频ID
    /// </summary>
    public int VideoId { get; set; }

    /// <summary>
    /// 最后观看时间点（单位：秒）
    /// </summary>
    public double LastPlayPosition { get; set; }

    /// <summary>
    /// 是否已看完（0:未看完 1:已看完）
    /// </summary>
    public VideoCompleteType IsCompleted { get; set; }

    /// <summary>
    /// 最后更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 课程名称（可选扩展字段）
    /// </summary>
    public string? CourseTitle { get; set; }

    /// <summary>
    /// 视频标题（可选扩展字段）
    /// </summary>
    public string? VideoTitle { get; set; }
}