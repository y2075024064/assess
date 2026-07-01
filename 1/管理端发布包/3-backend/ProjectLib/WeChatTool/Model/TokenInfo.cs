namespace WeChatTool.Model
{
    /// <summary>
    /// 应用对应Token信息模型
    /// </summary>
    public class AppTokenInfo
    {
        /// <summary>
        /// 应用AppID
        /// </summary>
        public string appId { get; set; } = string.Empty;

        /// <summary>
        /// access_token
        /// </summary>
        public string token { get; set; } = string.Empty;

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime overdueTime { get; set; }
    }

    /// <summary>
    /// 凭证获取结果模型
    /// </summary>
    public class GetTokenResult : BaseResult
    {
        /// <summary>
        /// 凭证
        /// </summary>
        public string access_token { get; set; } = string.Empty;

        /// <summary>
        /// 凭证有效时长（秒）
        /// </summary>
        public int expires_in { get; set; }
    }

    /// <summary>
    /// 网页授权凭证获取结果模型
    /// </summary>
    public class AuthTokenInfo : BaseResult
    {
        /// <summary>
        /// 网页授权接口调用凭证
        /// </summary>
        public string access_token { get; set; } = string.Empty;

        /// <summary>
        /// 调用凭证超时时长（秒）
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string refresh_token { get; set; } = string.Empty;

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string openid { get; set; } = string.Empty;

        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string scope { get; set; } = string.Empty;
    }

    /// <summary>
    /// 网页授权凭证模型
    /// </summary>
    public class AuthTokenModel
    {
        /// <summary>
        /// access_token
        /// </summary>
        public string? token { get; set; }

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string openid { get; set; } = string.Empty;

        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string? scope { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime overdueTime { get; set; }
    }
}