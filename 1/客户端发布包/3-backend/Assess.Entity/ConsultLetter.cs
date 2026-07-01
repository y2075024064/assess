using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NPOI.SS.UserModel;

namespace Assess.Entity;

/// <summary>
/// 咨询信件表
/// </summary>
[Table("ta_consult_letter")]
public class ConsultLetter
{
    /// <summary>
    /// ID
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 信件状态
    /// </summary>
    public LetterState State { get; set; }

    /// <summary>
    /// 是否为精华信件
    /// </summary>
    public bool IsModel { get; set; }

    /// <summary>
    /// 是否公开信件
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// 信件标题
    /// </summary>
    [StringLength(64)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 标签数组
    /// </summary>
    [StringLength(256)]
    public string? Label { get; set; }

    /// <summary>
    /// 信件内容
    /// </summary>
    [StringLength(512)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 图片名称字符串集合
    /// </summary>
    public string? Images { get; set; }

    /// <summary>
    /// 点赞数量
    /// </summary>
    public uint LikeNum { get; set; }

    /// <summary>
    /// 浏览量
    /// </summary>
    public uint ViewNum { get; set; }

    /// <summary>
    /// 评论数量
    /// </summary>
    public uint CommentNum { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 是否被标记删除
    /// </summary>
    public bool IsDelete { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Remark { get; set; }

    [ForeignKey(nameof(MemberId))]
    public Member? Member { get; set; }
}
