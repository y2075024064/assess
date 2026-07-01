using Microsoft.EntityFrameworkCore;
using Assess.Entity;
using Repository;

namespace WeChatTool.Lib
{
    /// <summary>
    /// 微信缓存服务
    /// </summary>
    public class WxCacheService : ServiceBase<AssessDBContext, WechatCache>, IWxCacheService
    {
        public WxCacheService(DbContextOptions<AssessDBContext> options) : base(options) { }

        public Task<WechatCache?> GetWxCacheAsync(string appId)
        {
            return GetEntityAsync(e => e.AppId == appId);
        }

        public Task<WechatMemberCache?> GetWxUserCacheAsync(string openid)
        {
            return Adapter.GetEntityAsync<WechatMemberCache>(e => e.OpenId == openid);
        }
    }
}