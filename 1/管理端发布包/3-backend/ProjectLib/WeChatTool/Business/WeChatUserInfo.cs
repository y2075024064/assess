using System.Security.Cryptography;
using WeChatTool.Business.Base;
using Infrastructure.Web;
using WeChatTool.Model;
using System.Text.Json;
using WeChatTool.Lib;
using System.Text;
using System.Web;

namespace WeChatTool.Business
{
    /// <summary>
    /// 微信用户信息服务类
    /// </summary>
    public class WeChatUserInfo : BaseService
    {
        /// <summary>
        /// 实例用户信息服务
        /// </summary>
        /// <param name="cacheManage">缓存管理器</param>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">AppSecret</param>
        public WeChatUserInfo(IWXCacheManage cacheManage, string appId, string appSecret)
            : base(cacheManage, appId, appSecret)
        {
        }

        #region 用 code 获取用户信息

        /// <summary>
        /// 通过 code 换取网页授权 token
        /// </summary>
        /// <param name="code">code</param>
        /// <returns>网页授权Token信息</returns>
        public async Task<UserMInfoModel> GetInfoByCode(string code)
        {
            UserMInfoModel result = new();

            try
            {
                AuthTokenModel model = await cacheManage.GetAuthTokenAsync("", code, appId, appSecret);
                if (model.scope?.Contains("snsapi_userinfo") == true)
                {
                    string url = "https://api.weixin.qq.com/sns/userinfo";
                    url += $"?access_token={model.token}&openid={model.openid}&lang=zh_CN";

                    string res = await HttpRequestTool.HttpGetAsync(url);
                    if (string.IsNullOrWhiteSpace(res))
                    {
                        result.errcode = -1;
                        result.errmsg = "微信请求失败！";
                        return result;
                    }

                    UserMInfoModel? _result = JsonSerializer.Deserialize<UserMInfoModel>(res);
                    if (_result != null) { result = _result; }
                }
                else
                {
                    result.errcode = 2;
                    result.errmsg = "用户授权作用域不支持拉取用户信息，scope：" + model.scope;
                }
            }
            catch (WeChatException ex)
            {
                result.errcode = 1;
                result.errmsg = ex.Message;
            }
            catch (Exception ex)
            {
                result.errcode = 1;
                result.errmsg = "获取网页授权Token信息失败。" + ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 通过 code 登录小程序
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns></returns>
        public async Task<WxLoginUserInfo> LoginMiniByCodeAsync(string code)
        {
            WxLoginUserInfo info = new();

            if (string.IsNullOrEmpty(code))
            {
                info.errcode = 40029;
                info.errmsg = "code 无效";
            }
            else
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session";
                url += $"?appid={appId}&secret={appSecret}&js_code={code}&grant_type=authorization_code";

                string res = await HttpRequestTool.HttpGetAsync(url);
                if (string.IsNullOrWhiteSpace(res))
                {
                    info.errcode = -1;
                    info.errmsg = "微信请求失败！";
                    return info;
                }

                WxLoginUserInfo? result = JsonSerializer.Deserialize<WxLoginUserInfo>(res);
                if (result != null) { info = result; }
                else { info.errcode = -1; info.errmsg = "系统繁忙，请稍后在试"; }
            }

            return info;
        }

        #endregion

        #region 用 code 获取用户手机号

        /// <summary>
        /// 通过 code 获取用户手机号
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns></returns>
        public async Task<MiniPhoneInfoModel> GetUserPhonenumberAsync(string code)
        {
            MiniPhoneInfoModel info = new();

            if (string.IsNullOrEmpty(code))
            {
                info.errcode = 40029;
                info.errmsg = "code 无效";
            }
            else
            {
                string token = await cacheManage.GetTokenAsync(appId, appSecret);

                string url = "https://api.weixin.qq.com/wxa/business/getuserphonenumber";
                url += "?access_token=" + token;

                string jsonParam = JsonSerializer.Serialize(new { code });
                HttpContent content = new StringContent(jsonParam, Encoding.UTF8, "application/json");
                string res = await HttpRequestTool.HttpPostAsync(url, content);
                if (string.IsNullOrWhiteSpace(res))
                {
                    info.errcode = -1;
                    info.errmsg = "微信请求失败！";
                    return info;
                }

                MiniPhoneInfoModel? result = JsonSerializer.Deserialize<MiniPhoneInfoModel>(res);
                if (result != null) { info = result; }
                else { info.errcode = -1; info.errmsg = "系统繁忙，请稍后在试"; }
            }

            return info;
        }

        #endregion

        #region 用 OpenId 换取用户信息

        /// <summary>
        /// 用openid换取用户信息
        /// </summary>
        /// <param name="openId">用户对应标识</param>
        /// <returns>用户信息</returns>
        public async Task<UserAInfoModel> GetInfoByOpenId(string openId)
        {
            UserAInfoModel result = new();

            try
            {
                //获取access_token
                string token = await cacheManage.GetTokenAsync(appId, appSecret);

                string url = "https://api.weixin.qq.com/cgi-bin/user/info";
                url += $"?access_token={token}&openid={openId}&lang=zh_CN";

                string res = await HttpRequestTool.HttpGetAsync(url);
                if (string.IsNullOrWhiteSpace(res))
                {
                    result.errcode = -1;
                    result.errmsg = "微信请求失败！";
                    return result;
                }

                UserAInfoModel? _result = JsonSerializer.Deserialize<UserAInfoModel>(res);
                if (_result != null) { result = _result; }
            }
            catch (Exception ex)
            {
                result.errcode = 1;
                result.errmsg = "换取用户信息失败。" + ex.Message;
            }

            return result;
        }

        #endregion

        #region 通过加密数据提取用户信息

        /// <summary>
        /// 通过加密数据提取用户信息
        /// </summary>
        /// <param name="iv">解密向量</param>
        /// <param name="key">解密秘钥</param>
        /// <param name="cipher">密文</param>
        /// <returns></returns>
        public static MiniUserInfoModel? GetInfoByEncryptedData(string iv, string key, string cipher)
        {
            string result = EncryptData(iv, key, cipher);
            MiniUserInfoModel? info = JsonSerializer.Deserialize<MiniUserInfoModel>(result);

            return info;
        }

        /// <summary>
        /// 解密加密数据
        /// </summary>
        /// <param name="iv">解密向量</param>
        /// <param name="key">解密秘钥</param>
        /// <param name="cipher">密文</param>
        /// <returns></returns>
        private static string EncryptData(string iv, string key, string cipher)
        {
            byte[] keyBytes = new byte[16];
            byte[] aesIV = Convert.FromBase64String(iv);
            byte[] aesKey = Convert.FromBase64String(key);
            byte[] aesCipher = Convert.FromBase64String(cipher);
            aesKey.CopyTo(keyBytes, 0);

            Aes aes = Aes.Create();
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            aes.Key = keyBytes;
            aes.IV = aesIV;

            ICryptoTransform transform = aes.CreateDecryptor();
            byte[] plainText = transform.TransformFinalBlock(aesCipher, 0, aesCipher.Length);
            string result = Encoding.UTF8.GetString(plainText);

            return result;
        }

        #endregion

        #region 其他工具

        /// <summary>
        /// 拼接 获取 code URL
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="redirect_uri">回调地址</param>
        /// <param name="state">其他参数</param>
        /// <returns></returns>
        public static string GetCodeUrl(string appId, string redirect_uri, string state = "STATE")
        {
            string redirect_url = HttpUtility.UrlEncode(redirect_uri);
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize";
            url += $"?appid={appId}&redirect_uri={redirect_url}&response_type=code&scope=snsapi_userinfo&state={state}#wechat_redirect";
            return url;
        }

        #endregion
    }
}