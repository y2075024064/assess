using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Infrastructure.Web;
using System.Text;
using Assess;

namespace WeChatTool.Tools
{
    /// <summary>
    /// 微信API工具类
    /// </summary>
    public class WxAPIV3Tool
    {
        private static string? wxSerialNo;

        private static RSA? mchRsa;

        private static RSA? wxRsa;

        private static DateTime? notAfter;

        /// <summary>
        /// 获取商户号签名
        /// </summary>
        /// <param name="message">签名内容</param>
        /// <returns></returns>
        public static string GetMchSignature(string message)
        {
            if (mchRsa == null)
            {
                string keyPem = AppSettings.Settings.MchKeyPem;
                if (File.Exists(keyPem))
                {
                    mchRsa = RSA.Create();
                    using StreamReader reader = new(keyPem, Encoding.UTF8);
                    string pemStr = reader.ReadToEnd();
                    mchRsa.ImportFromPem(pemStr);
                }
            }

            if (mchRsa != null)
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(message);
                byte[] resultBytes = mchRsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                return Convert.ToBase64String(resultBytes);
            }

            return "";
        }

        /// <summary>
        /// 验证微信证书序列号和过期时间
        /// </summary>
        /// <param name="serialNo">微信证书序列号</param>
        /// <returns></returns>
        public static bool VerifyWxCertSerialNoAndAfterTime(string serialNo)
        {
            if (wxRsa == null)
            {
                string certPem = AppSettings.Settings.WxCertPem;
                FileInfo fileInfo = new(certPem);
                if (fileInfo.Exists)
                {
                    notAfter = fileInfo.LastWriteTime.AddHours(12);
                    using FileStream fileStream = fileInfo.OpenRead();
                    using StreamReader reader = new(fileStream, Encoding.UTF8);
                    string pemStr = reader.ReadToEnd();
                    X509Certificate2 cert = X509Certificate2.CreateFromPem(pemStr);
                    wxSerialNo = cert.SerialNumber;
                    wxRsa = cert.GetRSAPublicKey();
                    cert.Dispose();
                }
            }

            return wxSerialNo == serialNo && notAfter > DateTime.Now;
        }

        /// <summary>
        /// 获取微信端签名
        /// </summary>
        /// <param name="message">签名内容</param>
        /// <param name="sign">微信的签名串</param>
        /// <returns></returns>
        public static bool VerifyWxSignature(string message, string sign)
        {
            if (wxRsa == null) { return false; }

            byte[] signBytes = Convert.FromBase64String(sign);
            byte[] dataBytes = Encoding.UTF8.GetBytes(message);

            return wxRsa.VerifyData(dataBytes, signBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }

        /// <summary>
        /// 发生请求
        /// </summary>
        /// <param name="request">请求消息</param>
        /// <returns></returns>
        public static async Task<string> SendAsync(HttpRequestMessage request)
        {
            if (request.RequestUri != null)
            {
                string body = "";
                string nonce = Path.GetRandomFileName();
                string mchid = AppSettings.Settings.MchId;
                string uri = request.RequestUri.PathAndQuery;
                string method = request.Method.Method.ToUpper();
                string serialNo = AppSettings.Settings.CertSerialNo;
                long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

                if (request.Content != null)
                {
                    string[] methods = new[] { "POST", "PUT", "PATCH" };
                    if (methods.Contains(method)) { body = await request.Content.ReadAsStringAsync(); }
                }

                _ = request.Headers.TryAddWithoutValidation("User-Agent", "https://zh.wikipedia.org/wiki/User_agent");
                request.Headers.Accept.ParseAdd("application/json");

                string message = $"{method}\n{uri}\n{timestamp}\n{nonce}\n{body}\n";
                string signature = GetMchSignature(message);

                string auth = $"mchid=\"{mchid}\",nonce_str=\"{nonce}\",timestamp=\"{timestamp}\",serial_no=\"{serialNo}\",signature=\"{signature}\"";
                string value = $"WECHATPAY2-SHA256-RSA2048 {auth}";
                request.Headers.Add("Authorization", value);

                return await HttpRequestTool.HttpSendAsync(request);
            }

            return "";
        }

        /// <summary>
        /// 解密加密内容
        /// </summary>
        /// <param name="ciphertext">加密内容</param>
        /// <param name="nonce">随机串</param>
        /// <param name="associated_data">附加数据</param>
        /// <returns></returns>
        public static string AesGcmDecrypt(string ciphertext, string nonce, string? associated_data)
        {
            Encoding encoding = Encoding.UTF8;
            string key = AppSettings.Settings.APIKey;

            byte[] keyBytes = encoding.GetBytes(key);
            byte[] nonceBytes = encoding.GetBytes(nonce);
            byte[] ciphertextBytes = Convert.FromBase64String(ciphertext);

            byte[]? associated_dataBytes = null;
            if (!string.IsNullOrEmpty(associated_data))
            {
                associated_dataBytes = encoding.GetBytes(associated_data);
            }

            byte[] cipherText = new byte[ciphertextBytes.Length - 16];
            Array.Copy(ciphertextBytes, 0, cipherText, 0, cipherText.Length);

            byte[] tag = new byte[16];
            Array.Copy(ciphertextBytes, cipherText.Length, tag, 0, 16);

            AesGcm aesGcm = new(keyBytes);
            byte[] plaintext = new byte[cipherText.Length];
            aesGcm.Decrypt(nonceBytes, cipherText, tag, plaintext, associated_dataBytes);

            return encoding.GetString(plaintext);
        }

        /// <summary>
        /// 初始化微信RSA签名信息
        /// </summary>
        /// <param name="rsa">公钥RSA对象</param>
        /// <param name="serialNo">微信支付证书序列号</param>
        internal static void InitWxRSAInfo(RSA? rsa, string? serialNo)
        {
            wxRsa = rsa;
            wxSerialNo = serialNo;
            notAfter = DateTime.Now.AddHours(12);
        }
    }
}