using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

/// <summary>
/// 用户错题记录表
/// </summary>
[Table("ta_member_error_record")]
public class MemberErrorRecord
{
    [Key]
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
    /// 用户最新的错误答案
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string ErrorAnswer { get; set; } = string.Empty;

    /// <summary>
    /// 用户是否通过
    /// </summary>
    public bool IsPass { get; set; }

    /// <summary>
    /// 通过时间
    /// </summary>
    public DateTime? PassTime { get; set; }

    /// <summary>
    /// 最新一次答错时间
    /// </summary>
    public DateTime LastErrorTime { get; set; }

    [ForeignKey(nameof(TopicId))]
    public virtual TopicInfo? TopicInfo { get; set; }
}
