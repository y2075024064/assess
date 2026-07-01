using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Utilities;
using System.Linq.Expressions;
using Assess.Service.Services;
using Assess.Model.Result;
using WeChatTool.Business;
using Assess.Model.Param;
using WeChatTool.JSSDK;
using WeChatTool.Lib;
using Assess.Entity;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 前台用户授权相关接口
/// </summary>
public class MemberAuthController : WebApiController<MemberAccount, IMemberAccountService>
{
    private readonly WeChatUserInfo weChatUserInfo;

    private readonly AppSettings appSettings;

    public MemberAuthController(
        IWXCacheManage cacheManage,
        IOptions<AppSettings> appSettings,
        IMemberAccountService entityService) : base(entityService)
    {
        this.appSettings = appSettings.Value;
        weChatUserInfo = new WeChatUserInfo(cacheManage, this.appSettings.AppID, this.appSettings.AppSecret);
    }

    /// <summary>
    /// 小程序登录
    /// </summary>
    /// <param name="param">用户登录凭证 code 信息</param>
    /// <remarks>
    /// 1、该 code 为调用小程序 login 方法后返回的 code ；
    /// 2、返回的 code 为 200 表示服务器登录成功；返回 420 表示需要注册，调用 小程序用户注册 接口；
    /// </remarks>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult<MemberLoginResult>> OnLogin(WinXinCodeParam param)
    {
        RequestResult<MemberLoginResult> result = new();

        var info = await weChatUserInfo.LoginMiniByCodeAsync(param.Code);
        if (!info.IsSucc)
        {
            result.Msg = $"errcode: {info.errcode}, errmsg: {info.errmsg}";
        }
        else
        {
            Expression<Func<MemberAccount, bool>>? where = null;
            if (!string.IsNullOrEmpty(info.unionid))
            {
                where = e => e.UnionId == info.unionid;
            }
            else if (!string.IsNullOrEmpty(info.openid))
            {
                where = e => e.MiniOpenId == info.openid;
            }

            if (where != null)
            {
                Session.Set("SessionKey", info.session_key);
                using var db = entityService.GetDbContext();
                MemberAccount? account = await db.MemberAccounts.Include(e => e.Member).FirstOrDefaultAsync(where);
                if (account != null)
                {
                    var _result = await LoginAsync(account, db, param.Remember);
                    result.Code = _result.Code;
                    result.Data = _result.Data;
                    result.Msg = _result.Msg;
                }
                else
                {
                    result.Code = (uint)ResultCode.NotRegist;
                    result.Msg = "您当前未注册账号！";
                }
            }
            else { result.Msg = "登录失败，微信端返回信息不全"; }
        }

        return result;
    }

    /// <summary>
    /// 小程序用户注册
    /// </summary>
    /// <param name="param">用户信息参数</param>
    /// <remarks>
    /// 1、传入的用户信息参数来源于调用小程序 getUserInfo 方法；
    /// 2、如果返回的 token 有值则更新当前保存的访问凭证信息；
    /// 3、grade 参数值的来源通过请求 api/Grade/Select 接口获取；
    /// </remarks>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult<MemberLoginResult>> Regist(EncryptedInfoParam param)
    {
        RequestResult<MemberLoginResult> result = new();

        var types = new[] { "学生", "家长", "教师" };
        if (!types.Contains(param.Type))
        {
            result.Msg = "用户类型不合法！";
            return result;
        }
        MemberType type = Enum.Parse<MemberType>(param.Type);

        if (type == MemberType.学生 && param.Grade.IsEmpty())
        {
            result.Msg = "学生类型时年级必选！";
            return result;
        }

        string? key = Session.GetString("SessionKey");
        if (key.IsEmpty())
        {
            result.Msg = "注册失败，登录过期！";
            return result;
        }

        string sign = new Signature().Sign(param.RawData + key);
        if (sign != param.Signature)
        {
            result.Msg = "签名验证失败！";
            return result;
        }

        var data = WeChatUserInfo.GetInfoByEncryptedData(param.Iv, key!, param.EncryptedData);
        if (data == null)
        {
            result.Msg = "用户信息解密失败！";
            return result;
        }

        if (!TimeStampTool.ValidateTimestamp(data.watermark.timestamp, 90, DateTimeKind.Utc))
        {
            result.Msg = "时间戳验证失败！";
            return result;
        }

        Expression<Func<MemberAccount, bool>> where;
        if (!data.unionId.IsEmpty()) { where = e => e.UnionId == data.unionId; }
        else { where = e => e.MiniOpenId == data.openId; }

        using var db = entityService.GetDbContext();
        MemberAccount? account = await db.MemberAccounts.Include(e => e.Member).FirstOrDefaultAsync(where);
        if (account != null)
        {
            var result_ = await LoginAsync(account, db, param.Remember);
            result.Code = result_.Code;
            result.Data = result_.Data;
            result.Msg = result_.Msg;
            return result;
        }

        if (data.nickName.IsEmpty())
        {
            data.nickName = CommonTool.CreateNickname();
        }

        string? avatar = await WebFileTool.SaveWechatAvatarAsync(data.avatarUrl).NotWaitBack();
        Member member = new()
        {
            Type = type,
            Avatar = avatar,
            IsBindWechat = true,
            Nickname = data.nickName,
            CreationTime = DateTime.Now,
            Gender = (GenderType)data.gender,
        };
        db.Members.Add(member);

        if (type == MemberType.学生)
        {
            StudentInfo studentInfo = new()
            {
                Member = member,
                RegistGrade = param.Grade!,
                CurrentGrade = param.Grade!,
            };
            db.StudentInfos.Add(studentInfo);
        }

        account = new MemberAccount()
        {
            Password = "",
            Signature = "",
            Member = member,
            UnionId = data.unionId,
            CanModifyAccount = true,
            MiniOpenId = data.openId,
            Account = CommonTool.CreateAccount(),
        };
        db.MemberAccounts.Add(account);

        var _result = await LoginAsync(account, db, true);
        result.Code = _result.Code;
        result.Data = _result.Data;
        result.Msg = _result.Msg;

        return result;
    }

