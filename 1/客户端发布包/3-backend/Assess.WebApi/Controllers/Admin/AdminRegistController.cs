using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Infrastructure.HashTools;
using Infrastructure.Utilities;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Entity;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 管理员注册参数
/// </summary>
public class AdminRegistParam
{
    public string Account { get; set; } = "";
    public string Password { get; set; } = "";
    public string MobilePhone { get; set; } = "";
    public string? RealName { get; set; }
    public string? Nickname { get; set; }
    public string? Gender { get; set; }
}

/// <summary>
/// 管理员注册（内部使用，无需验证码）
/// </summary>
[Route("admin/[controller]/[action]")]
[ApiExplorerSettings(GroupName = "admin_v1")]
[AccreditController(MemberType.组织, MemberType.管理员)]
public class AdminRegistController : CheckAuthController<Member, IMemberService>
{
    public AdminRegistController(IMemberService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 创建管理员账户
    /// </summary>
    [HttpPost]
    public async Task<RequestResult> Create([FromBody] AdminRegistParam model)
    {
        RequestResult result = new();

        if (string.IsNullOrWhiteSpace(model.Account))
        {
            result.Fail("账户名不能为空");
            return result;
        }

        using AssessDBContext db = entityService.GetDbContext();

        int count = await db.MemberAccounts.CountAsync(e => e.Account == model.Account).NotWaitBack();
        if (count > 0)
        {
            result.Fail("该账户名已被使用");
            return result;
        }

        count = await db.Members.CountAsync(e => e.MobilePhone == model.MobilePhone).NotWaitBack();
        if (count > 0)
        {
            result.Fail("该手机号已被其他账户绑定");
            return result;
        }

        // 解析性别
        GenderType gender = model.Gender switch
        {
            "男" => GenderType.男,
            "女" => GenderType.女,
            _ => GenderType.保密
        };

        Member member = new()
        {
            IsDelete = false,
            Type = MemberType.管理员,
            Gender = gender,
            RealName = model.RealName,
            CreationTime = DateTime.Now,
            MobilePhone = model.MobilePhone,
            Nickname = !string.IsNullOrWhiteSpace(model.Nickname) ? model.Nickname : CommonTool.CreateNickname(),
        };
        db.Members.Add(member);

        string sign = StringHashTool.Signature();
        EncodeType encodeType = EnumHelper.Random(EncodeType.None);
        string password = StringHashTool.Encode(model.Password, encodeType, sign);
        MemberAccount account = new()
        {
            Member = member,
            Signature = sign,
            Password = password,
            Account = model.Account,
            EncodeType = encodeType,
            CanModifyAccount = false
        };
        db.MemberAccounts.Add(account);

        count = await db.SaveChangesAsync().NotWaitBack();
        if (count > 0) { result.Success("管理员创建成功"); }
        else { result.Fail("创建失败"); }

        return result;
    }
}
