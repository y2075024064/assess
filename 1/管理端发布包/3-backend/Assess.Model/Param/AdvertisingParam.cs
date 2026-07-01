using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Repository;

namespace Assess.Model.Param;

/// <summary>
/// 广告分页参数模型
/// </summary>
public class AdvertisingPageParam : DefaultEntityPageParam
{
    /// <summary>
    /// 广告模块ID
    /// </summary>
    public int? ModuleId { get; set; }

    /// <summary>
    /// 广告类型
    /// </summary>
    public AdvertisingType? Type { get; set; }
}

/// <summary>
/// 广告添加参数模型
/// </summary>
public class AdvertisingInsertParam
{
    /// <summary>
    /// 广告模块ID
    /// </summary>
    public int? ModuleId { get; set; }

    /// <summary>
    /// 广告类型
    /// </summary>
    [Required(ErrorMessage = "类型必选")]
    public AdvertisingType Type { get; set; }

    /// <summary>
    /// 广告标题
    /// </summary>
    [StringLength(128)]
    [Required(ErrorMessage = "标题必填")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 预览图名称
    /// </summary>
    public IFormFile? PreviewFile { get; set; }

    /// <summary>
    /// 广告内容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 超链接
    /// </summary>
    [StringLength(255)]
    public string? Hyperlink { get; set; }

    /// <summary>
    /// 自定义广告文件路由
    /// </summary>
    public IFormFile[]? HtmlFiles { get; set; }

    /// <summary>
    /// 广告所属公司名称
    /// </summary>
    [StringLength(32)]
    public string? Company { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(255)]
    public string? Remark { get; set; }
}

/// <summary>
/// 广告更新参数模型
/// </summary>
public class AdvertisingUpdateParam : AdvertisingInsertParam
{
    /// <summary>
    /// 广告ID
    /// </summary>
    [Required(ErrorMessage = "广告ID必传")]
    public int Id { get; set; }
}
