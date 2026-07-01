using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Repository;

namespace Assess.Model.Param;

/// <summary>
/// 视频观看记录分页参数模型
/// </summary>
public class VideoRecordPageParam : DefaultEntityPageParam
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [DisplayName("用户ID")]
    public uint? MemberId { get; set; }

    /// <summary>
    /// 课程ID
    /// </summary>
    [DisplayName("课程ID")]
    public int? CourseId { get; set; }

    /// <summary>
    /// 视频ID
    /// </summary>
    [DisplayName("视频ID")]
    public int? VideoId { get; set; }

    /// <summary>
    /// 完成状态 (0:未看完 1:已看完)
    /// </summary>
    [DisplayName("完成状态")]
    [Range(0, 1, ErrorMessage = "状态值无效")]
    public VideoCompleteType? IsCompleted { get; set; }
}
/// <summary>
/// 视频观看记录查询参数模型
/// </summary>
public class VideoRecordParam : DefaultEntityPageParam
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [DisplayName("用户ID")]
    public uint? MemberId { get; set; }

    /// <summary>
    /// 课程ID
    /// </summary>
    [DisplayName("课程ID")]
    public int? CourseId { get; set; }

    /// <summary>
    /// 视频ID
    /// </summary>
    [DisplayName("视频ID")]
    public int? VideoId { get; set; }
}

/// <summary>
/// 添加视频观看记录参数模型
/// </summary>
public class VideoRecordInsertParam
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [DisplayName("用户ID")]
    public uint MemberId { get; set; }

    /// <summary>
    /// 课程ID
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [DisplayName("课程ID")]
    public int CourseId { get; set; }

    /// <summary>
    /// 视频ID
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [DisplayName("视频ID")]
    public int VideoId { get; set; }

    /// <summary>
    /// 最后观看时间点（单位：秒）
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "{0}不能为负数")]
    [DisplayName("观看进度")]
    public double LastPlayPosition { get; set; } = 0;

    /// <summary>
    /// 是否已看完（0:未看完 1:已看完）
    /// </summary>
    [DisplayName("完成状态")]
    public VideoCompleteType IsCompleted { get; set; } = VideoCompleteType.Uncompleted;
}

/// <summary>
/// 更新视频观看记录参数模型
/// </summary>
public class VideoRecordUpdateParam
{
    /// <summary>
    /// 记录ID
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [DisplayName("记录ID")]
    public long Id { get; set; }

    /// <summary>
    /// 最后观看时间点（单位：秒）
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "{0}不能为负数")]
    [DisplayName("观看进度")]
    public double LastPlayPosition { get; set; }

    /// <summary>
    /// 是否已看完（0:未看完 1:已看完）
    /// </summary>
    [DisplayName("完成状态")]
    public VideoCompleteType IsCompleted { get; set; }
}

/// <summary>
/// 视频观看记录参数模型
/// </summary>
public class VideoWatchRecordItem
{
    /// <summary>
    /// 最后播放位置（秒）
    /// </summary>
    public double LastPlayPosition { get; set; }

    /// <summary>
    /// 是否已看完
    /// </summary>
    public VideoCompleteType IsCompleted { get; set; }
}