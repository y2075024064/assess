using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Assess.Model.Param;

/// <summary>
/// 新增信件参数模型
/// </summary>
public class LetterInsertParam
{
    /// <summary>
    /// 是否公开信件
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// 信件标题
    /// </summary>
    [StringLength(64)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 标签数组
    /// </summary>
    public string[]? Label { get; set; }

    /// <summary>
    /// 信件内容
    /// </summary>
    [StringLength(512)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 图片文件集合
    /// </summary>
    public IFormFile[] Images { get; set; } = Array.Empty<FormFile>();
}

/// <summary>
/// 信件查询参数模型
/// </summary>
public class LetterSearchParam : PageBaseParam
{
    /// <summary>
    /// 开始时间范围
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间范围
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 排序类型（0：时间、1：热度）
    /// </summary>
    public byte OrderType { get; set; }

    /// <summary>
    /// 查询关键字
    /// </summary>
    public string? Keyword { get; set; }
}
