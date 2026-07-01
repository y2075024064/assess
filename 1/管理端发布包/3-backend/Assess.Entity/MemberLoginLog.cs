using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assess.Entity;

/// <summary>
/// 用户登录日志表
/// </summary>
[Table("ta_member_login_log")]
[Index(nameof(MemberId), Name = "IX_MemberLoginLog_MemberId")]
public class MemberLoginLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 用户账户名
    /// </summary>
    [StringLength(32)]
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 登录失败时输入的密码
    /// </summary>
    [StringLength(30)]
    public string? FailPwd { get; set; }

    /// <summary>
    /// 登录验证时间
    /// </summary>
    public DateTime CheckTime { get; set; }

    /// <summary>
    /// 是否验证通过
    /// </summary>
    public bool IsPass { get; set; }

    /// <summary>
    /// 客户端类型
    /// </summary>
    public ClientType ClientType { get; set; }

    /// <summary>
    /// 登录客户端IP
    /// </summary>
    [StringLength(15)]
    public string? ClientIp { get; set; }

    /// <summary>
    /// 验证通过时间
    /// </summary>
    public DateTime? PassTime { get; set; }

    /// <summary>
    /// 验证失败原因
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Reason { get; set; }
}
