namespace Assess.Model.Result;

/// <summary>
/// 用户登录结果模型
/// </summary>
public class MemberLoginResult
{
    /// <summary>
    /// 账户名称
    /// </summary>
    public string? Account { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public GenderType Gender { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string? Nickname { get; set; }

    /// <summary>
    /// 头像路径
    /// </summary>
    public string? Avatar { get; set; }
}

/// <summary>
/// 用户登录结果模型
/// </summary>
public class MemberLoginAllResult : MemberLoginResult
{
    /// <summary>
    /// 用户登录凭证
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// 用户类型
    /// </summary>
    public MemberType MemberType { get; set; }
}

/// <summary>
/// 用户登录验证结果模型
/// </summary>
public class CheckLoginResult : MemberLoginAllResult
{
    /// <summary>
    /// 验证结果信息
    /// </summary>
    public string Msg { get; set; } = string.Empty;

    /// <summary>
    /// 是否验证成功
    /// </summary>
    public bool IsSucceeded { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public long MemberId { get; set; }
}

/// <summary>
/// 用户分页结果模型
/// </summary>
public class MemberPageResult
{
    public uint Id { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string? RealName { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public GenderType Gender { get; set; }

    /// <summary>
    /// 头像路径
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public MemberType Type { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string? MobilePhone { get; set; }

    /// <summary>
    /// 电子邮箱地址
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 是否绑定微信
    /// </summary>
    public bool IsBindWechat { get; set; }

    /// <summary>
    /// 推荐码
    /// </summary>
    public string? InvitationCode { get; set; }

    /// <summary>
    /// 账户创建时间
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

    /// <summary>
    /// 账户名
    /// </summary>
    public string? Account { get; set; }

    /// <summary>
    /// 是否可以更改账户名
    /// </summary>
    public bool CanModifyAccount { get; set; }
}

/// <summary>
/// 用户信息模型
/// </summary>
public class MemberInfoModel
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 账户名
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string? RealName { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public GenderType Gender { get; set; }

    /// <summary>
    /// 头像路径
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public MemberType Type { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public DateOnly? Birthday { get; set; }

    /// <summary>
    /// 是否可以修改账户名
    /// </summary>
    public bool CanModifyAccount { get; set; }
}