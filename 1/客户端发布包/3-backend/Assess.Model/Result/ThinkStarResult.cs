namespace Assess.Model.Result;

/// <summary>
/// 思维之星分页项模型
/// </summary>
public class ThinkStarPageItem
{
    public uint Id { get; set; }

    /// <summary>
    /// 用户账户
    /// </summary>
    public string? Account { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 思维之星基础信息模型
/// </summary>
public class ThinkStarBaseItem
{
    /// <summary>
    /// ID
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 预览图路径
    /// </summary>
    public string? PreviewImg { get; set; }
}

/// <summary>
/// 思维之星详情
/// </summary>
public class ThinkStarDetail : ThinkStarPageItem
{
    /// <summary>
    /// 预览图名称
    /// </summary>
    public string? PreviewImg { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 头像路径
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 评论数量
    /// </summary>
    public uint CommentNum { get; set; }
}
