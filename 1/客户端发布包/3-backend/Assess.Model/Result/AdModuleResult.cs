namespace Assess.Model.Result;

/// <summary>
/// 广告模块详情模型
/// </summary>
public class AdModuleDetail
{
    public int Id { get; set; }

    /// <summary>
    /// 模块显示名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 模块地实在地点
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 模块标识名称
    /// </summary>
    public string ModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用该模块
    /// </summary>
    public bool IsEnable { get; set; }

    /// <summary>
    /// 是否启用自动轮播
    /// </summary>
    public bool IsCarousel { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 广告信息模型
/// </summary>
public class AdInfoModel
{
    /// <summary>
    /// 广告ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 广告标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 广告类型（富文本类型的广告需要另调接口获取广告内容）
    /// </summary>
    public AdvertisingType Type { get; set; }

    /// <summary>
    /// 超链接
    /// </summary>
    public string? Hyperlink { get; set; }

    /// <summary>
    /// 预览图
    /// </summary>
    public string? PreviewImg { get; set; }
}

/// <summary>
/// 广告模块模型
/// </summary>
public class ModuleInfoModel
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool EnableMark { get; set; }

    /// <summary>
    /// 是否自动轮播
    /// </summary>
    public bool IsCarousel { get; set; }

    /// <summary>
    /// 广告列表
    /// </summary>
    public AdInfoModel[] AdList { get; set; } = Array.Empty<AdInfoModel>();
}
