using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assess.Entity;

/// <summary>
/// 题目标签表
/// </summary>
[Table("ta_topic_label")]
[Index(nameof(Name), Name = "UQ_TopicLabel_Name", IsUnique = true)]
public class TopicLabel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// 标签名称
    /// </summary>
    [StringLength(32)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 标签原名称
    /// </summary>
    [StringLength(32)]
    public string? OldName { get; set; }

    /// <summary>
    /// 最近一次修改时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Remark { get; set; }
}
