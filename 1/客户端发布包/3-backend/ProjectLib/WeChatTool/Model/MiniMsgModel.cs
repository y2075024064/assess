namespace WeChatTool.Model
{
    #region 订阅消息模型

    /// <summary>
    /// 小程序订阅消息模型
    /// </summary>
    public class SubscribeMsgModel
    {
        /// <summary>
        /// 【必填】接收者 openid
        /// </summary>
        public string touser { get; set; } = string.Empty;

        /// <summary>
        /// 【必填】所需下发的订阅模板id
        /// </summary>
        public string template_id { get; set; } = string.Empty;

        /// <summary>
        /// 【选填】点击模板卡片后的跳转页面，仅限本小程序内的页面。支持带参数,（示例index?foo=bar）
        /// </summary>
        public string page { get; set; } = string.Empty;

        /// <summary>
        /// 【必填】模板内容，格式形如 { "key1": data, "key2": data }
        /// </summary>
        public Dictionary<string, MiniMgsDataModel> data { get; set; } = new Dictionary<string, MiniMgsDataModel>();

        /// <summary>
        /// 【选填】跳转小程序类型：developer为开发版；trial为体验版；formal为正式版；默认为正式版
        /// </summary>
        public string miniprogram_state { get; set; } = string.Empty;

        /// <summary>
        /// 【选填】语言类型，支持zh_CN(简体中文)、en_US(英文)、zh_HK(繁体中文)、zh_TW(繁体中文)，默认为zh_CN
        /// </summary>
        public string lang { get; set; } = string.Empty;
    }

    /// <summary>
    /// 模板内容模型
    /// </summary>
    public class MiniMgsDataModel
    {
        /// <summary>
        /// 【必填】数据值
        /// </summary>
        public string value { get; set; } = string.Empty;
    }

    #endregion
}