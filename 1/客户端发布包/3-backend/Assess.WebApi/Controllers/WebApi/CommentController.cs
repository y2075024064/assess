using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;

namespace Assess.WebApi.Controllers.WebApi
{
    /// <summary>
    /// 评论相关接口
    /// </summary>
    public class CommentController : WebApiController<CommentInfo, ICommentService>
    {
        public CommentController(ICommentService entityService) : base(entityService)
        {
        }

        /// <summary>
        /// 分页获取主体对象评论列表
        /// </summary>
        /// <param name="param">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public Task<RequestPageResult<CommentResultInfo>> List(CommentSearchParam param)
        {
            uint memberId = (uint)CurrentMemberId!;
            return entityService.GetListPagedAsync(param, memberId,
                (name) => WebFileTool.GetImageUrl(name, ImageType.Comment),
                WebFileTool.GetAvatarUrl);
        }

        /// <summary>
        /// 分页获取顶级评论回复列表
        /// </summary>
        /// <param name="commentId">顶级评论ID</param>
        /// <param name="param">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public Task<RequestPageResult<CommentResultInfo>> Replies(uint commentId, PageBaseParam param)
        {
            uint memberId = (uint)CurrentMemberId!;
            return entityService.GetChildrenAsync(commentId, param, memberId,
                (name) => WebFileTool.GetImageUrl(name, ImageType.Comment),
                WebFileTool.GetAvatarUrl);
        }

        /// <summary>
        /// 对某条记录新增评论
        /// </summary>
        /// <param name="param">评论信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<RequestResult> Insert([FromForm] CommentInsertParam param)
        {
            RequestResult result = new();
            uint memberId = (uint)CurrentMemberId!;
            MemberType memberType = (MemberType)CurrentMemberType!;

            CommentInfo entity = new()
            {
                MemberId = memberId,
                Content = param.Content,
                ObjectId = param.ObjectId,
                CommentId = param.CommentId,
                CreationTime = DateTime.Now,
                ObjectType = param.ObjectType,
                Type = memberType switch
                {
                    MemberType.管理员 => CommentType.官方,
                    MemberType.教师 => CommentType.专家,
                    _ => CommentType.用户,
                }
            };

            if (param.Images?.Length > 0)
            {
                string[] imgs = new string[param.Images.Length];
                for (int i = 0; i < imgs.Length; i++)
                {
                    imgs[i] = WebFileTool.SaveImage(param.Images[i], ImageType.Comment);
                }

                entity.Images = string.Join(",", imgs);
            }

            using AssessDBContext db = entityService.GetDbContext();
            switch (param.ObjectType)
            {
                case ObjectType.Topic:
                    var topic = await db.TopicInfos.FindAsync(param.ObjectId);
                    if (topic != null) { ++topic.CommentNum; }
                    break;
                case ObjectType.Letter:
                    var letter = await db.ConsultLetters.FindAsync(param.ObjectId);
                    if (letter != null) { ++letter.CommentNum; }
                    break;
                case ObjectType.ThinkStar:
                    var star = await db.ThinkStars.FindAsync(param.ObjectId);
                    if (star != null) { ++star.CommentNum; }
                    break;
            }
            _ = db.CommentInfos.Add(entity);

            int count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("评论成功"); }
            else { result.Fail("评论失败"); }

            return result;
        }

        /// <summary>
        /// 用户删除某个评论
        /// </summary>
        /// <param name="id">评论ID</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<RequestResult> Delete(uint id)
        {
            RequestResult result = new();
            uint memberId = (uint)CurrentMemberId!;

            using AssessDBContext db = entityService.GetDbContext();
            var entity = await db.CommentInfos.FindAsync(id);
            if (entity?.MemberId == memberId)
            {
                entity.IsDelete = true;
                switch (entity.ObjectType)
                {
                    case ObjectType.Topic:
                        var topic = await db.TopicInfos.FindAsync(entity.ObjectId);
                        if (topic != null) { --topic.CommentNum; }
                        break;
                    case ObjectType.Letter:
                        var letter = await db.ConsultLetters.FindAsync(entity.ObjectId);
                        if (letter != null) { --letter.CommentNum; }
                        break;
                    case ObjectType.ThinkStar:
                        var star = await db.ThinkStars.FindAsync(entity.ObjectId);
                        if (star != null) { --star.CommentNum; }
                        break;
                }
            }

            int count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("删除成功"); }
            else { result.Fail("删除失败"); }

            return result;
        }
    }
}
