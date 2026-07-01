using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Infrastructure.HashTools;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IMemberService : IBusinessServiceBase<Member>
{
    /// <summary>
    /// 用户登录验证
    /// </summary>
    /// <param name="model">用户登录信息</param>
    /// <param name="clientType">登录客户端类型</param>
    /// <param name="tokenDuration">访问凭证生存时长</param>
    /// <param name="clientIP">登录客户端IP</param>
    /// <param name="token">新的访问凭证</param>
    /// <returns></returns>
    Task<CheckLoginResult> CheckLoginAsync(
        MemberLoginParam model,
        ClientType clientType,
        uint tokenDuration,
        string clientIP,
        string token);
}

public class MemberService : BusinessServiceBase<Member>, IMemberService
{
    public MemberService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }

    public async Task<CheckLoginResult> CheckLoginAsync(
        MemberLoginParam model,
        ClientType clientType,
        uint tokenDuration,
        string clientIP,
        string token)
    {
        string msg = "";
        Member? member = null;
        CheckLoginResult result = new() { IsSucceeded = false };

        using (var db = GetDbContext())
        {
            //查询 用户信息及账户信息
            var account = db.MemberAccounts.Include(e => e.Member).FirstOrDefault(e => e.Account == model.Account);
            if (account == null)
            {
                member = db.Members.Include(e => e.MemberAccount).FirstOrDefault(
                    e => e.MobilePhone == model.Account || e.Email == model.Account
                );
                if (member != null) { account = member.MemberAccount; }
            }
            else { member = account.Member; }

            if (account != null)
            {
                MemberLoginLog loginLog = new()
                {
                    IsPass = false,
                    ClientIp = clientIP,
                    Account = model.Account,
                    ClientType = clientType,
                    CheckTime = DateTime.Now,
                    MemberId = account.MemberId,
                };

                Member memberInfo = member!;
                string password = StringHashTool.Encode(model.Password, account.EncodeType, account.Signature ?? "");
                if (password == account.Password)
                {
                    DateTime nowTime = DateTime.Now;

                    if (account.IsLocked) { msg = "账户已被冻结！"; }
                    else if (account.DisableEndTime != null && account.DisableEndTime > nowTime)
                    {
                        msg = $"账户已被封禁，{account.DisableEndTime:yyyy-MM-dd HH:mm:ss} 结束封禁！";
                    }
                    else
                    {
                        DateTime expirationTime = nowTime.AddMinutes(tokenDuration);

                        //更新用户访问凭证信息
                        var memberToken = db.MemberTokens.FirstOrDefault(e => e.MemberId == memberInfo.Id && e.ClientType == clientType);
                        if (memberToken != null)
                        {
                            memberToken.Token = token;
                            memberToken.IsRemember = model.Remember;
                            memberToken.ExpirationTime = expirationTime;
                        }
                        else
                        {
                            memberToken = new MemberToken()
                            {
                                Token = token,
                                Member = memberInfo,
                                ClientType = clientType,
                                IsRemember = model.Remember,
                                ExpirationTime = expirationTime,
                            };
                            db.MemberTokens.Add(memberToken);
                        }

                        result.IsSucceeded = true;
                        result.Account = model.Account;
                        result.MemberId = memberInfo.Id;
                        result.Avatar = memberInfo.Avatar;
                        result.Gender = memberInfo.Gender;
                        result.MemberType = memberInfo.Type;
                        result.Nickname = memberInfo.Nickname;

                        loginLog.PassTime = nowTime;
                        loginLog.IsPass = true;
                        msg = "登录成功！";
                    }
                }
                else
                {
                    msg = "账户名或密码错误！";
                    loginLog.FailPwd = model.Password;
                    loginLog.Reason = msg;
                }

                db.MemberLoginLogs.Add(loginLog);
            }
            else { msg = "账户名或密码错误！"; }

            _ = await db.SaveChangesAsync().NotWaitBack();

            result.Msg = msg;
        }

        return result;
    }
}