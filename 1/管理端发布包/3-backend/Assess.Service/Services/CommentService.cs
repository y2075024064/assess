using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using System.Linq.Expressions;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;

namespace Assess.Service.Services;

public interface ICommentService : IBusinessServiceBase<CommentInfo>
{
    /// <summary>
    /// 分页获取对象评论列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <param name="memberId">用户ID</param>
    /// <param name="imgFunc">获取图片地址</param>
    /// <param name="avatarFunc">获取头像地址</param>
    /// <returns></returns>
    Task<RequestPageResult<CommentResultInfo>> GetListPagedAsync(CommentSearchParam param, uint memberId, Func<string?, string> imgFunc, Func<string?, string> avatarFunc);

    /// <summary>
    /// 分页获取评论回复列表
    /// </summary>
    /// <param name="id">评论ID</param>
    /// <param name="param">分页参数</param>
    /// <param name="memberId">用户ID</param>
    /// <param name="imgFunc">获取图片地址</param>
    /// <param name="avatarFunc">获取头像地址</param>
    /// <returns></returns>
    Task<RequestPageResult<CommentResultInfo>> GetChildrenAsync(uint id, PageBaseParam param, uint memberId, Func<string?, string> imgFunc, Func<string?, string> avatarFunc);
}

public class CommentService : BusinessServiceBase<CommentInfo>, ICommentService
{
    public CommentService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }

    /// <summary>
    /// 分页获取对象评论列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <param name="memberId">用户ID</param>
    /// <param name="imgFunc">获取图片地址</param>
    /// <param name="avatarFunc">获取头像地址</param>
    /// <returns></returns>
    public async Task<RequestPageResult<CommentResultInfo>> GetListPagedAsync(CommentSearchParam param, uint memberId, Func<string?, string> imgFunc, Func<string?, string> avatarFunc)
    {
        List<CommentResultInfo> infos = new();
        RequestPageResult<CommentResultInfo> result = new();

        Expression<Func<CommentInfo, bool>> where = e =>
            e.IsDelete == false && e.ObjectId == param.ObjectId;

        using AssessDBContext db = GetDbContext();
        long count = db.CommentInfos.LongCount(where);
        if (count > 0)
        {
            where = where.AndAlso(e => e.ObjectType == param.ObjectType);
            var query = db.CommentInfos.Where(where).OrderByDescending(e => e.IsTop);
            if (param.OrderType == "hot")
            {
                query = query.ThenByDescending(e => e.LikeNum)
                    .ThenByDescending(e => e.CreationTime);
            }
            else
            {
                query = query.ThenByDescending(e => e.CreationTime)
                    .ThenByDescending(e => e.LikeNum);
            }

            var list = await query.Skip(param.Skip).Take(param.Take)
                .ToListAsync().NotWaitBack();

            var memberIds = list.Select(e => e.MemberId).Distinct();
            var memberInfos = await db.Members.Where(e => memberIds.Contains(e.Id))
                .Select(e => new Member { Id = e.Id, Nickname = e.Nickname, Avatar = e.Avatar, Type = e.Type })
                .ToListAsync().NotWaitBack();

            var objIds = list.Select(e => e.Id);
            var likeList = await db.LikeRecords.Where(e =>
                e.MemberId == memberId && e.ObjectType == param.ObjectType && objIds.Contains(e.ObjectId))
                .ToListAsync().NotWaitBack();

            foreach (var item in list)
            {
                CommentResultInfo info = new()
                {
                    Id = item.Id,
                    IsTop = item.IsTop,
                    Content = item.Content,
                    LikeNum = item.LikeNum,
                    MemberId = item.MemberId,
                    CreationTime = item.CreationTime,
                    IsLike = likeList.Exists(e => e.ObjectId == item.Id),
                };

                if (!string.IsNullOrEmpty(item.Images))
                {
                    string[] temp = item.Images.Split(',');
                    info.Images = new string[temp.Length];
                    for (int i = 0; i < temp.Length; i++)
                    {
                        info.Images[i] = imgFunc.Invoke(temp[i]);
                    }
                }

                var member = memberInfos.Find(e => e.Id == item.MemberId);
                if (member != null)
                {
                    info.Avatar = avatarFunc.Invoke(member.Avatar);
                    info.Nickname = member.Nickname;
                    info.MemberType = member.Type;
                }

                PageBaseParam pageParam = new() { Page = 1, Take = 3 };
                RequestPageResult<CommentResultInfo> childPage = await GetChildrenAsync(item.Id, pageParam, db, memberId, imgFunc, avatarFunc);
                info.Children = childPage.Data ?? Array.Empty<CommentResultInfo>();
                info.ChildrenTotal = childPage.Total;

                infos.Add(info);
            }
        }

        result.Data = infos.ToArray();
        result.Total = count;
        result.Success();

        return result;
    }

    /// <summary>
    /// 分页获取评论回复列表
    /// </summary>
    /// <param name="id">评论ID</param>
    /// <param name="param">分页参数</param>
    /// <param name="memberId">用户ID</param>
    /// <param name="imgFunc">获取图片地址</param>
    /// <param name="avatarFunc">获取头像地址</param>
    /// <returns></returns>
    public Task<RequestPageResult<CommentResultInfo>> GetChildrenAsync(uint id, PageBaseParam param, uint memberId, Func<string?, string> imgFunc, Func<string?, string> avatarFunc)
    {
        using AssessDBContext db = GetDbContext();
        return GetChildrenAsync(id, param, db, memberId, imgFunc, avatarFunc);
    }

    private static async Task<RequestPageResult<CommentResultInfo>> GetChildrenAsync(uint id, PageBaseParam param, AssessDBContext db, uint memberId, Func<string?, string> imgFunc, Func<string?, string> avatarFunc)
    {
        List<CommentResultInfo> infos = new();
        RequestPageResult<CommentResultInfo> result = new();

        result.Total = await db.CommentInfos.LongCountAsync(e =>
            e.IsDelete == false && e.CommentId == id && e.ObjectType == ObjectType.Comment);
        if (result.Total > 0)
        {
            var list = await db.CommentInfos.Where(e =>
                e.IsDelete == false && e.CommentId == id && e.ObjectType == ObjectType.Comment)
                .OrderBy(e => e.CreationTime).Skip(param.Skip).Take(param.Take)
                .ToListAsync().NotWaitBack();

            var memberIds = list.Select(e => e.MemberId).Distinct();
            var memberInfos = await db.Members.Where(e => memberIds.Contains(e.Id))
                .Select(e => new Member { Id = e.Id, Nickname = e.Nickname, Avatar = e.Avatar, Type = e.Type })
                .ToListAsync().NotWaitBack();

            var objectIds = list.Select(e => e.Id);
            List<LikeRecord> likeList = await db.LikeRecords.Where(e =>
                e.MemberId == memberId && e.ObjectType == ObjectType.Comment && objectIds.Contains(e.ObjectId))
                .ToListAsync().NotWaitBack();

            foreach (var item in list)
            {
                CommentResultInfo info = new()
                {
                    Id = item.Id,
                    Content = item.Content,
                    LikeNum = item.LikeNum,
                    MemberId = item.MemberId,
                    CreationTime = item.CreationTime,
                    IsLike = likeList.Exists(e => e.ObjectId == item.Id),
                };

                if (!string.IsNullOrEmpty(item.Images))
                {
                    string[] temp = item.Images.Split(',');
                    info.Images = new string[temp.Length];
                    for (int i = 0; i < temp.Length; i++)
                    {
                        info.Images[i] = imgFunc.Invoke(temp[i]);
                    }
                }

                var member = memberInfos.Find(e => e.Id == item.MemberId);
                if (member != null)
                {
                    info.Avatar = avatarFunc.Invoke(member.Avatar);
                    info.Nickname = member.Nickname;
                    info.MemberType = member.Type;
                }

                var comment = list.Find(e => e.Id == item.CommentId);
                if (comment != null)
                {
                    member = memberInfos.Find(e => e.Id == comment.MemberId);
                    if (member != null)
                    {
                        info.ReplyNickname = member.Nickname;
                        info.ReplyMemberId = member.Id;
                    }
                }

                infos.Add(info);
            }
        }

        result.Data = infos.ToArray();
        result.Success();

        return result;
    }
}
