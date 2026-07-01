using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Repository;

namespace Assess.Model.Param;

/// <summary>
/// 分类分页参数
/// </summary>
public class CourseCategoryPageParam : DefaultEntityPageParam
{
    /// <summary>
    /// 父分类ID
    /// </summary>
    [DisplayName("父分类ID")]
    public int? ParentId { get; set; }
}

/// <summary>
/// 添加分类参数
/// </summary>
public class CourseCategoryInsertParam
{
    /// <summary>
    /// 分类名称
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [StringLength(50, ErrorMessage = "{0}最长50字符")]
    [DisplayName("分类名称")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 分类描述
    /// </summary>
    [StringLength(500, ErrorMessage = "{0}最长500字符")]
    [DisplayName("分类描述")]
    public string? Description { get; set; }

    /// <summary>
    /// 父分类ID
    /// </summary>
    [DisplayName("父分类")]
    public int? ParentId { get; set; }

    /// <summary>
    /// 排序权重
    /// </summary>
    [DisplayName("排序权重")]
    [Range(0, 1000, ErrorMessage = "{0}范围0-1000")]
    public int SortWeight { get; set; } = 0;
}

/// <summary>
/// 修改分类参数
/// </summary>
public class CourseCategoryUpdateParam : CourseCategoryInsertParam
{
    /// <summary>
    /// 分类ID
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [DisplayName("分类ID")]
    public int Id { get; set; }
}
