namespace Assess.Model.Result;

/// <summary>
/// 课程视频项
/// </summary>
public class CourseVideoItem
{
    /// <summary>
    /// 视频ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 课程ID
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// 视频标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 视频URL
    /// </summary>
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
    public bool IsFree { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 格式化后的时长(HH:mm:ss)
    /// </summary>
    public string FormattedDuration =>
        TimeSpan.FromSeconds(Duration).ToString(@"hh\:mm\:ss");
}