namespace Assess.Model.Result;

public class LikeListResult
{
    /// <summary>
    /// 用户头像地址集合
    /// </summary>
    public string[] Avatars { get; set; } = Array.Empty<string>();

    /// <summary>
    /// 用户昵称集合
    /// </summary>
    public string[] Nicknames { get; set; } = Array.Empty<string>();

    /// <summary>
    /// 点赞总数
    /// </summary>
    public uint Total { get; set; }

    /// <summary>
    /// 点赞最新时间
    /// </summary>
    public DateTime LikeTime { get; set; }

    /// <summary>
    /// 点赞的内容简要
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 点赞的对象ID
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// 点赞的对象类型
    /// </summary>
    public ObjectType ObjectType { get; set; }
}
