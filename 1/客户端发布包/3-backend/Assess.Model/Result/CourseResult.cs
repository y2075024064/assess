
using Assess.Model.Param;

namespace Assess.Model.Result;


/// <summary>
/// 课程分页项
/// </summary>
public class CoursePageItem
{
    /// <summary>
    /// 课程ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 课程标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 课程价格
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 折扣后的价格（如果有）
    /// </summary>
    public decimal? DiscountPrice { get; set; }

    /// <summary>
    /// 课程状态
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 分类ID
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// 分类名称
    /// </summary>
    public string? CategoryName { get; set; }

    /// <summary>
    /// 教师ID
    /// </summary>
    public int TeacherId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 封面图片URL
    /// </summary>
    public string? CoverImageUrl { get; set; }
}

/// <summary>
/// 课程详情
/// </summary>
public class CourseVideoDetail
{
    /// <summary>
    /// 视频基本信息
    /// </summary>
    public CourseVideoItem VideoInfo { get; set; }

    /// <summary>
    /// 用户观看记录 
    /// </summary>
    public VideoWatchRecordItem? WatchRecord { get; set; }
}


