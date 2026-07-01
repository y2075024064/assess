using System.ComponentModel.DataAnnotations;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Entity;

namespace Assess.WebApi.Controllers.WebApi;

/// <summary>
/// 用户信息控制器
/// </summary>
public class MemberController : WebApiController<Member, IMemberService>
{
    public MemberController(
        IMemberService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<MemberInfoModel>> Info()
    {
        uint memberId = (uint)CurrentMemberId!;
        RequestResult<MemberInfoModel> result = new();

        var info = await entityService.GetEntityAsync(e => e.Id == memberId, e => new MemberInfoModel()
        {
            Type = e.Type,
            MemberId = e.Id,
            Avatar = e.Avatar,
            Gender = e.Gender,
            Nickname = e.Nickname,
            RealName = e.RealName,
            Birthday = e.Birthday,
            Account = e.MemberAccount!.Account,
            CanModifyAccount = e.MemberAccount!.CanModifyAccount,
        });

        if (info != null)
        {
            info.Avatar = WebFileTool.GetAvatarUrl(info.Avatar);
            result.Data = info;
            result.Success();
        }
        else { result.Fail("当前登录信息丢失，请重新登录！"); }

        return result;
    }

    /// <summary>
    /// 用户修改账户名
    /// </summary>
    /// <param name="param">新的账户名信息</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> EditAccount(ValueParam param)
    {
        RequestResult result = new();

        if (param.Value.IsEmpty()) { result.Fail("账户名不能为空！"); }
        else
        {
            long count = await entityService.Adapter.GetCountAsync<MemberAccount>(e => e.Account == param.Value).NotWaitBack();
            if (count > 0) { result.Fail("该账户名已被使用！"); }
            else
            {
                uint memberId = (uint)CurrentMemberId!;
                count = await entityService.Adapter.UpdateAsync<MemberAccount>(
                    e => e.MemberId == memberId,
                    e => new MemberAccount() { Account = param.Value, CanModifyAccount = false }
                ).NotWaitBack();

                if (count <= 0) { result.Fail("修改失败！"); }
                else { result.Success("修改成功！"); }
            }
        }

        return result;
    }

    /// <summary>
    /// 用户修改昵称
    /// </summary>
    /// <param name="param">新的昵称信息</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> EditNickname(ValueParam param)
    {
        RequestResult result = new();

        if (param.Value.IsEmpty()) { result.Fail("昵称不能为空！"); }
        else
        {
            bool exists = await entityService.ExistsAsync(e => e.Nickname == param.Value).NotWaitBack();
            if (exists) { result.Fail("该昵称已被使用！"); }
            else
            {
                var msg = await EditSingleFieldAsync(e => e.Nickname = param.Value);
                if (msg != null) { result.Fail(msg); }
                else { result.Success("修改成功！"); }
            }
        }

        return result;
    }

    /// <summary>
    /// 用户修改性别
    /// </summary>
    /// <param name="gender">新的性别</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult> EditGender([Required] GenderType gender)
    {
        RequestResult result = new();

        var msg = await EditSingleFieldAsync(e => e.Gender = gender);
        if (msg != null) { result.Fail(msg); }
        else { result.Success("修改成功！"); }

        return result;
    }

    /// <summary>
    /// 用户修改头像
    /// </summary>
    /// <param name="param">新的头像地址信息</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestStrResult> EditAvatar([FromForm] ValueParam param)
    {
        RequestStrResult result = new();

        if (param.Value.IsEmpty()) { result.Fail("头像地址不能为空！"); }
        else
        {
            string? avatar = null;
            var files = Request.Form.Files;
            if (files.Count > 0)
            {
                IFormFile file = files[0];
                if (file.Length > 2097152)
                {
                    result.Fail("头像文件大小不得超过2M");
                    return result;
                }

                avatar = WebFileTool.SaveAvatar(file);
            }
            else
            {
                avatar = await WebFileTool.SaveWechatAvatarAsync(param.Value).NotWaitBack();
            }

            try
            {
                var msg = await EditSingleFieldAsync(e => e.Avatar = avatar);
                if (msg != null) { result.Fail(msg); }
                else
                {
                    result.Data = WebFileTool.GetAvatarUrl(avatar);
                    result.Success("修改成功！");
                }
            }
            catch
            {
                WebFileTool.DeleteAvatar(avatar);
                throw;
            }
        }

        return result;
    }

    /// <summary>
    /// 用户修改生日
    /// </summary>
    /// <param name="param">新的生日（yyyy-MM-dd）</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> EditBirthday(ValueParam param)
    {
        RequestResult result = new();

        if (param.Value.IsEmpty()) { result.Fail("生日不能为空！"); }
        else
        {
            if (!DateOnly.TryParse(param.Value, out DateOnly date)) { result.Fail("生日格式错误！"); }
            else
            {
                var msg = await EditSingleFieldAsync(e => e.Birthday = date);
                if (msg != null) { result.Fail(msg); }
                else { result.Success("修改成功！"); }
            }
        }

        return result;
    }

    /// <summary>
    /// 用户获取收藏分组名称列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<string[]>> GetGroupNames()
    {
        RequestResult<string[]> result = new();
        uint memberId = (uint)CurrentMemberId!;

        var dataList = await entityService.GetListAsync(e => e.Id == memberId,
            e => new SelectNode { Value = e.CollectGroupNames }).NotWaitBack();

        List<string> datas = new();
        if (dataList?.Count > 0)
        {
            var value = (string?)dataList[0].Value;
            if (value.IsNotEmpty())
            {
                var list = value.ToModel<List<string>>();
                if (list != null) { datas = list; }
            }
        }

        datas.Insert(0, "默认");
        result.Data = datas.ToArray();
        result.Success();

        return result;
    }

    /// <summary>
    /// 用户添加收藏分组
    /// </summary>
    /// <param name="name">分组名称</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult> AddGroupName([Required] string name)
    {
        RequestResult result = new();
        uint memberId = (uint)CurrentMemberId!;

        using AssessDBContext db = entityService.GetDbContext();
        var entity = await db.Members.FindAsync(memberId);
        if (entity == null) { result.Fail("您的登录信息丢失，请重新登录"); }
        else
        {
            var value = entity.CollectGroupNames;
            var datas = value?.ToModel<List<string>>();
            if (datas != null) { datas.Add(name); }
            else { datas = new List<string>() { name }; }

            entity.CollectGroupNames = datas.ToJson();
            int count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("添加成功"); }
            else { result.Fail("添加失败"); }
        }

        return result;
    }

    /// <summary>
    /// 异步编辑单个属性
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    private async Task<string?> EditSingleFieldAsync(Action<Member> action)
    {
        string? msg;
        uint memberId = (uint)CurrentMemberId!;

        using var db = entityService.GetDbContext();
        Member? entity = await db.Members.FindAsync(memberId);
        if (entity == null) { msg = "当前登录信息丢失，请重新登录！"; }
        else
        {
            action(entity);
            _ = await db.SaveChangesAsync().NotWaitBack();
            msg = null;
        }

        return msg;
    }
}
