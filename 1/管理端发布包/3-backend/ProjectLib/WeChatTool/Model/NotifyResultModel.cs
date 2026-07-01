namespace WeChatTool.Model
{
    /// <summary>
    /// 通知结果参数模型
    /// </summary>
    public class NotifyResultParam
    {
        /// <summary>
        /// 通知ID
        /// </summary>
        public string? id { get; set; }

        /// <summary>
        /// 通知创建时间
        /// </summary>
        public string? create_time { get; set; }

        /// <summary>
        /// 通知数据类型
        /// </summary>
        public string? resource_type { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        public string? event_type { get; set; }

        /// <summary>
        /// 回调摘要
        /// </summary>
        public string? summary { get; set; }

        /// <summary>
        /// 通知资源数据
        /// </summary>
        public NotifyResourceModel? resource { get; set; }
    }

    /// <summary>
    /// 通知资源信息模型
    /// </summary>
    public class NotifyResourceModel : EncryptCertInfo
    {
        /// <summary>
        /// 加密前的对象类型
        /// </summary>
        public string? original_type { get; set; }
    }
}