namespace Assess.Model.Result;

/// <summary>
/// 动态分页每项信息模型
/// </summary>
public class DynamicPageItem
{
    /// <summary>
    /// 动态ID
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// 动态类型
    /// </summary>
    public DynamicType Type { get; set; }

    /// <summary>
    /// 动态标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 原文连接
    /// </summary>
    public string? OriginalHref { get; set; }

    /// <summary>
    /// 预计发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 动态创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 动态编辑时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 动态详情模型
/// </summary>
public class DynamicDetail : DynamicPageItem
{
    /// <summary>
    /// 动态内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 封面图片名称
    /// </summary>
    public string? PreviewImg { get; set; }
}

/// <summary>
/// 动态列表每项信息模型
/// </summary>
public class DynamicListItem
{
    /// <summary>
    /// 动态ID
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// 动态标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 动态内容简要
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 封面图片名称
    /// </summary>
    public string? PreviewImg { get; set; }

    /// <summary>
    /// 预计发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 发布用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 发布用户昵称
    /// </summary>
    public string? NickName { get; set; }

    /// <summary>
    /// 发布用户头像路径
    /// </summary>
    public string? Avatar { get; set; }
}

/// <summary>
/// 动态列表每项详情模型
/// </summary>
public class DynamicListDetail : DynamicListItem
{
    /// <summary>
    /// 原文连接
    /// </summary>
    public string? OriginalHref { get; set; }

    /// <summary>
    /// 动态编辑时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }
}
