using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Assess.Model.Param;

/// <summary>
/// 新增评论参数模型
/// </summary>
public class CommentInsertParam
{
    /// <summary>
    /// 评论主体对象ID（评论的是信件那就是信件ID；评论的是一条用户评论，那就是该条评论所属的顶级评论ID）
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// 评论子评论ID（就是回复的那条评论的评论ID）
    /// </summary>
    public uint? CommentId { get; set; }

    /// <summary>
    /// 评论对象类型
    /// </summary>
    public ObjectType ObjectType { get; set; }

    /// <summary>
    /// 评论内容
    /// </summary>
    [StringLength(320)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 图片集合
    /// </summary>
    public IFormFile[] Images { get; set; } = Array.Empty<FormFile>();
}

/// <summary>
/// 查询评论参数模型
/// </summary>
public class CommentSearchParam : PageBaseParam
{
    /// <summary>
    /// 排序类型（hot、time）
    /// </summary>
    public string OrderType { get; set; } = string.Empty;

    /// <summary>
    /// 评论对象ID
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// 评论对象类型
    /// </summary>
    public ObjectType ObjectType { get; set; }
}

/// <summary>
/// 查询历史评论参数模型
/// </summary>
public class CommentHistoryParam : PageBaseParam
{
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
/// 管理员评论参数
/// </summary>
public class AdminCommentParam
{
    /// <summary>
    /// 信件ID
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// 评论内容
    /// </summary>
    [StringLength(320)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 是否为精华信件
    /// </summary>
    public bool IsModel { get; set; }

    /// <summary>
    /// 信件状态（Viewed、Processing、Resolved）
    /// </summary>
    public LetterState State { get; set; }

    /// <summary>
    /// 图片集合
    /// </summary>
    public IFormFile[] Images { get; set; } = Array.Empty<FormFile>();
}
