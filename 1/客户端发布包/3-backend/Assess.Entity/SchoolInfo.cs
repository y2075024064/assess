using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

/// <summary>
/// 学校信息表
/// </summary>
[Table("ta_school_info")]
public class SchoolInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint? MemberId { get; set; }

    /// <summary>
    /// 学校名称
    /// </summary>
    [MaxLength(32)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 学校全称
    /// </summary>
    [MaxLength(64)]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// 校长姓名
    /// </summary>
    [MaxLength(32)]
    public string? Rector { get; set; }

    /// <summary>
    /// 学校简介
    /// </summary>
    [MaxLength(560)]
    public string? Intro { get; set; }

    /// <summary>
    /// 学校地址
    /// </summary>
    [MaxLength(255)]
    public string? Address { get; set; }

    /// <summary>
    /// 学校类型
    /// </summary>
    public SchoolType SchoolType { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 学校邮箱
    /// </summary>
    [MaxLength(100)]
    public string? Email { get; set; }

    /// <summary>
    /// Logo
    /// </summary>
    [MaxLength(64)]
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
