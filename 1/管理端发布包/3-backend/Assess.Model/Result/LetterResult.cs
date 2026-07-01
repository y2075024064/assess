namespace Assess.Model.Result;

/// <summary>
/// 信件基础信息模型
/// </summary>
public class LetterBaseInfo
{
    /// <summary>
    /// 信件ID
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// 发布用户昵称
    /// </summary>
    public string? Nickname { get; set; }

    /// <summary>
    /// 发布用户头像
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 信件标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 信件状态
    /// </summary>
    public LetterState State { get; set; }

    /// <summary>
    /// 是否为精华信件
    /// </summary>
    public bool IsModel { get; set; }

    /// <summary>
    /// 信件内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 是否公开信件
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// 标签数组
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// 点赞数量
    /// </summary>
    public uint LikeNum { get; set; }

    /// <summary>
    /// 浏览量
    /// </summary>
    public uint ViewNum { get; set; }

    /// <summary>
    /// 评论数量
    /// </summary>
    public uint CommentNum { get; set; }

    /// <summary>
    /// 图片
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }
}

/// <summary>
/// 信件详细信息模型
/// </summary>
public class LetterDetailInfo : LetterBaseInfo
{
    /// <summary>
    /// 图片名称字符串集合
    /// </summary>
    public string[] Images { get; set; } = Array.Empty<string>();

    /// <summary>
    /// 浏览记录（没有登录的该值为空）
    /// </summary>
    public BrowseRecordInfo? BrowseRecord { get; set; }
}
