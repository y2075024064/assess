using Infrastructure.Web;
using WeChatTool.Model;
using System.Text.Json;
using Assess.Entity;

namespace WeChatTool.Lib
{
    /// <summary>
    /// 微信ACCESS_TOKEN管理类
    /// </summary>
    public class WXCacheManage : IWXCacheManage
    {
        private static readonly List<AppTokenInfo> AppTokens = new();

        /// <summary>
        /// 微信缓存服务
        /// </summary>
        public IWxCacheService WXCacheService { get; private set; }

        /// <summary>
        /// 构建微信ACCESS_TOKEN管理类
        /// </summary>
        /// <param name="service">服务</param>
        public WXCacheManage(IWxCacheService service)
        {
            WXCacheService = service;
        }

        /// <summary>
        /// 获取对应应用Token
        /// </summary>
        /// <param name="appId">AppID</param>
        /// <param name="appSecret">AppSecret</param>
        /// <exception cref="WeChatException"></exception>
        /// <returns>access_token</returns>
        public async Task<string> GetTokenAsync(string appId, string appSecret)
        {
            var model = AppTokens.Find(e => e.appId == appId);
            if (model != null && model.overdueTime > DateTime.Now)
            {
                return model.token;
            }
            else
            {
                if (model != null) { AppTokens.Remove(model); }

                model = await GetAccessTokenAsync(appId, appSecret);
                if (model == null)
                {
                    throw new WeChatException("获取 access_token 失败，请求中断");
                }
                else
                {
                    AppTokens.Add(model);
                    return model.token;
                }
            }
        }

        /// <summary>
        /// 获取网页授权Token
        /// </summary>
        /// <param name="openid">Openid</param>
        /// <param name="code">Code</param>
        /// <param name="appId">AppID</param>
        /// <param name="appSecret">AppSecret</param>
        /// <returns>网页授权access_token</returns>
        public async Task<AuthTokenModel> GetAuthTokenAsync(string openid, string code, string appId, string appSecret)
        {
            AuthTokenModel model;
            DateTime nowTime = DateTime.Now;

            WechatMemberCache? wxCache = await WXCacheService.GetWxUserCacheAsync(openid);
            if (wxCache != null && string.IsNullOrWhiteSpace(wxCache.Atoken) && wxCache.AtokenOverdueTime > nowTime)
            {
                model = new AuthTokenModel()
                {
                    scope = wxCache.Scope,
                    token = wxCache.Atoken,
                    openid = wxCache.OpenId,
                    overdueTime = (DateTime)wxCache.AtokenOverdueTime,
                };
            }
            else
            {
                AuthTokenInfo result;
                if (wxCache != null && string.IsNullOrWhiteSpace(wxCache.Rtoken) && wxCache.RtokenOverdueTime > nowTime)
                {
                    result = await RefreshAuth_Access_TokenAsync(appId, wxCache.Rtoken ?? "");
                    if (result.errcode != 0)
                    {
                        throw new WeChatException("{ errcode: " + result.errcode + ", errmsg: \"" + result.errmsg + "\" }");
                    }
                }
                else
                {
                    result = await GetAuth_Access_TokenAsync(appId, appSecret, code);
                    if (result.errcode != 0)
                    {
                        throw new WeChatException("{ errcode: " + result.errcode + ", errmsg: \"" + result.errmsg + "\" }");
                    }
                }

                if (wxCache != null)
                {
                    wxCache.Scope = result.scope;
                    wxCache.OpenId = result.openid;
                    wxCache.Atoken = result.access_token;
                    wxCache.AtokenOverdueTime = nowTime.AddSeconds(result.expires_in - 60);
                    List<string> fileds = new()
                    {
                        nameof(WechatMemberCache.Scope),
                        nameof(WechatMemberCache.OpenId),
                        nameof(WechatMemberCache.Atoken),
                        nameof(WechatMemberCache.AtokenOverdueTime)
                    };

                    if (wxCache.Rtoken != result.refresh_token)
                    {
                        wxCache.Rtoken = result.refresh_token;
                        wxCache.RtokenOverdueTime = nowTime.AddDays(29);
                        fileds.AddRange(new[] { nameof(WechatMemberCache.Rtoken), nameof(WechatMemberCache.RtokenOverdueTime) });
                    }

                    WXCacheService.Adapter.Update(wxCache, fileds.ToArray());
                }
                else
                {
                    wxCache = new WechatMemberCache()
                    {
                        Scope = result.scope,
                        OpenId = result.openid,
                        Atoken = result.access_token,
                        Rtoken = result.refresh_token,
                        RtokenOverdueTime = nowTime.AddDays(29),
                        AtokenOverdueTime = nowTime.AddSeconds(result.expires_in - 60),
                    };
                    WXCacheService.Adapter.Insert(wxCache);
                }

                model = new AuthTokenModel()
                {
                    scope = result.scope,
                    openid = result.openid,
                    token = result.access_token,
                    overdueTime = nowTime.AddSeconds(result.expires_in - 60),
                };
            }

            return model;
        }

