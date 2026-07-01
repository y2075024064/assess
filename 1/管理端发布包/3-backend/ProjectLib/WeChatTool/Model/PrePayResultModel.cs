namespace WeChatTool.Model
{
    /// <summary>
    /// 预支付结果信息模型
    /// </summary>
    public class PrePayResultModel : BaseResult
    {
        /// <summary>
        /// 预支付Id
        /// </summary>
        public string? prepay_id { get; set; }
    }
}