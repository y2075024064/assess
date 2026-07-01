using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assess.Entity;

/// <summary>
/// 广告信息表
/// </summary>
[Table("ta_advertising")]
[Index(nameof(Title), Name = "UQ_Advertising_Title", IsUnique = true)]
public class Advertising
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// 广告模块ID
    /// </summary>
    public int? ModuleId { get; set; }

    /// <summary>
    /// 广告类型
    /// </summary>
    public AdvertisingType Type { get; set; }

    /// <summary>
    /// 广告标题
    /// </summary>
    [StringLength(128)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 预览图名称
    /// </summary>
    [StringLength(64)]
    public string? PreviewImg { get; set; }

    /// <summary>
    /// 广告内容
    /// </summary>
    [Column(TypeName = "text")]
    public string? Content { get; set; }

    /// <summary>
    /// 超链接
    /// </summary>
    [StringLength(255)]
    public string? Hyperlink { get; set; }

    /// <summary>
    /// 自定义广告文件路由
    /// </summary>
    [StringLength(128)]
    public string? FolderUrl { get; set; }

    /// <summary>
    /// 广告所属公司名称
    /// </summary>
    [StringLength(32)]
    public string? Company { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Remark { get; set; }

    public virtual AdModule? Module { get; set; }
}
