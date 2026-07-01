using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

[Table("ta_wechat_cache")]
public class WechatCache
{
    [Key]
    [StringLength(32)]
    public string AppId { get; set; } = string.Empty;

    [StringLength(240)]
    public string Token { get; set; } = string.Empty;

    public DateTime? TokenOverdueTime { get; set; }

    [StringLength(240)]
    public string? Jticket { get; set; }

    public DateTime? JticketOverdueTime { get; set; }
}