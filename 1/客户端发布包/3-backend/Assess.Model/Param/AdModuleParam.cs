using System.ComponentModel.DataAnnotations;

namespace Assess.Model.Param;

/// <summary>
/// 广告模块添加参数模型
/// </summary>
public class AdModuleInsertParam
{
    /// <summary>
    /// 模块显示名称
    /// </summary>
    [StringLength(32)]
    [Required(ErrorMessage = "显示名称必填")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 模块所在地点
    /// </summary>
    [StringLength(64)]
    [Required(ErrorMessage = "所在地点必填")]
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 模块标识名称
    /// </summary>
    [StringLength(32)]
    [Required(ErrorMessage = "标识名称必填")]
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
    [StringLength(255)]
    public string? Remark { get; set; }
}

/// <summary>
/// 广告模块编辑参数模型
/// </summary>
public class AdModuleUpdateParam : AdModuleInsertParam
{
    [Required(ErrorMessage = "模块ID必传")]
    public int Id { get; set; }
}