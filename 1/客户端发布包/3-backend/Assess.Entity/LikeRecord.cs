using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

[Table("ta_like_record")]
public class LikeRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 我的点赞记录ID
    /// </summary>
    public long MyLikeRecordId { get; set; }

    /// <summary>
    /// 点赞对象ID
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// 点赞对象类型
    /// </summary>
    public ObjectType ObjectType { get; set; }

    /// <summary>
    /// 点赞时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    [ForeignKey(nameof(MyLikeRecordId))]
    public virtual MyLikeRecord? MyLikeRecord { get; set; }
}
