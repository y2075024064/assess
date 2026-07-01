namespace WeChatTool.Model
{
    /// <summary>
    /// 退款信息参数模型
    /// </summary>
    public class RefundInfoParamModel
    {
        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string? transaction_id { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string? out_trade_no { get; set; }

        /// <summary>
        /// 商户退款单号
        /// </summary>
        public string? out_refund_no { get; set; }

        /// <summary>
        /// 退款原因
        /// </summary>
        public string? reason { get; set; }

        /// <summary>
        /// 退款结果回调url
        /// </summary>
        public string? notify_url { get; set; }

        /// <summary>
        /// 退款资金来源
        /// </summary>
        //public string? funds_account { get; set; }

        /// <summary>
        /// 退款金额信息
        /// </summary>
        public RefundAmountInfo? amount { get; set; }
    }

    /// <summary>
    /// 退款金额信息模型
    /// </summary>
    public class RefundAmountInfo
    {
        /// <summary>
        /// 退款金额
        /// </summary>
        public int refund { get; set; }

        /// <summary>
        /// 原订单金额
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 退款币种
        /// </summary>
        public string? currency { get; set; }
    }
}