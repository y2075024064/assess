namespace Assess;

/// <summary>
/// 验证码类型
/// </summary>
public enum VerCodeType : byte
{
    All = 0,

    Regist = 10,

    Login = 20,

    RePassword = 30,

    BindPhone = 40,

    BindEmail = 50,
}