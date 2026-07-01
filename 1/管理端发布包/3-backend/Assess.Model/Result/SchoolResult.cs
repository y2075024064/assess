namespace Assess.Model.Result;

/// <summary>
/// 学校分页项信息模型
/// </summary>
public class SchoolPageItem
{
    /// <summary>
    /// 学校ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 用户账户
    /// </summary>
    public string? Account { get; set; }

    /// <summary>
    /// 学校名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 学校全称
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// 校长姓名
    /// </summary>
    public string? Rector { get; set; }

    /// <summary>
    /// 学校简介
    /// </summary>
    public string? Intro { get; set; }

    /// <summary>
    /// 学校地址
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// 学校类型
    /// </summary>
    public SchoolType SchoolType { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 学校邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Logo
    /// </summary>
    public string? Logo { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 最近一次修改时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}
