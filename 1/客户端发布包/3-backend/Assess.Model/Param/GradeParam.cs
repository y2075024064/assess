using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Model.Param;

/// <summary>
/// 年级新增参数模型
/// </summary>
public class GradeInsertParam
{
    /// <summary>
    /// 名称
    /// </summary>
    [StringLength(10)]
    [Required(ErrorMessage = "名称必填")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 排序级别
    /// </summary>
    public int SortLevel { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Remark { get; set; }
}

/// <summary>
/// 年级更新参数模型
/// </summary>
public class GradeUpdateParam : GradeInsertParam
{
    public int Id { get; set; }
}