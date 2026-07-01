using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using Repository;

namespace Assess.Model.Param;

/// <summary>
/// 微信Code参数模型
/// </summary>
public class WinXinCodeParam
{
    /// <summary>
    /// 小程序返回的Code
    /// </summary>
    /// <remarks>啊手动阀手动阀</remarks>
    /// <example>kajdhsfiahweuihoiashdofihwehfiabdsv</example>
    [Required(ErrorMessage = "code 不能为空！")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 是否记住当前登录状态
    /// </summary>
    /// <example>false</example>
    [DefaultValue(false)]
    public bool Remember { get; set; } = false;
}

/// <summary>
/// 个人用户分页参数
/// </summary>
public class MemberPageParam : DefaultEntityPageParam
{
    /// <summary>
    /// 用户类型
    /// </summary>
    public MemberType? Type { get; set; }

    /// <summary>
    /// 用户性别
    /// </summary>
    public GenderType? Gender { get; set; }

    /// <summary>
    /// 注册码
    /// </summary>
    public string? InvitationCode { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string? MobilePhone { get; set; }

    /// <summary>
    /// 查询开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 查询结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
}

/// <summary>
/// 用户登录信息模型
/// </summary>
public class MemberLoginParam
{
    /// <summary>
    /// 账户名（个人为手机号）
    /// </summary>
    [Required(ErrorMessage = "账户名必填")]
    [StringLength(32, MinimumLength = 2, ErrorMessage = "账户名长度应为 2-32 位")]
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 登录密码（长度范围 8-20）
    /// </summary>
    [Required(ErrorMessage = "登录密码必填")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "密码长度应为 8-20 位")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 验证码（登录时可选）
    /// </summary>
    [StringLength(4, ErrorMessage = "验证码长度不得超过 4 位")]
    public string? VerCode { get; set; }

    /// <summary>
    /// 是否记住登录，默认：false
    /// </summary>
    public bool Remember { get; set; }
}

/// <summary>
/// 用户注册信息模型
/// </summary>
public class MemberRegistParam
{
    /// <summary>
    /// 账户名
    /// </summary>
    [Required(ErrorMessage = "账户名必填")]
    [StringLength(32, MinimumLength = 6, ErrorMessage = "账户名长度应为 6-32 位")]
    [RegularExpression(@"^[\w]+$", ErrorMessage = "账户名只能是字母、数字、下划线组合")]
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 手机号码
    /// </summary>
    [Required(ErrorMessage = "手机号码必填")]
    [RegularExpression(@"^1[\d]{10}$", ErrorMessage = "手机号码格式错误")]
    public string MobilePhone { get; set; } = string.Empty;

    /// <summary>
    /// 登录密码（长度范围 8-20）
    /// </summary>
    [Required(ErrorMessage = "登录密码必填")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "密码长度应为 8-20 位")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 确认密码（长度范围 8-20）
    /// </summary>
    [Required(ErrorMessage = "确认密码必填")]
    [Compare("Password", ErrorMessage = "两次密码不一致")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "密码长度应为 8-20 位")]
    public string RePassword { get; set; } = string.Empty;

    /// <summary>
    /// 用户类型
    /// </summary>
    public MemberType MemberType { get; set; }

    /// <summary>
    /// 年级
    /// </summary>
    public string? Grade { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    [Required(ErrorMessage = "验证码必填")]
    [StringLength(4, ErrorMessage = "验证码长度不得超过 4 位")]
    public string VerCode { get; set; } = string.Empty;
}

/// <summary>
/// 用户密码信息模型
/// </summary>
public class MemberRepwdParam
{
    /// <summary>
    /// 原密码（长度范围 8-20）
    /// </summary>
    [Required(ErrorMessage = "原密码必填")]
    public string OldPwd { get; set; } = string.Empty;

    /// <summary>
    /// 新密码（长度范围 8-20）
    /// </summary>
    [Required(ErrorMessage = "新密码必填")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "密码长度应为 8-20 位")]
    public string NewPwd { get; set; } = string.Empty;

    /// <summary>
    /// 确认密码（长度范围 8-20）
    /// </summary>
    [Required(ErrorMessage = "新密码必填")]
    [Compare(nameof(NewPwd), ErrorMessage = "两次密码不一致")]
    public string RePwd { get; set; } = string.Empty;

    /// <summary>
    /// 验证码
    /// </summary>
    [Required(ErrorMessage = "验证码必填")]
    public string VerCode { get; set; } = string.Empty;
}

/// <summary>
/// 用户编辑参数模型
/// </summary>
public class MemberEditParam
{
    /// <summary>
    /// 性别
    /// </summary>
    public GenderType Gender { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [Required(ErrorMessage = "昵称必填")]
    [StringLength(40, ErrorMessage = "昵称长度不得大于 40")]
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// 头像路由
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// 头像文件
    /// </summary>
    public IFormFile? Avatar { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    [StringLength(128, ErrorMessage = "电子邮箱长度不得大于 128")]
    public string? Email { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public DateOnly? Birthday { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string? MobilePhone { get; set; }
}

/// <summary>
/// 用户新增参数模型
/// </summary>
public class MemberInsertParam
{
    /// <summary>
    /// 账户名
    /// </summary>
    [StringLength(32)]
    [Required(ErrorMessage = "账户名必填")]
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    [StringLength(20)]
    [Required(ErrorMessage = "真实姓名必填")]
    public string RealName { get; set; } = string.Empty;

    /// <summary>
    /// 性别
    /// </summary>
    public GenderType Gender { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [Required(ErrorMessage = "手机号码必填")]
    [RegularExpression(@"^1[\d]{9}[\d]$", ErrorMessage = "手机号码格式错误")]
    public string MobilePhone { get; set; } = string.Empty;

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 用户更改参数模型
/// </summary>
public class MemberUpdateParam : MemberInsertParam
{
    public uint Id { get; set; }
}

/// <summary>
/// 加密信息参数模型
/// </summary>
public class EncryptedInfoParam
{
    /// <summary>
    /// 不包括敏感信息的原始数据字符串
    /// </summary>
    [Required(ErrorMessage = "rawData 无效")]
    public string RawData { get; set; } = string.Empty;

    /// <summary>
    /// 签名
    /// </summary>
    [Required(ErrorMessage = "signature 无效")]
    public string Signature { get; set; } = string.Empty;

    /// <summary>
    /// 加密算法的初始向量（用户信息）
    /// </summary>
    [Required(ErrorMessage = "iv 无效")]
    public string Iv { get; set; } = string.Empty;

    /// <summary>
    /// 用户信息的加密数据（用户信息）
    /// </summary>
    [Required(ErrorMessage = "encryptedData 无效")]
    public string EncryptedData { get; set; } = string.Empty;

    /// <summary>
    /// 注册的用户类型（可选值：学生、家长、教师）
    /// </summary>
    /// <example>学生</example>
    [Required(ErrorMessage = "用户类型必选")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 年级 如果是学生必传
    /// </summary>
    /// <example>一年级</example>
    public string? Grade { get; set; }

    /// <summary>
    /// 是否记住登录状态
    /// </summary>
    /// <example>false</example>
    public bool Remember { get; set; } = false;
}