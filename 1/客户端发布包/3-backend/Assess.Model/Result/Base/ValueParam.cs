using System.ComponentModel.DataAnnotations;

namespace Assess.Model.Result;

/// <summary>
/// 单个值参数模型
/// </summary>
public class ValueParam
{
    /// <summary>
    /// 传递的值
    /// </summary>
    [Required(ErrorMessage = "参数值不能为空")]
    public string Value { get; set; } = string.Empty;
}
