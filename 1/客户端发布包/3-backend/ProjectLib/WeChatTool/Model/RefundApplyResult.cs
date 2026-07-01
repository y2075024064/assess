namespace WeChatTool.Model
{
    /// <summary>
    /// 退款申请结果模型
    /// </summary>
    public class RefundApplyResult : BaseResult
    {
        /// <summary>
        /// 微信支付退款单号
        /// </summary>
        public string? refund_id { get; set; }

        /// <summary>
        /// 商户退款单号
        /// </summary>
        public string? out_refund_no { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string? transaction_id { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string? out_trade_no { get; set; }

        /// <summary>
        /// 退款渠道
        /// </summary>
        public string? channel { get; set; }

        /// <summary>
        /// 退款入账账户
        /// </summary>
        public string? user_received_account { get; set; }

        /// <summary>
        /// 退款成功时间
        /// </summary>
        public DateTime? success_time { get; set; }

        /// <summary>
        /// 退款创建时间
        /// </summary>
        public string? create_time { get; set; }

        /// <summary>
        /// 退款状态
        /// </summary>
        public string? status { get; set; }

        /// <summary>
        /// 资金账户
        /// </summary>
        public string? funds_account { get; set; }

        /// <summary>
        /// 退款金额详细信息
        /// </summary>
        public RefundAmountResult? amount { get; set; }
    }

    /// <summary>
    /// 退款金额结果模型
    /// </summary>
    public class RefundAmountResult : RefundAmountInfo
    {
        /// <summary>
        /// 用户支付金额
        /// </summary>
        public int payer_total { get; set; }

        /// <summary>
        /// 用户退款金额
        /// </summary>
        public int payer_refund { get; set; }

        /// <summary>
        /// 应结退款金额
        /// </summary>
        public int settlement_refund { get; set; }

        /// <summary>
        /// 应结订单金额
        /// </summary>
        public int settlement_total { get; set; }

        /// <summary>
        /// 优惠退款金额
        /// </summary>
        public int discount_refund { get; set; }

        /// <summary>
        /// 手续费退款金额
        /// </summary>
        public int? refund_fee { get; set; }
    }
}