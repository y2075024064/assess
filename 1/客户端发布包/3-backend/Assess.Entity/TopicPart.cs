using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

/// <summary>
/// 题目分部表
/// </summary>
[Table("ta_topic_part")]
public class TopicPart
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// 父级ID
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// 类型名称
    /// </summary>
    [StringLength(32)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 原名称
    /// </summary>
    [StringLength(32)]
    public string? OldName { get; set; }

    /// <summary>
    /// 推荐年级数组
    /// </summary>
    [Column(TypeName = "json")]
    public string? RcmdGrade { get; set; }

    /// <summary>
    /// 最近一次修改时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }

    /// <summary>
    /// 排序级别
    /// </summary>
    public int SortLevel { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Remark { get; set; }

    [ForeignKey(nameof(ParentId))]
    [InverseProperty(nameof(Childrens))]
    public virtual TopicPart? Parent { get; set; }

    [InverseProperty(nameof(Parent))]
    public virtual ICollection<TopicPart> Childrens { get; set; } = new List<TopicPart>();
}
