namespace Assess.Model.Result;

/// <summary>
/// 字典详情
/// </summary>
public class DictionaryDetail
{
    /// <summary>
    /// 标识
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 字典键
    /// </summary>
    public DictionaryType Key { get; set; }

    /// <summary>
    /// 字典值
    /// </summary>
    public string Value { get; set; } = string.Empty;
}
