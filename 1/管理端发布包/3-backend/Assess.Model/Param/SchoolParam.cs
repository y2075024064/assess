using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Repository;

namespace Assess.Model.Param;

/// <summary>
/// 学校分页参数信息模型
/// </summary>
public class SchoolPageParam : DefaultEntityPageParam
{
    /// <summary>
    /// 学校类型
    /// </summary>
    public SchoolType? SchoolType { get; set; }
}

/// <summary>
/// 添加学校信息模型
/// </summary>
public class SchoolInsertParam
{
    /// <summary>
    /// 学校名称
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 学校全称
    /// </summary>
    [Required]
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
    [Required]
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
    public IFormFile? LogoFile { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(255)]
    public string? Remark { get; set; }
}

/// <summary>
/// 更改学校信息模型
/// </summary>
public class SchoolUpdateParam : SchoolInsertParam
{
    /// <summary>
    /// 学校ID
    /// </summary>
    [Required]
    public int Id { get; set; }
}
