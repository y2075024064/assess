using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Entity;

namespace Assess.WebApi.Controllers.WebApi
{
    /// <summary>
    /// 点赞相关接口
    /// </summary>
    public class LikeController : WebApiController<LikeRecord, ILikeService>
    {
        public LikeController(ILikeService entityService) : base(entityService)
        {
        }

        /// <summary>
        /// 用户点赞某个对象
        /// </summary>
        /// <param name="objectId">对象ID</param>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<RequestResult> Insert([Required] uint objectId, [Required] ObjectType type)
        {
            RequestResult result = new();
            uint memberId = (uint)CurrentMemberId!;

            using AssessDBContext db = entityService.GetDbContext();

            int count = await db.LikeRecords.CountAsync(e =>
                e.MemberId == memberId && e.ObjectId == objectId && e.ObjectType == type).NotWaitBack();

            //已点赞过就返回点赞成功
            if (count > 0) { result.Success("点赞成功"); }
            else
            {
                uint? objectMemberId = null;
                switch (type)
                {
                    case ObjectType.Comment:
                        var commentInfo = await db.CommentInfos.FindAsync(objectId);
                        if (commentInfo != null)
                        {
                            objectMemberId = commentInfo.MemberId;
                            ++commentInfo.LikeNum;
                        }
                        break;
                    case ObjectType.Letter:
                        var letterInfo = await db.ConsultLetters.FindAsync(objectId);
                        if (letterInfo != null)
                        {
                            objectMemberId = letterInfo.MemberId;
                            ++letterInfo.LikeNum;
                        }
                        break;
                    case ObjectType.Topic:
                        var topicInfo = await db.TopicInfos.FindAsync(objectId);
                        if (topicInfo != null)
                        {
                            objectMemberId = topicInfo.MemberId;
                            ++topicInfo.LikeNum;
                        }
                        break;
                }

                var myLikeRecord = await db.MyLikeRecords.FirstOrDefaultAsync(e =>
                    e.ObjectId == objectId && e.ObjectType == type);

                if (myLikeRecord != null) { ++myLikeRecord.LikeNum; }
                else
                {
                    myLikeRecord = new()
                    {
                        LikeNum = 1,
                        ObjectType = type,
                        ObjectId = objectId,
                        LastTime = DateTime.Now,
                        MemberId = objectMemberId ?? 0,
                    };
                    _ = db.MyLikeRecords.Add(myLikeRecord);
                }

                LikeRecord entity = new()
                {
                    ObjectType = type,
                    MemberId = memberId,
                    ObjectId = objectId,
                    MyLikeRecord = myLikeRecord,
                    CreationTime = DateTime.Now,
                };
                _ = db.LikeRecords.Add(entity);

                count = await db.SaveChangesAsync().NotWaitBack();
                if (count > 0) { result.Success("点赞成功"); }
                else { result.Fail("点赞失败"); }
            }

            return result;
        }

        /// <summary>
        /// 用户取消点赞某个对象
        /// </summary>
        /// <param name="objectId">对象ID</param>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<RequestResult> Delete([Required] uint objectId, [Required] ObjectType type)
        {
            RequestResult result = new();
            uint memberId = (uint)CurrentMemberId!;

            using AssessDBContext db = entityService.GetDbContext();

            var entity = await db.LikeRecords.FirstOrDefaultAsync(e =>
                e.MemberId == memberId && e.ObjectId == objectId && e.ObjectType == type).NotWaitBack();

            //没有点赞过就返回取消成功
            if (entity == null) { result.Success("取消成功"); }
            else
            {
                _ = db.LikeRecords.Remove(entity);

                switch (type)
                {
                    case ObjectType.Comment:
                        var commentInfo = await db.CommentInfos.FindAsync(objectId);
                        if (commentInfo != null) { --commentInfo.LikeNum; }
                        break;
                    case ObjectType.Letter:
                        var letterInfo = await db.ConsultLetters.FindAsync(objectId);
                        if (letterInfo != null) { --letterInfo.LikeNum; }
                        break;
                    case ObjectType.Topic:
                        var topicInfo = await db.TopicInfos.FindAsync(objectId);
                        if (topicInfo != null) { --topicInfo.LikeNum; }
                        break;
                }

                var myLikeRecord = await db.MyLikeRecords.FirstOrDefaultAsync(e =>
                    e.ObjectId == objectId && e.ObjectType == type);

                if (myLikeRecord != null) { --myLikeRecord.LikeNum; }

                int count = await db.SaveChangesAsync().NotWaitBack();
                if (count > 0) { result.Success("取消成功"); }
                else { result.Fail("取消失败"); }
            }

            return result;
        }

        /// <summary>
        /// 分页获取收到的赞
        /// </summary>
        /// <param name="page">当前页（从1开始）</param>
        /// <param name="take">每页加载数量</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<RequestResult<LikeListResult>> LikeMeList([Required] int page, [Required] int take)
        {
            uint memberId = (uint)CurrentMemberId!;
            RequestResult<LikeListResult> result = new();

            using AssessDBContext db = entityService.GetDbContext();

            List<MyLikeRecord> datas = await db.MyLikeRecords.Where(e => e.MemberId == memberId)
                .Select(e => new MyLikeRecord
                {
                    Id = e.Id,
                    LikeNum = e.LikeNum,
                    LastTime = e.LastTime,
                    ObjectId = e.ObjectId,
                    ObjectType = e.ObjectType,
                    LikeRecords = e.LikeRecords.Take(2).ToList(),
                }).Skip((page - 1) * take).Take(take).ToListAsync().NotWaitBack();

            List<uint> member_ids = datas.SelectMany(e => e.LikeRecords.Select(e => e.MemberId)).Distinct().ToList();
            List<Member> members = await db.Members.Where(e => member_ids.Contains(e.Id))
                .Select(e => new Member
                {
                    Id = e.Id,
                    Avatar = e.Avatar,
                    Nickname = e.Nickname,
                }).ToListAsync();

            List<LikeListResult> list = new();
            foreach (MyLikeRecord record in datas)
            {
                LikeListResult model = new()
                {
                    Total = record.LikeNum,
                    LikeTime = record.LastTime,
                    ObjectId = record.ObjectId,
                    ObjectType = record.ObjectType,
                };

                uint[] ids = record.LikeRecords.Select(e => e.MemberId).ToArray();
                if (ids.Length > 0 && members.Count > 0)
                {
                    model.Avatars = new string[ids.Length];
                    model.Nicknames = new string[ids.Length];

                    for (int i = 0; i < ids.Length; i++)
                    {
                        var member = members.Find(e => e.Id == ids[i]);
                        if (member != null)
                        {
                            model.Nicknames[i] = member.Nickname;
                            model.Avatars[i] = WebFileTool.GetAvatarUrl(member.Avatar);
                        }
                    }
                }
            }

            return result;
        }
    }
}
