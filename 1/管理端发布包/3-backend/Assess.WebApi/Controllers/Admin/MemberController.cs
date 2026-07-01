using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Infrastructure.HashTools;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Repository;
using Mapster;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 用户管理控制器
/// </summary>
public class MemberController : AdminApiController<Member, IMemberService>
{
    public MemberController(
        IMemberService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 获取用户登录信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<MemberLoginResult>> GetInfo()
    {
        RequestResult<MemberLoginResult> result = new();

        var entity = await entityService.GetEntityAsync(e => e.Id == CurrentMemberId!, nameof(Member.MemberAccount)).NotWaitBack();
        if (entity == null) { result.Fail("您要获取的用户不存在"); }
        else
        {
            result.Data = new MemberLoginResult()
            {
                Gender = entity.Gender,
                Nickname = entity.Nickname,
                Account = entity.MemberAccount!.Account,
                Avatar = WebFileTool.GetAvatarUrl(entity.Avatar),
            };
            result.Success();
        }

        return result;
    }

    /// <summary>
    /// 获取用户编辑信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<MemberEditParam>> GetEditInfo()
    {
        RequestResult<MemberEditParam> result = new();

        var model = await entityService.GetEntityAsync(e => e.Id == CurrentMemberId, e => new MemberEditParam
        {
            Email = e.Email,
            Gender = e.Gender,
            AvatarUrl = e.Avatar,
            Birthday = e.Birthday,
            Nickname = e.Nickname,
            MobilePhone = e.MobilePhone,
        }).NotWaitBack();
        if (model == null) { result.Fail("您的登录信息已丢失请重新登录"); }
        else
        {
            model.AvatarUrl = WebFileTool.GetAvatarUrl(model.AvatarUrl);
            result.Data = model;
            result.Success();
        }

        return result;
    }

    /// <summary>
    /// 用户编辑信息
    /// </summary>
    /// <param name="param">用户信息</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> EditInfo([FromForm] MemberEditParam param)
    {
        string avatarPath = "";
        RequestResult result = new();

        try
        {
            using AssessDBContext db = entityService.GetDbContext();
            var entity = await db.Members.FindAsync(CurrentMemberId!).ConfigureAwait(false);
            if (entity == null) { result.Msg = "您的账户信息丢失，请联系管理员"; }
            else
            {
                int count = 0;
                bool hasChange = false;
                if (entity.Nickname != param.Nickname)
                {
                    count = await db.Members.CountAsync(e => e.Nickname == param.Nickname).NotWaitBack();
                    if (count > 0)
                    {
                        result.Fail("该昵称重复！");
                        return result;
                    }
                    else
                    {
                        entity.Nickname = param.Nickname;
                        hasChange = true;
                    }
                }

                hasChange = hasChange || entity.ChangeField(param,
                    nameof(MemberEditParam.Email),
                    nameof(MemberEditParam.Avatar),
                    nameof(MemberEditParam.MobilePhone));

                string? oldImgName = "";
                if (param.Avatar != null)
                {
                    avatarPath = WebFileTool.SaveAvatar(param.Avatar);
                    oldImgName = entity.Avatar;
                    entity.Avatar = avatarPath;
                    hasChange = true;
                }

                if (hasChange)
                {
                    entity.ModificationTime = DateTime.Now;
                    count = await db.SaveChangesAsync().NotWaitBack();
                    if (count > 0)
                    {
                        WebFileTool.DeleteAvatar(oldImgName);
                        result.Success("保存成功");
                    }
                    else
                    {
                        WebFileTool.DeleteAvatar(avatarPath);
                        result.Fail("保存失败");
                    }
                }
                else { result.Fail("无更改项"); }
            }
        }
        catch
        {
            WebFileTool.DeleteAvatar(avatarPath);
            throw;
        }

        return result;
    }

    /// <summary>
    /// 分页获取用户列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<MemberPageResult>> Page(MemberPageParam param)
    {
        RequestPageResult<MemberPageResult> result = new();
        EntityPageModel<Member> pageModel = new(param)
        {
            Where = e => e.IsDelete == false &&
                (param.Gender == null || e.Gender == param.Gender) &&
                (param.EndTime == null || e.CreationTime <= param.EndTime) &&
                (param.StartTime == null || e.CreationTime >= param.StartTime) &&
                (string.IsNullOrWhiteSpace(param.MobilePhone) || e.MobilePhone == param.MobilePhone) &&
                (string.IsNullOrWhiteSpace(param.InvitationCode) || e.InvitationCode == param.InvitationCode) &&
                (string.IsNullOrWhiteSpace(param.Keyword) ||
                 e.Remark!.Contains(param.Keyword) ||
                 e.Nickname.Contains(param.Keyword) ||
                 e.RealName!.Contains(param.Keyword) ||
                 e.MemberAccount!.Account == param.Keyword)
        };
        if (param.Type == null) { pageModel.Where = pageModel.Where.AndAlso(e => e.Type != MemberType.管理员); }
        else { pageModel.Where = pageModel.Where.AndAlso(e => e.Type == param.Type); }

        var page = await entityService.GetPageAsync(e => new MemberPageResult
        {
            Id = e.Id,
            Gender = e.Gender,
            Remark = e.Remark,
            RealName = e.RealName,
            Nickname = e.Nickname,
            MobilePhone = e.MobilePhone,
            CreationTime = e.CreationTime,
            InvitationCode = e.InvitationCode,
            Account = e.MemberAccount!.Account,
            ModificationTime = e.ModificationTime,
            Avatar = WebFileTool.GetAvatarUrl(e.Avatar),
            CanModifyAccount = e.MemberAccount.CanModifyAccount,
        }, pageModel).NotWaitBack();
        if (page != null) { result.LoadPage(page); }

        return result;
    }

    /// <summary>
    /// 新增用户信息
    /// </summary>
    /// <param name="param">新增的用户信息参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert(MemberInsertParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();
        int count = await db.MemberAccounts.CountAsync(e => e.Account == param.Account && e.Member!.IsDelete == true).NotWaitBack();
        if (count > 0) { result.Fail("该账户名重复"); }
        else
        {
            Member entity = param.Adapt<Member>();
            entity.Nickname = CommonTool.CreateNickname();
            entity.CreationTime = DateTime.Now;
            entity.Type = MemberType.管理员;

            db.Members.Add(entity);

            string sign = StringHashTool.Signature();
            EncodeType encodeType = EnumHelper.Random(EncodeType.None);
            string pwd = StringHashTool.Encode("admin123456", encodeType, sign);
            MemberAccount account = new()
            {
                Password = pwd,
                Member = entity,
                Signature = sign,
                Account = param.Account,
                EncodeType = encodeType,
                CanModifyAccount = false,
            };

            db.MemberAccounts.Add(account);

            count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("新增成功，默认密码：admin123456"); }
            else { result.Fail("新增失败"); }
        }