        /// <summary>
        /// 获取应用Token信息
        /// </summary>
        /// <param name="appId">AppID</param>
        /// <param name="appSecret">AppSecret</param>
        /// <exception cref="WeChatException"></exception>
        /// <returns>Token信息</returns>
        private async Task<AppTokenInfo> GetAccessTokenAsync(string appId, string appSecret)
        {
            AppTokenInfo result;

            try
            {
                DateTime nowTime = DateTime.Now;
                WechatCache? wxCache = await WXCacheService.GetWxCacheAsync(appId);
                if (wxCache != null && !string.IsNullOrWhiteSpace(wxCache.Token) && wxCache.TokenOverdueTime > nowTime)
                {
                    result = new AppTokenInfo() { appId = appId, token = wxCache.Token, overdueTime = wxCache.TokenOverdueTime.Value };
                }
                else
                {
                    GetTokenResult tokenResult = await GetAccess_TokenAsync(appId, appSecret);
                    if (tokenResult.errcode != 0)
                    {
                        throw new WeChatException("{ errcode: " + tokenResult.errcode + ", errmsg: \"" + tokenResult.errmsg + "\" }");
                    }
                    else
                    {
                        result = new AppTokenInfo()
                        {
                            appId = appId,
                            token = tokenResult.access_token,
                            overdueTime = nowTime.AddSeconds(tokenResult.expires_in - 60),
                        };

                        if (wxCache != null)
                        {
                            wxCache.Token = result.token;
                            wxCache.TokenOverdueTime = result.overdueTime;
                            WXCacheService.Update(wxCache, nameof(WechatCache.Token), nameof(WechatCache.TokenOverdueTime));
                        }
                        else
                        {
                            wxCache = new WechatCache() { AppId = appId, Token = result.token, TokenOverdueTime = result.overdueTime };
                            WXCacheService.Insert(wxCache);
                        }
                    }
                }
            }
            catch (WeChatException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new WeChatException("获取 access_token 失败", ex);
            }

            return result;
        }

        /// <summary>
        /// 请求微信接口获取Token信息
        /// </summary>
        /// <param name="appId">AppID</param>
        /// <param name="appSecret">AppSecret</param>
        /// <returns></returns>
        private async static Task<GetTokenResult> GetAccess_TokenAsync(string appId, string appSecret)
        {
            GetTokenResult result = new();

            try
            {
                string url = "https://api.weixin.qq.com/cgi-bin/token";
                url += $"?grant_type=client_credential&appid={appId}&secret={appSecret}";

                string res = await HttpRequestTool.HttpGetAsync(url);
                if (string.IsNullOrWhiteSpace(res))
                {
                    result.errcode = -1;
                    result.errmsg = "微信请求失败！";
                    return result;
                }

                GetTokenResult? authToken = JsonSerializer.Deserialize<GetTokenResult>(res);
                if (authToken != null) { result = authToken; }
            }
            catch (Exception ex)
            {
                result.errcode = 1;
                result.errmsg = "获取 access_token 失败。" + ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 通过 code 获取网页授权Token
        /// </summary>
        /// <param name="appId">AppID</param>
        /// <param name="appSecret">AppSecret</param>
        /// <param name="code">Code</param>
        /// <returns>网页授权Token信息</returns>
        private async static Task<AuthTokenInfo> GetAuth_Access_TokenAsync(string appId, string appSecret, string code)
        {
            AuthTokenInfo result = new();

            try
            {
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token";
                url += $"?grant_type=authorization_code&appid={appId}&secret={appSecret}&code={code}";

                string res = await HttpRequestTool.HttpGetAsync(url);
                if (string.IsNullOrWhiteSpace(res))
                {
                    result.errcode = -1;
                    result.errmsg = "微信请求失败！";
                    return result;
                }

                AuthTokenInfo? authToken = JsonSerializer.Deserialize<AuthTokenInfo>(res);
                if (authToken != null) { result = authToken; }
            }
            catch (Exception ex)
            {
                result.errcode = 1;
                result.errmsg = "获取网页授权Token信息失败。" + ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 刷新网页授权Token
        /// </summary>
        /// <param name="appId">AppID</param>
        /// <param name="token">refresh_token</param>
        /// <returns>网页授权Token信息</returns>
        private async static Task<AuthTokenInfo> RefreshAuth_Access_TokenAsync(string appId, string token)
        {
            AuthTokenInfo result = new();

            try
            {
                string url = "https://api.weixin.qq.com/sns/oauth2/refresh_token";
                url += $"?appid={appId}&grant_type=refresh_token&refresh_token={token}";

                string res = await HttpRequestTool.HttpGetAsync(url);
                if (string.IsNullOrWhiteSpace(res))
                {
                    result.errcode = -1;
                    result.errmsg = "微信请求失败！";
                    return result;
                }

                AuthTokenInfo? authToken = JsonSerializer.Deserialize<AuthTokenInfo>(res);
                if (authToken != null) { result = authToken; }
            }
            catch (Exception ex)
            {
                result.errcode = 1;
                result.errmsg = "刷新网页授权Token信息失败。" + ex.Message;
            }

            return result;
        }
    }
}