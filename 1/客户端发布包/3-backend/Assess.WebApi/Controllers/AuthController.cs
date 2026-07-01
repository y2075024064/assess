using System.ComponentModel.DataAnnotations;
using SixLabors.ImageSharp.Formats.Png;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Infrastructure.VerCodeImage;
using Infrastructure.Extensions;
using Infrastructure.HashTools;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using SixLabors.ImageSharp;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Serilog;

namespace Assess.WebApi.Controllers;

/// <summary>
/// 用户授权相关
/// </summary>
[Route("[controller]/[action]")]
public class AuthController : CheckAuthController<Member, IMemberService>
{
    private readonly AppSettings appSettings;

    public AuthController(
        IOptions<AppSettings> options,
        IMemberService entityService) : base(entityService)
    {
        this.appSettings = options.Value;
    }

    /// <summary>
    /// 获取图片验证码
    /// </summary>
    /// <param name="codeType">验证码类型</param>
    /// <param name="bgb">是否黑色背景</param>
    /// <returns></returns>
    [HttpGet]
    public Task<FileResult> VerCode([Required] VerCodeType codeType, bool bgb = false)
    {
        return Task.Run<FileResult>(() => File(CreateVerCode(codeType, bgb), "image/gif"));
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="model">登录信息</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult<MemberLoginAllResult>> Login(MemberLoginParam model)
    {
        RequestResult<MemberLoginAllResult> result = new();

        string token = UserToken.NowToken();
        uint tokenD = appSettings.TokenDuration;
        CheckLoginResult checkResult = await entityService.CheckLoginAsync(model, ClientType, tokenD, ClientIP ?? "", token);
        if (checkResult.IsSucceeded)
        {
            result.Data = new MemberLoginAllResult()
            {
                Token = token,
                Gender = checkResult.Gender,
                Account = checkResult.Account,
                Nickname = checkResult.Nickname,
                MemberType = checkResult.MemberType,
                Avatar = WebFileTool.GetAvatarUrl(checkResult.Avatar),
            };
            result.Success();

            Session.Clear(token);
            Session.Set(new Dictionary<string, object?> {
                { memberIdKey, checkResult.MemberId },
                { memberTypeKey, checkResult.MemberType },
            });
        }
        result.Msg = checkResult.Msg;

        return result;
    }

    /// <summary>
    /// 检查账号是否已被使用
    /// </summary>
    /// <param name="account">要检查的账号</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult> CheckAccount(string account)
    {
        RequestResult result = new();

        if (account.IsEmpty()) { result.Fail("账号不能为空"); }
        else
        {
            using AssessDBContext db = entityService.GetDbContext();
            bool exists = await db.MemberAccounts.AnyAsync(e => e.Account == account).NotWaitBack();
            if (exists) { result.Fail("该账户名已被使用"); }
            else { result.Success("账号可用"); }
        }

        return result;
    }

    /// <summary>
    /// 检查手机号是否已被绑定
    /// </summary>
    /// <param name="mobilePhone">要检查的手机号</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult> CheckMobilePhone(string mobilePhone)
    {
        RequestResult result = new();

        if (mobilePhone.IsEmpty()) { result.Fail("手机号不能为空"); }
        else
        {
            using AssessDBContext db = entityService.GetDbContext();
            bool exists = await db.Members.AnyAsync(e => e.MobilePhone == mobilePhone).NotWaitBack();
            if (exists) { result.Fail("该手机号已被其他账户绑定"); }
            else { result.Success("手机号可用"); }
        }

        return result;
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="model">注册信息</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Regist(MemberRegistParam model)
    {
        RequestResult result = new();

        if (model.MemberType == MemberType.学生 && model.Grade.IsEmpty())
        {
            result.Fail("当前年级必选");
            return result;
        }

        string? msg = CheckVerCode(model.VerCode, VerCodeType.Regist);
        if (msg == null)
        {
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

            Member member = new()
            {
                IsDelete = false,
                Type = model.MemberType,
                Gender = GenderType.保密,
                CreationTime = DateTime.Now,
                MobilePhone = model.MobilePhone,
                Nickname = CommonTool.CreateNickname(),
            };
            db.Members.Add(member);

            if (model.MemberType == MemberType.学生)
            {
                StudentInfo studentInfo = new()
                {
                    Member = member,
                    RegistGrade = model.Grade!,
                    CurrentGrade = model.Grade!,
                };
                db.StudentInfos.Add(studentInfo);
            }

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
            if (count > 0) { result.Success("注册成功"); }
            else { result.Fail("注册失败"); }
        }
        else { result.Fail(msg); }

        return result;
    }

    /// <summary>
    /// 用户退出登录
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AccreditAction]
    public async Task<RequestResult> Logout()
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();
        var token = await db.MemberTokens.FirstOrDefaultAsync(e => e.MemberId == CurrentMemberId!).NotWaitBack();
        if (token == null) { result.Fail("您未登录不能退出"); }
        else
        {
            token.ExpirationTime = DateTime.Now;
            token.IsRemember = false;

            int count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("退出成功"); }
            else { result.Fail("退出失败"); }
        }

