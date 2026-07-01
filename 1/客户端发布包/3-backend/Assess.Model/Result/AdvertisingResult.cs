namespace Assess.Model.Result;

/// <summary>
/// 广告详情模型
/// </summary>
public class AdvertisingPageItem
{
    public int Id { get; set; }

    /// <summary>
    /// 模块显示名称
    /// </summary>
    public string? ModuleName { get; set; }

    /// <summary>
    /// 广告标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 广告类型
    /// </summary>
    public AdvertisingType Type { get; set; }

    /// <summary>
    /// 超链接
    /// </summary>
    public string? Hyperlink { get; set; }

    /// <summary>
    /// 自定义广告文件路由
    /// </summary>
    public string? FolderUrl { get; set; }

    /// <summary>
    /// 广告所属公司名称
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 广告分页列表每项信息模型
/// </summary>
public class AdvertisingInfo : AdvertisingPageItem
{
    /// <summary>
    /// 广告内容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 广告模块ID
    /// </summary>
    public int? ModuleId { get; set; }

    /// <summary>
    /// 预览图名称
    /// </summary>
    public string? PreviewImg { get; set; }
}
