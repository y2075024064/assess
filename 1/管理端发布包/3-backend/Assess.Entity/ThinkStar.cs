using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

[Table("ta_think_star")]
public class ThinkStar
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [MaxLength(255)]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 广告标题
    /// </summary>
    [StringLength(128)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 预览图名称
    /// </summary>
    [StringLength(64)]
    public string? PreviewImg { get; set; }

    /// <summary>
    /// 评论内容
    /// </summary>
    [Column(TypeName = "text")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 评论数量
    /// </summary>
    public uint CommentNum { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Remark { get; set; }

    /// <summary>
    /// 用户信息
    /// </summary>
    [ForeignKey(nameof(MemberId))]
    public virtual Member? Member { get; set; }
}
