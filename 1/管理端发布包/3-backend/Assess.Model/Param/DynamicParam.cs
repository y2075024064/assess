using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Repository;

namespace Assess.Model.Param;

/// <summary>
/// 广告分页参数模型
/// </summary>
public class DynamicPageParam : DefaultEntityPageParam
{
    /// <summary>
    /// 动态类型
    /// </summary>
    public DynamicType? Type { get; set; }
}

/// <summary>
/// 添加动态信息模型
/// </summary>
public class DynamicInsertParam
{
    /// <summary>
    /// 动态类型
    /// </summary>
    [Required(ErrorMessage = "类型必选")]
    public DynamicType Type { get; set; }

    /// <summary>
    /// 动态标题
    /// </summary>
    [StringLength(125)]
    [Required(ErrorMessage = "标题必填")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 动态内容
    /// </summary>
    [Required(ErrorMessage = "内容必填")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 封面图片文件
    /// </summary>
    public IFormFile? PreviewFile { get; set; }

    /// <summary>
    /// 原文连接
    /// </summary>
    [StringLength(255)]
    public string? OriginalHref { get; set; }

    /// <summary>
    /// 预计发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(255)]
    public string? Remark { get; set; }
}

/// <summary>
/// 更改动态信息模型
/// </summary>
public class DynamicUpdateParam : DynamicInsertParam
{
    /// <summary>
    /// 动态ID
    /// </summary>
    [Required(ErrorMessage = "ID必传")]
    public uint Id { get; set; }
}
