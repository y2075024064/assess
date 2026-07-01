namespace WeChatTool.Model
{
    /// <summary>
    /// 退款结果模型
    /// </summary>
    public class RefundResultModel
    {
        /// <summary>
        /// 直连商户号
        /// </summary>
        public string? mchid { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string? out_trade_no { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string? transaction_id { get; set; }

        /// <summary>
        /// 商户退款单号
        /// </summary>
        public string? out_refund_no { get; set; }

        /// <summary>
        /// 微信支付退款单号
        /// </summary>
        public string? refund_id { get; set; }

        /// <summary>
        /// 退款状态
        /// </summary>
        public string? refund_status { get; set; }

        /// <summary>
        /// 退款成功时间
        /// </summary>
        public DateTime? success_time { get; set; }

        /// <summary>
        /// 退款入账账户
        /// </summary>
        public string? user_received_account { get; set; }

        /// <summary>
        /// 金额信息
        /// </summary>
        public RefundAmountModel? amount { get; set; }
    }

    /// <summary>
    /// 退款金额信息模型
    /// </summary>
    public class RefundAmountModel
    {
        /// <summary>
        /// 订单金额
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public int refund { get; set; }

        /// <summary>
        /// 用户支付金额
        /// </summary>
        public int payer_total { get; set; }

        /// <summary>
        /// 用户退款金额
        /// </summary>
        public int payer_refund { get; set; }
    }
}