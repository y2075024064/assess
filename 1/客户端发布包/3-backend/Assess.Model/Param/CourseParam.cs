using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Repository;
using Assess.Model.Result;

namespace Assess.Model.Param;

/// <summary>
/// 课程分页参数信息模型
/// </summary>
public class CoursePageParam : DefaultEntityPageParam
{
    /// <summary>
    /// 课程分类ID
    /// </summary>
    [DisplayName("分类ID")]
    public int? CategoryId { get; set; }

    /// <summary>
    /// 讲师ID
    /// </summary>
    [DisplayName("讲师ID")]
    public int? TeacherId { get; set; }

    /// <summary>
    /// 课程状态 (0:未发布, 1:已发布, 2:已下架)
    /// </summary>
    [DisplayName("状态")]
    [Range(0, 2, ErrorMessage = "状态值无效")]
    public int? Status { get; set; }
}

/// <summary>
/// 添加课程信息模型
/// </summary>
public class CourseInsertParam
{
    /// <summary>
    /// 课程名称
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(100, ErrorMessage = "{0}最长100字符")]
    [DisplayName("课程名称")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 课程描述
    /// </summary>
    [MaxLength(2000, ErrorMessage = "{0}最长2000字符")]
    [DisplayName("课程描述")]
    public string? Description { get; set; }

    /// <summary>
    /// 封面图片URL
    /// </summary>
    [MaxLength(255, ErrorMessage = "{0}最长255字符")]
    [Url(ErrorMessage = "请输入有效的{0}地址")]
    [DisplayName("封面图片")]
    public string? CoverImageUrl { get; set; }

    /// <summary>
    /// 课程价格
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [Range(0, 999999.99, ErrorMessage = "{0}范围0-999999.99")]
    [DisplayName("课程价格")]
    public decimal Price { get; set; }

    /// <summary>
    /// 折扣价格
    /// </summary>
    [Range(0, 999999.99, ErrorMessage = "{0}范围0-999999.99")]
    [DisplayName("折扣价格")]
    public decimal? DiscountPrice { get; set; }

    /// <summary>
    /// 课程分类ID
    /// </summary>
    [Required(ErrorMessage = "请选择{0}")]
    [DisplayName("课程分类")]
    public int CategoryId { get; set; }

    /// <summary>
    /// 讲师ID
    /// </summary>
    [Required(ErrorMessage = "请选择{0}")]
    [DisplayName("授课讲师")]
    public int TeacherId { get; set; }

    /// <summary>
    /// 课程状态 (0:未发布, 1:已发布, 2:已下架)
    /// </summary>
    [DisplayName("课程状态")]
    [Range(0, 2, ErrorMessage = "{0}值无效")]
    public int Status { get; set; } = 0;
}

/// <summary>
/// 修改课程信息模型
/// </summary>
public class CourseUpdateParam : CourseInsertParam
{
    /// <summary>
    /// 课程ID
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [DisplayName("课程ID")]
    public int Id { get; set; }
}