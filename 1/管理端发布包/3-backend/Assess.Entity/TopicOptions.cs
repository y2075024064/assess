using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

/// <summary>
/// 科目题目选项表
/// </summary>
[Table("ta_topic_options")]
public class TopicOptions
{
    /// <summary>
    /// 题目ID
    /// </summary>
    [Key]
    public uint TopicId { get; set; }

    /// <summary>
    /// 选项数组（每项为 Markdown 文本）
    /// </summary>
    [Column(TypeName = "json")]
    public string Options { get; set; } = string.Empty;

    [ForeignKey(nameof(TopicId))]
    public virtual TopicInfo? TopicInfo { get; set; }
}