        return result;
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="param">密码信息</param>
    /// <returns></returns>
    [HttpPost]
    [AccreditAction]
    public async Task<RequestResult> Repwd(MemberRepwdParam param)
    {
        RequestResult result = new();

        string? msg = CheckVerCode(param.VerCode, VerCodeType.RePassword);
        if (msg == null)
        {
            using AssessDBContext db = entityService.GetDbContext();
            var account = await db.MemberAccounts.FindAsync(CurrentMemberId!).ConfigureAwait(false);
            if (account != null)
            {
                string sign = account.Signature!;
                EncodeType encodeType = account.EncodeType;
                string password = StringHashTool.Encode(param.OldPwd, encodeType, sign);
                if (account.Password == password)
                {
                    sign = StringHashTool.Signature();
                    encodeType = EnumHelper.Random(EncodeType.None);
                    password = StringHashTool.Encode(param.NewPwd, encodeType, sign);

                    account.Signature = sign;
                    account.Password = password;
                    account.EncodeType = encodeType;

                    int count = await db.SaveChangesAsync().NotWaitBack();
                    if (count > 0) { result.Success("密码修改成功！"); }
                    else { result.Fail("密码修改失败！"); }
                }
                else { result.Fail("原密码不正确！"); }
            }
            else { result.Fail("您的登录信息丢失，请重新登录！"); }
        }
        else { result.Fail(msg); }

        return result;
    }

    #region 工具方法

    private byte[] CreateVerCode(VerCodeType codeType, bool isBlackBg)
    {
        MemoryStream stream = new();

        CharGroup charGroup = new(4, 125, 40)
        {
            InterferingLine = true,
            InterferingSpot = true,
        };
        if (isBlackBg)
        {
            charGroup.BackColor = Color.Parse("#0d182a");
            charGroup.ArryColor = new[] { Color.White, Color.Red, Color.Yellow, Color.Green, Color.LightGray, Color.Cyan };
        }

        Image image = charGroup.CreateSingleVercodeImg();
        image.Save(stream, new PngEncoder());

        long ticks = DateTime.Now.AddSeconds(appSettings.VerCodeDuration).Ticks;
        string verCode = charGroup.ValidateStr!.ToLower();
        Session.Set($"{codeType}_VerCode", $"{verCode}{ticks}");

        Log.Information(stream.Length.ToString());
        return stream.GetBuffer();
    }

    private string? CheckVerCode(string verCode, VerCodeType codeType)
    {
        verCode = verCode.ToLower();
        var verCodeString = Session.Get($"{codeType}_VerCode")?.ToString();
        if (verCodeString == null) { return "请先获取验证码！"; }
        else
        {
            string _verCode = verCodeString[..4];
            if (verCode != _verCode) { return "验证码错误！"; }

            string ticks = verCodeString[4..];
            DateTime dateTime = new(long.Parse(ticks));
            if (dateTime < DateTime.Now) { return "该验证码已失效！"; }

            return null;
        }
    }

    #endregion
}
