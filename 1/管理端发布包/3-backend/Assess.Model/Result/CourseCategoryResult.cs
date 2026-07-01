namespace Assess.Model.Result;

/// <summary>
/// 课程分类项
/// </summary>
public class CourseCategoryItem
{
    /// <summary>
    /// 分类ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 分类名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 排序权重
    /// </summary>
    public int SortWeight { get; set; }

    /// <summary>
    /// 分类描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 父分类ID
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// 父分类名称
    /// </summary>
    public string? ParentName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}
