using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.HashTools;

namespace Assess.Entity;

/// <summary>
/// 用户账户信息表
/// </summary>
[Table("ta_member_account")]
[Index(nameof(Account), Name = "UQ_MemberAccount_Account", IsUnique = true)]
public class MemberAccount
{
    [Key]
    public uint MemberId { get; set; }

    /// <summary>
    /// 用户账户名
    /// </summary>
    [StringLength(32)]
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 微信开放平台统一码 UnionId
    /// </summary>
    [StringLength(64)]
    public string? UnionId { get; set; }

    /// <summary>
    /// 微信公众号 OpenId
    /// </summary>
    [StringLength(64)]
    public string? OpenId { get; set; }

    /// <summary>
    /// 微信小程序 OpenId
    /// </summary>
    [StringLength(64)]
    public string? MiniOpenId { get; set; }

    /// <summary>
    /// 账户密码
    /// </summary>
    [StringLength(240)]
    public string? Password { get; set; }

    /// <summary>
    /// 密码签名
    /// </summary>
    [StringLength(120)]
    public string? Signature { get; set; }

    /// <summary>
    /// 加密类型
    /// </summary>
    public EncodeType EncodeType { get; set; }

    /// <summary>
    /// 账户是否锁住
    /// </summary>
    public bool IsLocked { get; set; }

    /// <summary>
    /// 账户禁用开始时间
    /// </summary>
    public DateTime? DisableStartTime { get; set; }

    /// <summary>
    /// 账户禁用结束时间
    /// </summary>
    public DateTime? DisableEndTime { get; set; }

    /// <summary>
    /// 是否可以修改账户名
    /// </summary>
    public bool CanModifyAccount { get; set; }

    [ForeignKey(nameof(MemberId))]
    public virtual Member? Member { get; set; }
}