using Assess.Entity;
using Repository;

namespace WeChatTool.Lib
{
    /// <summary>
    /// 微信缓存服务接口
    /// </summary>
    public interface IWxCacheService : IServiceBase<AssessDBContext, WechatCache>
    {
        /// <summary>
        /// 异步获取微信缓存
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <returns></returns>
        Task<WechatCache?> GetWxCacheAsync(string appId);

        /// <summary>
        /// 异步获取微信用户缓存信息
        /// </summary>
        /// <param name="openid">用户OpenId</param>
        /// <returns></returns>
        Task<WechatMemberCache?> GetWxUserCacheAsync(string openid);
    }
}