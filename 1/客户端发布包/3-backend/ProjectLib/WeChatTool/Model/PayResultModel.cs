namespace WeChatTool.Model
{
    /// <summary>
    /// 支付结果信息模型
    /// </summary>
    public class PayResultModel
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
        /// 商户订单号
        /// </summary>
        public string? out_trade_no { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string? transaction_id { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public string? trade_type { get; set; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public string? trade_state { get; set; }

        /// <summary>
        /// 交易状态描述
        /// </summary>
        public string? trade_state_desc { get; set; }

        /// <summary>
        /// 付款银行
        /// </summary>
        public string? bank_type { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public string? attach { get; set; }

        /// <summary>
        /// 支付完成时间
        /// </summary>
        public DateTime? success_time { get; set; }

        /// <summary>
        /// 支付者信息
        /// </summary>
        public PayPayerModel? payer { get; set; }

        /// <summary>
        /// 订单金额信息
        /// </summary>
        public PayAmountResultModel? amount { get; set; }

        /// <summary>
        /// 支付场景信息描述
        /// </summary>
        public PaySceneInfoModel? scene_info { get; set; }

        /// <summary>
        /// 优惠功能
        /// </summary>
        public object? promotion_detail { get; set; }
    }

    /// <summary>
    /// 支付金额信息模型
    /// </summary>
    public class PayAmountResultModel : PayAmountModel
    {
        /// <summary>
        /// 用户支付金额
        /// </summary>
        public int payer_total { get; set; }

        /// <summary>
        /// 用户支付币种
        /// </summary>
        public string? payer_currency { get; set; }
    }

    /// <summary>
    /// 支付场景信息模型
    /// </summary>
    public class PaySceneInfoModel
    {
        /// <summary>
        /// 商户端设备号
        /// </summary>
        public string? device_id { get; set; }
    }

    /// <summary>
    /// 支付优惠信息模型
    /// </summary>
    public class PromotionDetailModel
    {

    }
}