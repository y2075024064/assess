using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.HashTools;
using Infrastructure.Utilities;
using Assess.Service.Services;
using Assess.Entity;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 用户授权管理控制器
/// </summary>
public class MemberAuthController : AdminApiController<Member, IMemberService>
{
    private readonly AppSettings appSettings;

    public MemberAuthController(
        IOptions<AppSettings> options,
        IMemberService entityService) : base(entityService)
    {
        this.appSettings = options.Value;
    }

    #region 初始化管理员

    [HttpGet]
    [AccreditAction(IgnoreAuth = true)]
    public int InitRoot()
    {
        using var db = entityService.GetDbContext();
        int count = db.Members.Count(e => e.RealName == "超级管理员" && e.Type == MemberType.管理员);
        if (count > 0) { return 0; }

        Member member = new()
        {
            IsDelete = false,
            RealName = "root",
            Type = MemberType.管理员,
            Gender = GenderType.保密,
            CreationTime = DateTime.Now,
            Nickname = "SuperAdministrator",
        };
        db.Members.Add(member);

        string sign = StringHashTool.Signature();
        EncodeType encodeType = EnumHelper.Random(EncodeType.None);
        string password = StringHashTool.Encode("root123456", encodeType, sign);
        MemberAccount account = new()
        {
            Member = member,
            Account = "root",
            Signature = sign,
            Password = password,
            EncodeType = encodeType,
        };
        db.MemberAccounts.Add(account);

        MemberToken token = new()
        {
            Member = member,
            ClientType = ClientType,
            Token = UserToken.NowToken(),
            ExpirationTime = DateTime.Now.AddMinutes(appSettings.TokenDuration),
        };
        db.MemberTokens.Add(token);

        return db.SaveChanges();
    }

    #endregion
}