        return result;
    }

    /// <summary>
    /// 更改用户信息
    /// </summary>
    /// <param name="param">更改的用户信息参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update(MemberUpdateParam param)
    {
        int count = 0;
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();
        var entity = await db.Members.Include(e => e.MemberAccount).SingleOrDefaultAsync(e => e.Id == param.Id);
        if (entity == null) { result.Fail("您要修改的用户不存在"); }
        else
        {
            bool hasChange = false;
            if (entity.MemberAccount!.CanModifyAccount && param.Account != entity.MemberAccount!.Account)
            {
                count = await db.MemberAccounts.CountAsync(e => e.Account == param.Account && e.Member!.IsDelete == true).NotWaitBack();
                if (count > 0)
                {
                    result.Fail("该账户名重复");
                    return result;
                }
                else
                {
                    entity.MemberAccount.Account = param.Account;
                    hasChange = true;
                }
            }

            hasChange = hasChange || entity.ChangeField(param);
            if (hasChange)
            {
                entity.ModificationTime = DateTime.Now;

                count = await db.SaveChangesAsync().NotWaitBack();
                if (count > 0) { result.Success("更改成功"); }
                else { result.Fail("更改失败"); }
            }
            else { result.Fail("无更改项"); }
        }

        return result;
    }

    /// <summary>
    /// 删除用户信息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete(uint id)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();
        var entity = await db.Members.FindAsync(id);
        if (entity == null) { result.Fail("您要删除的用户不存在"); }
        else if (entity.Type != MemberType.管理员) { result.Fail("该账户无权删除"); }
        else
        {
            entity.IsDelete = true;

            int count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("删除成功"); }
            else { result.Fail("删除失败"); }
        }

        return result;
    }

    /// <summary>
    /// 封锁用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns></returns>
    [HttpGet]
    public Task<RequestResult> Lock(uint id)
    {
        return UnLockOrLockAsync(id, true);
    }

    /// <summary>
    /// 解除封锁
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns></returns>
    [HttpGet]
    public Task<RequestResult> UnLock(uint id)
    {
        return UnLockOrLockAsync(id, false);
    }

    /// <summary>
    /// 解除或者封锁用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <param name="isLock">是否封锁</param>
    /// <returns></returns>
    private async Task<RequestResult> UnLockOrLockAsync(uint id, bool isLock)
    {
        RequestResult result = new();
        string type = isLock ? "封锁" : "解除";

        int count = await entityService.Adapter.UpdateAsync<MemberAccount>(
            e => e.MemberId == id, e => new MemberAccount { IsLocked = isLock }).NotWaitBack();
        if (count > 0) { result.Success($"{type}成功"); }
        else { result.Fail($"{type}失败"); }

        return result;
    }

    /// <summary>
    /// 获取用户详细信息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<object>> Detail(uint id)
    {
        RequestResult<object> result = new();

        var entity = await entityService.GetEntityAsync(e => e.Id == id, nameof(Member.MemberAccount)).NotWaitBack();
        if (entity == null) { result.Fail("您要获取的用户不存在"); }
        else
        {
            result.Success();
            result.Data = new
            {
                entity.Id,
                entity.Type,
                entity.Email,
                entity.Remark,
                entity.Avatar,
                entity.Gender,
                entity.RealName,
                entity.Nickname,
                entity.Birthday,
                entity.MobilePhone,
                entity.CreationTime,
                entity.IsBindWechat,
                entity.InvitationCode,
                entity.ModificationTime,
                entity.MemberAccount?.Account,
                entity.MemberAccount?.IsLocked,
                entity.MemberAccount?.DisableEndTime,
                entity.MemberAccount?.DisableStartTime,
            };
        }

        return result;
    }

    /// <summary>
    /// 根据账户名称获取用户ID
    /// </summary>
    /// <param name="q"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<SelectResult> Select(string? q)
    {
        SelectResult result = new();

        if (string.IsNullOrEmpty(q)) { result.Data = Array.Empty<SelectNode>(); }
        else
        {
            var list = await entityService.Adapter.GetListAsync<MemberAccount, SelectNode>(
                e => e.Account.Contains(q),
                e => new SelectNode { Text = e.Account, Value = e.MemberId }
            ).NotWaitBack();
            result.Data = list.ToArray();
        }
        result.Success();

        return result;
    }
}
