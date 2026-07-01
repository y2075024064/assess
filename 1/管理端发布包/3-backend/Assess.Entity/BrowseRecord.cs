using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

[Table("ta_browse_record")]
public class BrowseRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// 浏览对象ID
    /// </summary>
    public long ObjectId { get; set; }

    /// <summary>
    /// 浏览对象类型
    /// </summary>
    public ObjectType ObjType { get; set; }

    /// <summary>
    /// 是否点赞
    /// </summary>
    public bool IsGiveLike { get; set; }

    /// <summary>
    /// 是否收藏
    /// </summary>
    public bool IsCollection { get; set; }

    /// <summary>
    /// 最后一次浏览时间
    /// </summary>
    public DateTime LastBrowseOn { get; set; }

    /// <summary>
    /// 浏览用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 第一次浏览时间
    /// </summary>
    public DateTime CreationTime { get; set; }
}
