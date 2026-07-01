using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

[Table("ta_my_like_record")]
public class MyLikeRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 点赞总数
    /// </summary>
    public uint LikeNum { get; set; }

    /// <summary>
    /// 点赞对象ID
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// 点赞对象类型
    /// </summary>
    public ObjectType ObjectType { get; set; }

    /// <summary>
    /// 最近点赞时间
    /// </summary>
    public DateTime LastTime { get; set; }

    public List<LikeRecord> LikeRecords { get; set; } = new List<LikeRecord>();
}
