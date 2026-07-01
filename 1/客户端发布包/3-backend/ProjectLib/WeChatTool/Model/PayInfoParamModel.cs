namespace WeChatTool.Model
{
    /// <summary>
    /// 支付信息参数模型
    /// </summary>
    public class PayInfoParamModel
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string? appid { get; set; }

        /// <summary>
        /// 直连商户号
        /// </summary>
        public string? mchid { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string? description { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string? out_trade_no { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public string? attach { get; set; }

        /// <summary>
        /// 通知地址
        /// </summary>
        public string? notify_url { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public PayAmountModel? amount { get; set; }

        /// <summary>
        /// 支付者信息
        /// </summary>
        public PayPayerModel? payer { get; set; }
    }

    /// <summary>
    /// 支付金额信息模型
    /// </summary>
    public class PayAmountModel
    {
        /// <summary>
        /// 总金额
        /// </summary>
        public int total { get; set; }
    }

    /// <summary>
    /// 支付者信息模型
    /// </summary>
    public class PayPayerModel
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string? openid { get; set; }
    }
}