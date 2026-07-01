namespace WeChatTool.JSSDK
{
    /// <summary>
    /// 配置类
    /// </summary>
    public class SignPackage
    {
        public bool debug { get; set; }

        /// <summary>
        /// 必填，公众号的唯一标识
        /// </summary>
        public string appId { get; set; } = string.Empty;

        /// <summary>
        /// 生成签名的时间戳
        /// </summary>
        public long timestamp { get; set; }

        /// <summary>
        /// 生成签名的随机串
        /// </summary>
        public string nonceStr { get; set; } = string.Empty;

        /// <summary>
        /// 签名
        /// </summary>
        public string signature { get; set; } = string.Empty;

        /// <summary>
        /// 需要使用的JS接口列表
        /// </summary>
        public string[] jsApiList { get; set; } = Array.Empty<string>();
    }
}