    /// <summary>
    /// 绑定手机号
    /// </summary>
    /// <param name="param">Code信息</param>
    /// <remarks>
    /// code 来源参考：https://developers.weixin.qq.com/miniprogram/dev/framework/open-ability/getPhoneNumber.html
    /// </remarks>
    /// <returns></returns>
    [HttpPost]
    [AccreditAction]
    public async Task<RequestResult> PhoneBind(ValueParam param)
    {
        RequestResult result = new();

        var info = await weChatUserInfo.GetUserPhonenumberAsync(param.Value);
        if (!info.IsSucc)
        {
            result.Msg = $"errcode: {info.errcode}, errmsg: {info.errmsg}";
        }
        else
        {
            string phoneNumber = info.phone_info.purePhoneNumber.Trim();
            using var db = entityService.GetDbContext();
            Member? entity = await db.Members.FindAsync(CurrentMemberId);
            if (entity != null)
            {
                entity.MobilePhone = phoneNumber;
                int count = await db.SaveChangesAsync().NotWaitBack();

                if (count > 0) { result.Success("绑定成功！"); }
                else { result.Msg = "绑定失败！"; }
            }
            else { result.Msg = "您的登录信息已丢失，请重新登录！"; }
        }

        return result;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="account"></param>
    /// <param name="db"></param>
    /// <param name="remember"></param>
    /// <param name="isBind"></param>
    /// <returns></returns>
    private async Task<RequestResult<MemberLoginResult>> LoginAsync(MemberAccount account, AssessDBContext db, bool remember, bool isBind = false)
    {
        RequestResult<MemberLoginResult> result = new();

        string? clientIP = ClientIP;
        DateTime nowTime = DateTime.Now;
        ClientType type = ClientType.Mini;
        DateTime expirationTime = nowTime.AddMinutes(appSettings.TokenDuration);

        if (isBind)
        {
            int count = await db.SaveChangesAsync().NotWaitBack();
            if (count <= 0)
            {
                result.Msg = "注册失败！";
                return result;
            }
        }

        Member member = account.Member!;

        MemberToken? token = await db.MemberTokens.FirstOrDefaultAsync(e => e.MemberId == member.Id && e.ClientType == type);
        if (token != null)
        {
            token.IsRemember = remember;
            token.Token = Session.SessionId;
            token.ExpirationTime = expirationTime;
        }
        else
        {
            token = new MemberToken()
            {
                Member = member,
                ClientType = type,
                IsRemember = remember,
                Token = Session.SessionId,
                ExpirationTime = expirationTime,
            };
            db.MemberTokens.Add(token);
        }

        MemberLoginLog loginLog = new()
        {
            IsPass = true,
            ClientType = type,
            PassTime = nowTime,
            CheckTime = nowTime,
            ClientIp = clientIP,
            Reason = "登录成功",
            MemberId = member.Id,
            Account = account.Account,
        };
        db.MemberLoginLogs.Add(loginLog);

        if (await db.SaveChangesAsync().NotWaitBack() > 0)
        {
            result.Success("登录成功");
            result.Data = new MemberLoginResult()
            {
                Gender = member.Gender,
                Account = account.Account,
                Nickname = member.Nickname,
                Avatar = WebFileTool.GetAvatarUrl(member.Avatar),
            };

            Session.Set(new Dictionary<string, object?> {
                { "MiniOpenId", account.MiniOpenId },
                { memberIdKey, account.MemberId },
                { memberTypeKey, member.Type },
            });
        }
        else { result.Msg = "登录失败"; }

        return result;
    }
}
