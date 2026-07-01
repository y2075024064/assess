using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Repository;

namespace Assess.Model.Param;

/// <summary>
/// 课程视频分页参数
/// </summary>
public class CourseVideoPageParam : DefaultEntityPageParam
{
    /// <summary>
    /// 课程ID
    /// </summary>
    [DisplayName("课程ID")]
    public int? CourseId { get; set; }

    /// <summary>
    /// 是否免费
    /// </summary>
    [DisplayName("是否免费")]
    public bool? IsFree { get; set; }
}

/// <summary>
/// 添加视频参数
/// </summary>
public class CourseVideoInsertParam
{
    /// <summary>
    /// 课程ID
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [DisplayName("课程ID")]
    public int CourseId { get; set; }

    /// <summary>
    /// 视频标题
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [StringLength(100, ErrorMessage = "{0}最长100字符")]
    [DisplayName("视频标题")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 视频URL
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [StringLength(255, ErrorMessage = "{0}最长255字符")]
    [DisplayName("视频URL")]
    public string VideoUrl { get; set; } = string.Empty;

    /// <summary>
    /// 视频时长(秒)
    /// </summary>
    [DisplayName("视频时长")]
    [Range(0, int.MaxValue, ErrorMessage = "{0}必须大于0")]
    public int Duration { get; set; }

    /// <summary>
    /// 排序序号
    /// </summary>
    [DisplayName("排序序号")]
    public int SortOrder { get; set; }

    /// <summary>
    /// 是否免费
    /// </summary>
    [DisplayName("是否免费")]
    public bool IsFree { get; set; } = false;
}

/// <summary>
/// 修改视频参数
/// </summary>
public class CourseVideoUpdateParam : CourseVideoInsertParam
{
    /// <summary>
    /// 视频ID
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [DisplayName("视频ID")]
    public int Id { get; set; }
}