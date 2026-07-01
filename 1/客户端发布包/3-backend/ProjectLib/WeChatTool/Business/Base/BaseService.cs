using WeChatTool.Lib;

namespace WeChatTool.Business.Base
{
    /// <summary>
    /// 基础服务类
    /// </summary>
    public class BaseService
    {
        /// <summary>
        /// AppId
        /// </summary>
        protected readonly string appId = "";

        /// <summary>
        /// AppSecret
        /// </summary>
        protected readonly string appSecret = "";

        /// <summary>
        /// 缓存管理器
        /// </summary>
        protected readonly IWXCacheManage cacheManage;

        /// <summary>
        /// 实例服务
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">AppSecret</param>
        /// <param name="cacheManage">缓存管理器</param>
        public BaseService(IWXCacheManage cacheManage, string appId, string appSecret)
        {
            this.appId = appId;
            this.appSecret = appSecret;
            this.cacheManage = cacheManage;
        }
    }
}