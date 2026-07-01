using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assess.Entity;

/// <summary>
/// 用户信息表
/// </summary>
[Table("ta_member")]
[Index(nameof(Nickname), Name = "UQ_Member_Nickname", IsUnique = true)]
public class Member
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [StringLength(40)]
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    [StringLength(20)]
    public string? RealName { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public GenderType Gender { get; set; }

    /// <summary>
    /// 头像路径
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Avatar { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public MemberType Type { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [Column(TypeName = "char(15)")]
    public string? MobilePhone { get; set; }

    /// <summary>
    /// 电子邮箱地址
    /// </summary>
    [StringLength(128)]
    public string? Email { get; set; }

    /// <summary>
    /// 主账户ID
    /// </summary>
    public uint? MainMemberId { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public DateOnly? Birthday { get; set; }

    /// <summary>
    /// 身份证号码
    /// </summary>
    [Column(TypeName = "char(18)")]
    public string? IdCard { get; set; }

    /// <summary>
    /// 介绍用户ID
    /// </summary>
    public uint? RefereeId { get; set; }

    /// <summary>
    /// 是否绑定微信
    /// </summary>
    public bool IsBindWechat { get; set; }

    /// <summary>
    /// 推荐码
    /// </summary>
    [Column(TypeName = "char(10)")]
    public string? InvitationCode { get; set; }

    /// <summary>
    /// 用户题目收藏自定义分组数组
    /// </summary>
    [Column(TypeName = "json")]
    public string? CollectGroupNames { get; set; }

    /// <summary>
    /// 账户创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 最近一次修改时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }

    /// <summary>
    /// 是否已注销账户
    /// </summary>
    public bool IsDelete { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(TypeName = "tinytext")]
    public string? Remark { get; set; }

    public virtual MemberAccount? MemberAccount { get; set; }
}