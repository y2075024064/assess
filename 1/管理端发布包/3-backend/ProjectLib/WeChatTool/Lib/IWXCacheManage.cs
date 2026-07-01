using WeChatTool.Model;

namespace WeChatTool.Lib
{
    /// <summary>
    /// 微信缓存管理接口
    /// </summary>
    public interface IWXCacheManage
    {
        /// <summary>
        /// 微信缓存服务
        /// </summary>
        public IWxCacheService WXCacheService { get; }

        /// <summary>
        /// 获取对应应用Token
        /// </summary>
        /// <param name="appId">AppID</param>
        /// <param name="appSecret">AppSecret</param>
        /// <exception cref="WeChatException"></exception>
        /// <returns>access_token</returns>
        Task<string> GetTokenAsync(string appId, string appSecret);

        /// <summary>
        /// 获取网页授权Token
        /// </summary>
        /// <param name="openid">Openid</param>
        /// <param name="code">Code</param>
        /// <param name="appId">AppID</param>
        /// <param name="appSecret">AppSecret</param>
        /// <returns>网页授权access_token</returns>
        Task<AuthTokenModel> GetAuthTokenAsync(string openid, string code, string appId, string appSecret);
    }
}