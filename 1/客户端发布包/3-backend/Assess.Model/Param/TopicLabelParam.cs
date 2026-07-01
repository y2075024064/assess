using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Model.Param;

/// <summary>
/// 标签新增参数模型
/// </summary>
public class TopicLabelInsertParam
{
    /// <summary>
    /// 名称
    /// </summary>
    [StringLength(32)]
    [Required(ErrorMessage = "名称必填")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 备注
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Remark { get; set; }
}

/// <summary>
/// 标签更新参数模型
/// </summary>
public class TopicLabelUpdateParam : TopicLabelInsertParam
{
    public int Id { get; set; }
}