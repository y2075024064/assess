using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assess.Entity;

/// <summary>
/// 广告模块表
/// </summary>
[Table("ta_ad_module")]
[Index(nameof(ModuleName), Name = "UQ_AdModule_ModuleName", IsUnique = true)]
public class AdModule
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// 模块显示名称
    /// </summary>
    [StringLength(32)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 模块所在地点
    /// </summary>
    [StringLength(64)]
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 模块标识名称
    /// </summary>
    [StringLength(32)]
    public string ModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用该模块
    /// </summary>
    public bool IsEnable { get; set; }

    /// <summary>
    /// 是否启用轮播
    /// </summary>
    public bool IsCarousel { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Remark { get; set; }

    public List<Advertising> Advertisings { get; set; } = new List<Advertising>();
}
