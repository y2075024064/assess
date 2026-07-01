using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

[Table("ta_dynamic_info")]
public class DynamicInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    /// <summary>
    /// 动态类型
    /// </summary>
    public DynamicType Type { get; set; }

    /// <summary>
    /// 动态标题
    /// </summary>
    [StringLength(125)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 动态内容
    /// </summary>
    [Column(TypeName = "text")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 封面图片名称
    /// </summary>
    [StringLength(64)]
    public string? PreviewImg { get; set; }

    /// <summary>
    /// 原文连接
    /// </summary>
    [StringLength(255)]
    public string? OriginalHref { get; set; }

    /// <summary>
    /// 预计发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 创建用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 动态创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 动态编辑时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }

    /// <summary>
    /// 删除标记
    /// </summary>
    public bool IsDelete { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Remark { get; set; }

    [ForeignKey(nameof(MemberId))]
    public virtual Member? Member { get; set; }
}
