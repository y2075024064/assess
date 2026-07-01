using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

[Table("ta_comment_info")]
public class CommentInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 评论主体对象ID
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// 评论子对象ID
    /// </summary>
    public uint? CommentId { get; set; }

    /// <summary>
    /// 点赞数量
    /// </summary>
    public uint LikeNum { get; set; }

    /// <summary>
    /// 是否为置顶评论
    /// </summary>
    public bool IsTop { get; set; }

    /// <summary>
    /// 是否为精华评论
    /// </summary>
    public bool IsUseful { get; set; }

    /// <summary>
    /// 评论类型
    /// </summary>
    public CommentType Type { get; set; }

    /// <summary>
    /// 评论对象类型
    /// </summary>
    public ObjectType ObjectType { get; set; }

    /// <summary>
    /// 评论内容
    /// </summary>
    [StringLength(320)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 图片名称字符串集合
    /// </summary>
    public string? Images { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 是否被标记删除
    /// </summary>
    public bool IsDelete { get; set; }

    [ForeignKey(nameof(MemberId))]
    public Member? Member { get; set; }
}
