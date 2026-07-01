using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using System.Linq.Expressions;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using Repository;
using Mapster;

namespace Assess.WebApi.Controllers.WebApi
{
    /// <summary>
    /// 信件相关
    /// </summary>
    public class LetterController : WebApiController<ConsultLetter, ILetterService>
    {
        public LetterController(ILetterService entityService) : base(entityService)
        {
        }

        /// <summary>
        /// 分页获取公开信件列表
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <returns></returns>
        [HttpPost]
        public Task<RequestResult<LetterBaseInfo[]>> PublicList(LetterSearchParam param)
        {
            return ListAsync(param, false);
        }

        /// <summary>
        /// 用户分页获取自己的历史信件列表
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <returns></returns>
        [HttpPost]
        public Task<RequestResult<LetterBaseInfo[]>> History(LetterSearchParam param)
        {
            return ListAsync(param, true);
        }

        /// <summary>
        /// 管理员分页获取待回复信件列表
        /// </summary>
        /// <param name="param">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public Task<RequestResult<LetterBaseInfo[]>> AdminList(PageBaseParam param)
        {
            LetterSearchParam param1 = new() { Page = param.Page, Take = param.Take };
            return ListAsync(param1, false, true);
        }

        /// <summary>
        /// 用户发布信件
        /// </summary>
        /// <param name="param">信件参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<RequestResult> Insert([FromForm] LetterInsertParam param)
        {
            int count = 0;
            RequestResult result = new();

            string[] imgs = Array.Empty<string>();
            if (param.Images?.Length > 0)
            {
                imgs = new string[param.Images.Length];
                for (int i = 0; i < imgs.Length; i++)
                {
                    imgs[i] = WebFileTool.SaveImage(param.Images[i], ImageType.Letter);
                }
            }

            using (var db = entityService.GetDbContext())
            {
                ConsultLetter entity = new()
                {
                    Title = param.Title,
                    State = LetterState.New,
                    Content = param.Content,
                    IsPublic = param.IsPublic,
                    CreationTime = DateTime.Now,
                    Label = param.Label?.ToJson(),
                    Images = string.Join(",", imgs),
                    MemberId = (uint)CurrentMemberId!,
                };
                _ = await db.AddAsync(entity);

                if (param.Label?.Length > 0)
                {
                    var list = await db.TagRecords.Where(e => param.Label.Contains(e.Name))
                        .ToListAsync();
                    foreach (var item in list) { item.UseNum += 1; }
                    foreach (var item in param.Label)
                    {
                        if (!list.Exists(e => e.Name == item))
                        {
                            TagRecord record = new() { Name = item, UseNum = 1 };
                            _ = await db.AddAsync(record);
                        }
                    }
                }

                count = await db.SaveChangesAsync().NotWaitBack();
            }

            if (count > 0) { result.Success("发布成功"); }
            else
            {
                result.Fail("发布失败");
                foreach (var item in imgs)
                {
                    WebFileTool.DeleteImage(item, ImageType.Letter);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取信件详情
        /// </summary>
        /// <param name="id">信件ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<RequestResult<LetterDetailInfo>> Detail(uint id)
        {
            uint? memberId = CurrentMemberId;
            RequestResult<LetterDetailInfo> result = new();

            ConsultLetter? entity = null;
            BrowseRecordInfo? recordInfo = null;

            using (AssessDBContext db = entityService.GetDbContext())
            {
                entity = await db.ConsultLetters.Include(e => e.Member).SingleOrDefaultAsync(e => e.Id == id);
                if (entity != null && memberId.HasValue)
                {
                    DateTime nowTime = DateTime.Now;
                    BrowseRecord? browseRecord = await db.BrowseRecords.FirstOrDefaultAsync(e =>
                        e.ObjectId == id && e.ObjType == ObjectType.Letter && e.MemberId == memberId);
                    if (browseRecord == null)
                    {
                        entity.ViewNum += 1;
                        browseRecord = new BrowseRecord()
                        {
                            ObjectId = id,
                            IsGiveLike = false,
                            IsCollection = false,
                            CreationTime = nowTime,
                            LastBrowseOn = nowTime,
                            MemberId = memberId.Value,
                            ObjType = ObjectType.Letter,
                        };
                        _ = await db.AddAsync(browseRecord);
                    }
                    else
                    {
                        if (browseRecord.LastBrowseOn.Date != nowTime.Date)
                        {
                            entity.ViewNum += 1;
                        }
                        browseRecord.LastBrowseOn = nowTime;
                    }
                    recordInfo = browseRecord.Adapt<BrowseRecordInfo>();

                    _ = await db.SaveChangesAsync().NotWaitBack();
                }
            }

            if (entity != null)
            {
                result.Data = new LetterDetailInfo
                {
                    Id = entity.Id,
                    State = entity.State,
                    Title = entity.Title,
                    Label = entity.Label,
                    Content = entity.Content,
                    ViewNum = entity.ViewNum,
                    IsModel = entity.IsModel,
                    LikeNum = entity.LikeNum,
                    BrowseRecord = recordInfo,
                    IsPublic = entity.IsPublic,
                    CommentNum = entity.CommentNum,
                    Nickname = entity.Member?.Nickname,
                    CreationTime = entity.CreationTime,
                    Avatar = WebFileTool.GetAvatarUrl(entity.Member?.Avatar),
                };

                if (!string.IsNullOrEmpty(entity.Images))
                {
                    string[] imgs = entity.Images.Split(',');
                    result.Data.Images = new string[imgs.Length];
                    for (int i = 0; i < imgs.Length; i++)
                    {
                        string imgUrl = WebFileTool.GetImageUrl(imgs[i], ImageType.Letter);
                        result.Data.Images[i] = imgUrl;
                    }
                }

                result.Success();
            }
            else { result.Fail("您要获取的信件不存在"); }

            return result;
        }

        /// <summary>
        /// 用户删除某个信件
        /// </summary>
        /// <param name="id">信件ID</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<RequestResult> Delete(uint id)
        {
            RequestResult result = new();
            uint memberId = (uint)CurrentMemberId!;

            int count = await entityService.UpdateAsync(
                e => e.Id == id && e.MemberId == memberId,
                e => new ConsultLetter { IsDelete = true });

            if (count > 0) { result.Success("删除成功"); }
            else { result.Fail("删除失败"); }

            return result;
        }

        /// <summary>
        /// 管理员对信件进行评论
        /// </summary>
        /// <param name="param">评论信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<RequestResult> AdminReply([FromForm] AdminCommentParam param)
        {
            RequestResult result = new();
            uint memberId = (uint)CurrentMemberId!;
            MemberType memberType = (MemberType)CurrentMemberType!;

            if (memberType != MemberType.管理员)
            {
                result.Fail("您不是管理员");
            }
            else
            {
                string[] imgs = new string[param.Images.Length];
                if (param.Images?.Length > 0)
                {
                    for (int i = 0; i < imgs.Length; i++)
                    {
                        imgs[i] = WebFileTool.SaveImage(param.Images[i], ImageType.Comment);
                    }
                }

                using var db = entityService.GetDbContext();
                CommentInfo comment = new()
                {
                    IsTop = true,
                    MemberId = memberId,
                    Content = param.Content,
                    Type = CommentType.官方,
                    ObjectId = param.ObjectId,
                    CreationTime = DateTime.Now,
                    ObjectType = ObjectType.Letter,
                    Images = string.Join(",", imgs)
                };
                _ = await db.AddAsync(comment);

                var letter = await db.ConsultLetters.FindAsync(param.ObjectId);
                if (letter != null)
                {
                    ++letter.CommentNum;
                    letter.State = param.State;
                    letter.IsModel = param.IsModel;
                }

                int count = await db.SaveChangesAsync().NotWaitBack();
                if (count > 0) { result.Success("评论成功"); }
                else
                {
                    result.Fail("评论失败");
                    foreach (var image in imgs)
                    {
                        WebFileTool.DeleteImage(image, ImageType.Comment);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 分页获取信件列表
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <param name="isHistory">是否是查询用户历史信件</param>
        /// <param name="isAdmin">是否管理员</param>
        /// <returns></returns>
        private async Task<RequestResult<LetterBaseInfo[]>> ListAsync(LetterSearchParam param, bool isHistory, bool isAdmin = false)
        {
            RequestResult<LetterBaseInfo[]> result = new();

            Expression<Func<ConsultLetter, bool>> where = e => e.IsDelete == false;
            if (isAdmin)
            {
                where = where.AndAlso(e => e.State == LetterState.New);
            }
            else
            {
                if (isHistory)
                {
                    uint memberId = (uint)CurrentMemberId!;
                    where = where.AndAlso(e => e.MemberId == memberId);
                }
                else
                {
                    where = where.AndAlso(e => e.IsPublic == true && e.State > LetterState.New);
                }

                if (param.StartTime != null)
                {
                    where = where.AndAlso(e => e.CreationTime >= param.StartTime);
                }
                if (param.EndTime != null)
                {
                    where = where.AndAlso(e => e.CreationTime <= param.EndTime);
                }
                if (!string.IsNullOrWhiteSpace(param.Keyword))
                {
                    where = where.AndAlso(e => e.Title.Contains(param.Keyword) || e.Content.Contains(param.Keyword));
                }
            }

            string orderType = nameof(ConsultLetter.CreationTime);
            if (param.OrderType == 1) { orderType = nameof(ConsultLetter.ViewNum); }

            var page = await entityService.GetPageAsync(where, e => new LetterBaseInfo
            {
                Id = e.Id,
                Title = e.Title,
                State = e.State,
                Label = e.Label,
                Image = e.Images,
                IsModel = e.IsModel,
                LikeNum = e.LikeNum,
                ViewNum = e.ViewNum,
                IsPublic = e.IsPublic,
                CommentNum = e.CommentNum,
                CreationTime = e.CreationTime,
                Content = e.Content.Substring(0, 200),

                Avatar = e.Member!.Avatar,
                Nickname = e.Member!.Nickname,
            }, orderType, OrderRule.Desc, param.Page, param.Take).NotWaitBack();

            if (page?.Data != null)
            {
                foreach (var item in page.Data)
                {
                    if (!string.IsNullOrEmpty(item.Image))
                    {
                        string img = item.Image.Split(',')[0];
                        item.Image = WebFileTool.GetImageUrl(img, ImageType.Letter);
                    }
                    item.Avatar = WebFileTool.GetAvatarUrl(item.Avatar);
                }

                result.Data = page.Data.ToArray();
            }
            else
            {
                result.Data = Array.Empty<LetterBaseInfo>();
            }

            result.Success();

            return result;
        }
    }
}
