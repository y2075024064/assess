using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assess.Entity;

/// <summary>
/// 用户访问凭证表
/// </summary>
[Table("ta_member_token")]
[Index(nameof(Token), Name = "UQ_MemberToken_Token", IsUnique = true)]
public class MemberToken
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public uint MemberId { get; set; }

    /// <summary>
    /// 用户访问凭证
    /// </summary>
    [Column(TypeName = "char(64)")]
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// 客户端类型
    /// </summary>
    public ClientType ClientType { get; set; }

    /// <summary>
    /// 凭证过期时间
    /// </summary>
    [Column(TypeName = "timestamp")]
    public DateTime ExpirationTime { get; set; }

    /// <summary>
    /// 是否永久记住该凭证
    /// </summary>
    public bool IsRemember { get; set; }

    [ForeignKey(nameof(MemberId))]
    public virtual Member? Member { get; set; }
}