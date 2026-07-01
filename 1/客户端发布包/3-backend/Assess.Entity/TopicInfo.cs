using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assess.Entity;

/// <summary>
/// 题目信息表
/// </summary>
[Table("ta_topic_info")]
[Index(nameof(Number), Name = "UQ_TopicInfo_Number", IsUnique = true)]
public class TopicInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    /// <summary>
    /// 题目编号
    /// </summary>
    [Column(TypeName = "char(12)")]
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// 题目标题
    /// </summary>
    [StringLength(128)]
    public string? Title {  get; set; }

    /// <summary>
    /// 题目类型
    /// </summary>
    public TopicType Type { get; set; }

    /// <summary>
    /// 题目难度，0.0 - 1.0，越小越难
    /// </summary>
    public decimal Level { get; set; } = 1;

    /// <summary>
    /// 分部ID
    /// </summary>
    public int? PartId { get; set; }

    /// <summary>
    /// 题目标签数组
    /// </summary>
    [StringLength(256)]
    public string? Label { get; set; }

    /// <summary>
    /// 推荐年级数组
    /// </summary>
    [StringLength(32)]
    public string? RcmdGrade { get; set; }

    /// <summary>
    /// 考察目的
    /// </summary>
    [Column(TypeName = "text")]
    public string? Purpose { get; set; }

    /// <summary>
    /// 出题用户ID
    /// </summary>
    public uint? MemberId { get; set; }

    /// <summary>
    /// 题目内容（Markdown）
    /// </summary>
    [Column(TypeName = "longtext")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 用途
    /// </summary>
    public TopicUseType UseType { get; set; }

    /// <summary>
    /// 正确答案
    /// </summary>
    [Column(TypeName = "json")]
    public string? RightAnswer { get; set; }

    /// <summary>
    /// 答案解析
    /// </summary>
    [Column(TypeName = "longtext")]
    public string? Analysis { get; set; }

    /// <summary>
    /// 点赞数量
    /// </summary>
    public uint LikeNum { get; set; }

    /// <summary>
    /// 收藏数量
    /// </summary>
    public int CollectionNum { get; set; }

    /// <summary>
    /// 评论数量
    /// </summary>
    public int CommentNum { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    [StringLength(256)]
    public string? Origin { get; set; }

    /// <summary>
    /// 原始编号
    /// </summary>
    [StringLength(128)]
    public string? OriginNumber { get; set; }

    /// <summary>
    /// 原始类型
    /// </summary>
    [StringLength(128)]
    public string? OriginType { get; set; }

    /// <summary>
    /// 是否原创
    /// </summary>
    public bool IsOriginal { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 最近一次修改时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }

    /// <summary>
    /// 是否已删除
    /// </summary>
    public bool IsDelete { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Remark { get; set; }

    [ForeignKey(nameof(PartId))]
    public virtual TopicPart? Part { get; set; }

    [ForeignKey(nameof(MemberId))]
    public virtual Member? Member { get; set; }

    public virtual TopicOptions? TopicOptions { get; set; }

    public List<PracticeRecord> PracticeRecords { get; set; } = new List<PracticeRecord>();
}
