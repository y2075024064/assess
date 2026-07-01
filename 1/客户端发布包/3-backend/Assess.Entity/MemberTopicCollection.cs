using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

/// <summary>
/// 用户题目收藏表
/// </summary>
[Table("ta_member_topic_collection")]
public class MemberTopicCollection
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 题目ID
    /// </summary>
    public uint TopicId { get; set; }

    /// <summary>
    /// 自定义收藏分组名称
    /// </summary>
    [StringLength(120)]
    public string? GroupName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    [ForeignKey(nameof(TopicId))]
    public virtual TopicInfo? TopicInfo { get; set; }

    [ForeignKey(nameof(MemberId))]
    public virtual Member? Member { get; set; }
}
