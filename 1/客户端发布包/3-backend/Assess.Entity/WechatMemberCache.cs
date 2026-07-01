using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

[Table("ta_wechat_member_cache")]
public class WechatMemberCache
{
    [Key]
    [StringLength(64)]
    public string OpenId { get; set; } = string.Empty;

    [StringLength(64)]
    public string? Scope { get; set; }

    [StringLength(240)]
    public string? Atoken { get; set; }

    public DateTime? AtokenOverdueTime { get; set; }

    [StringLength(240)]
    public string? Rtoken { get; set; }

    public DateTime? RtokenOverdueTime { get; set; }
}