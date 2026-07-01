using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

/// <summary>
/// 系统字典信息表
/// </summary>
[Table("ta_dictionary")]
public class DictionaryInfo
{
    /// <summary>
    /// 报销标识
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// 字典键
    /// </summary>
    public DictionaryType Key { get; set; }

    /// <summary>
    /// 字典值
    /// </summary>
    [MaxLength(255)]
    public string Value { get; set; } = string.Empty;
}
