using Repository;

namespace Assess.Model.Param;

/// <summary>
/// 字典分页参数模型
/// </summary>
public class DictionaryPageParam : DefaultEntityPageParam
{
    /// <summary>
    /// 字典键
    /// </summary>
    public DictionaryType? Key { get; set; }
}
