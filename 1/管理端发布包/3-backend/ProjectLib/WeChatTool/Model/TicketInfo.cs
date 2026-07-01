namespace WeChatTool.Model
{
    /// <summary>
    /// ticket获取结果模型
    /// </summary>
    public class TicketInfo : BaseResult
    {
        /// <summary>
        /// ticket
        /// </summary>
        public string ticket { get; set; } = string.Empty;

        /// <summary>
        /// ticket有效时长（秒）
        /// </summary>
        public int expires_in { get; set; }
    }
}