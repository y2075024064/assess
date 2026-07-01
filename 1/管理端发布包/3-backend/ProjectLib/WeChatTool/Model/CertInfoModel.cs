namespace WeChatTool.Model
{
    /// <summary>
    /// 证书信息结果模型
    /// </summary>
    public class CertInfoResult : BaseResult
    {
        /// <summary>
        /// 证书信息集合
        /// </summary>
        public CertInfoModel[]? data { get; set; }
    }

    /// <summary>
    /// 证书信息模型
    /// </summary>
    public class CertInfoModel
    {
        /// <summary>
        /// 证书序列号
        /// </summary>
        public string? serial_no { get; set; }

        /// <summary>
        /// 生效日期时间
        /// </summary>
        public DateTime? effective_time { get; set; }

        /// <summary>
        /// 失效日期时间
        /// </summary>
        public DateTime? expire_time { get; set; }

        /// <summary>
        /// 加密证书信息
        /// </summary>
        public EncryptCertInfo? encrypt_certificate { get; set; }
    }

    /// <summary>
    /// 加密证书信息模型
    /// </summary>
    public class EncryptCertInfo
    {
        /// <summary>
        /// 加密算法类型
        /// </summary>
        public string? algorithm { get; set; }

        /// <summary>
        /// 数据密文
        /// </summary>
        public string? ciphertext { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public string? associated_data { get; set; }

        /// <summary>
        /// 随机串
        /// </summary>
        public string? nonce { get; set; }
    }
}