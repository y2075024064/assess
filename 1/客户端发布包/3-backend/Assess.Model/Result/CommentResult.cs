namespace Assess.Model.Result;

/// <summary>
/// 评论分页结果信息模型
/// </summary>
public class CommentResultInfo
{
    /// <summary>
    /// 评论ID
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// 评论用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 评论的用户类型
    /// </summary>
    public MemberType MemberType { get; set; }

    /// <summary>
    /// 评论用户昵称
    /// </summary>
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// 评论用户头像地址
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 回复对象用户Id
    /// </summary>
    public uint? ReplyMemberId { get; set; }

    /// <summary>
    /// 回复对象用户昵称
    /// </summary>
    public string? ReplyNickname { get; set; }

    /// <summary>
    /// 评论内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 点赞数量
    /// </summary>
    public long LikeNum { get; set; }

    /// <summary>
    /// 评论时间
    /// </summary>
    /// <example>2021-01-01 12:00:00</example>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 内容图片地址集合
    /// </summary>
    public string[] Images { get; set; } = Array.Empty<string>();

    /// <summary>
    /// 是否已点赞
    /// </summary>
    public bool IsLike { get; set; }

    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool IsTop { get; set; }

    /// <summary>
    /// 子回复总数
    /// </summary>
    public long ChildrenTotal { get; set; }

    /// <summary>
    /// 子回复列表
    /// </summary>
    public CommentResultInfo[] Children { get; set; } = Array.Empty<CommentResultInfo>();
}
