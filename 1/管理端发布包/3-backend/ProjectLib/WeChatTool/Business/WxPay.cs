using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using WeChatTool.Model;
using WeChatTool.Tools;
using System.Text;
using Assess;

namespace WeChatTool.Business
{
    /// <summary>
    /// 微信支付
    /// </summary>
    public class WxPay
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="param">订单参数</param>
        /// <returns></returns>
        public static async Task<PrePayResultModel> CreateOrderAsync(PayInfoParamModel param)
        {
            PrePayResultModel result = new();

            try
            {
                string url = "https://api.mch.weixin.qq.com/v3/pay/transactions/jsapi";

                string jsonParam = JsonSerializer.Serialize(param);
                HttpRequestMessage request = new(HttpMethod.Post, url)
                {
                    Content = new StringContent(jsonParam, Encoding.UTF8, "application/json")
                };

                string res = await WxAPIV3Tool.SendAsync(request);
                if (string.IsNullOrWhiteSpace(res))
                {
                    result.errcode = -1;
                    result.errmsg = "支付下单失败！";
                    return result;
                }

                PrePayResultModel? _result = JsonSerializer.Deserialize<PrePayResultModel>(res);
                if (_result != null) { result = _result; }
            }
            catch (Exception ex)
            {
                result.errcode = 1;
                result.errmsg = "支付下单失败。" + ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="param">退款参数</param>
        /// <returns></returns>
        public static async Task<RefundApplyResult> RefundAsync(RefundInfoParamModel param)
        {
            RefundApplyResult result = new();

            try
            {
                string url = "https://api.mch.weixin.qq.com/v3/refund/domestic/refunds";

                string jsonParam = JsonSerializer.Serialize(param);
                HttpRequestMessage request = new(HttpMethod.Post, url)
                {
                    Content = new StringContent(jsonParam, Encoding.UTF8, "application/json")
                };

                string res = await WxAPIV3Tool.SendAsync(request);
                if (string.IsNullOrWhiteSpace(res))
                {
                    result.errcode = -1;
                    result.errmsg = "退款申请失败！";
                    return result;
                }

                RefundApplyResult? _result = JsonSerializer.Deserialize<RefundApplyResult>(res);
                if (_result != null) { result = _result; }
            }
            catch (Exception ex)
            {
                result.errcode = 1;
                result.errmsg = "退款申请失败。" + ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 验证微信签名信息
        /// </summary>
        /// <param name="data">待验证数据</param>
        /// <param name="sign">签名串</param>
        /// <param name="serialNo">微信支付证书序列号</param>
        /// <returns></returns>
        public static async Task<bool> VerifyWxSignAsync(string data, string sign, string serialNo)
        {
            bool isSucc = WxAPIV3Tool.VerifyWxCertSerialNoAndAfterTime(serialNo);
            if (!isSucc)
            {
                WxPay wxPay = new();
                CertInfoResult certInfoResult = await wxPay.GetCertificatesAsync();
                if (certInfoResult.IsSucc && certInfoResult.data?.Length > 0)
                {
                    CertInfoModel certInfo = certInfoResult.data.OrderBy(e => e.expire_time).Last();
                    isSucc = wxPay.SaveCertFile(certInfo);
                }
            }

            if (isSucc) { isSucc = WxAPIV3Tool.VerifyWxSignature(data, sign); }

            return isSucc;
        }

        /// <summary>
        /// 获取微信支付证书
        /// </summary>
        /// <returns></returns>
        public async Task<CertInfoResult> GetCertificatesAsync()
        {
            CertInfoResult result = new();

            try
            {
                string url = "https://api.mch.weixin.qq.com/v3/certificates";

                HttpRequestMessage request = new(HttpMethod.Get, url);

                string res = await WxAPIV3Tool.SendAsync(request);
                if (string.IsNullOrWhiteSpace(res))
                {
                    result.errcode = -1;
                    result.errmsg = "获取微信支付证书失败！";
                    return result;
                }

                CertInfoResult? _result = JsonSerializer.Deserialize<CertInfoResult>(res);
                if (_result != null) { result = _result; }
            }
            catch (Exception ex)
            {
                result.errcode = 1;
                result.errmsg = "获取微信支付证书失败。" + ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 保存证书文件
        /// </summary>
        /// <param name="model">证书信息模型</param>
        /// <returns></returns>
        public bool SaveCertFile(CertInfoModel model)
        {
            EncryptCertInfo? enc_Info = model.encrypt_certificate;
            if (enc_Info == null) { return false; }

            if (string.IsNullOrWhiteSpace(enc_Info.ciphertext) ||
                string.IsNullOrWhiteSpace(enc_Info.nonce))
            { return false; }

            string certTxt = WxAPIV3Tool.AesGcmDecrypt(enc_Info.ciphertext, enc_Info.nonce, enc_Info.associated_data);
            //保存证书文件
            string certPem = AppSettings.Settings.WxCertPem;
            using (StreamWriter writer = new(certPem, false, Encoding.UTF8))
            {
                writer.Write(certTxt);
            }

            X509Certificate2 cert = X509Certificate2.CreateFromPem(certTxt);
            WxAPIV3Tool.InitWxRSAInfo(cert.GetRSAPublicKey(), model.serial_no);
            cert.Dispose();

            return true;
        }
    }
}