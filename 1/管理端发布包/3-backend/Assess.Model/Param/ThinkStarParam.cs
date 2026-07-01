using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Repository;

namespace Assess.Model.Param;

/// <summary>
/// 思维之星分页参数模型
/// </summary>
public class ThinkStarPageParam : DefaultEntityPageParam
{
    /// <summary>
    /// 类型
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// 开始时间范围
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间范围
    /// </summary>
    public DateTime? EndTime { get; set; }
}

/// <summary>
/// 思维之星添加信息模型
/// </summary>
public class ThinkStarInsertParam
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [Required]
    public uint MemberId { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 广告标题
    /// </summary>
    [Required]
    [StringLength(128)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 预览图
    /// </summary>
    public IFormFile? Image { get; set; }

    /// <summary>
    /// 评论内容
    /// </summary>
    [Required]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(255)]
    public string? Remark { get; set; }
}

/// <summary>
/// 思维之星更改信息模型
/// </summary>
public class ThinkStarUpdateParam : ThinkStarInsertParam
{
    /// <summary>
    /// 思维之星ID
    /// </summary>
    public uint Id { get; set; }
}
