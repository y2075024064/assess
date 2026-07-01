using WeChatTool.Business.Base;
using Infrastructure.Web;
using System.Text.Json;
using WeChatTool.Model;
using WeChatTool.Lib;
using Assess.Entity;

namespace WeChatTool.JSSDK
{
    public class JSSDK : BaseService
    {
        private static string ticket = string.Empty;
        private static DateTime overdueTime;

        private readonly bool debug;

        /// <summary>
        /// 构建JDK对象
        /// </summary>
        /// <param name="cacheManage">缓存管理器</param>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">AppSecret</param>
        /// <param name="debug">是否调试</param>
        public JSSDK(IWXCacheManage cacheManage, string appId, string appSecret, bool debug = false)
            : base(cacheManage, appId, appSecret)
        {
            this.debug = debug;
        }

        /// <summary>
        /// 获取ApiTicket
        /// </summary>
        /// <returns>ApiTicket</returns>
        private async Task<string> GetJsApiTicketAsync()
        {
            try
            {
                DateTime nowTime = DateTime.Now;

                if (overdueTime > nowTime && !string.IsNullOrWhiteSpace(ticket))
                {
                    return ticket;
                }
                else
                {
                    var wxCache = await cacheManage.WXCacheService.GetWxCacheAsync(appId);
                    if (wxCache != null && string.IsNullOrWhiteSpace(wxCache.Jticket) && wxCache.JticketOverdueTime > nowTime)
                    {
                        ticket = wxCache.Jticket ?? "";
                        overdueTime = wxCache.JticketOverdueTime.Value;
                        return ticket;
                    }
                    else
                    {
                        string token = await cacheManage.GetTokenAsync(appId, appSecret);
                        string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket";
                        url += $"?access_token={token}&type=jsapi";

                        string result = await HttpRequestTool.HttpGetAsync(url);
                        TicketInfo? ticketInfo = JsonSerializer.Deserialize<TicketInfo>(result);

                        if (ticketInfo != null)
                        {
                            if (ticketInfo.errcode == 0)
                            {
                                ticket = ticketInfo.ticket;
                                overdueTime = nowTime.AddSeconds(ticketInfo.expires_in - 60);
                                if (wxCache != null)
                                {
                                    wxCache.Jticket = ticket;
                                    wxCache.JticketOverdueTime = overdueTime;
                                    _ = await cacheManage.WXCacheService.UpdateAsync(wxCache, nameof(WechatCache.Jticket), nameof(WechatCache.JticketOverdueTime));
                                }
                                else
                                {
                                    wxCache = new WechatCache()
                                    {
                                        AppId = appId,
                                        Jticket = ticket,
                                        JticketOverdueTime = overdueTime,
                                    };
                                    _ = await cacheManage.WXCacheService.InsertAsync(wxCache);
                                }

                                return ticket;
                            }
                            else
                            {
                                throw new WeChatException("获取 api ticket 失败，{ errcode: " + ticketInfo.errcode + ", errmsg: " + ticketInfo.errmsg + " }");
                            }
                        }
                        else { throw new WeChatException("获取 api ticket 失败，无信息返回"); }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new WeChatException("获取 api ticket 失败，服务器异常", ex);
            }
        }

        /// <summary>
        /// 获取jssdk签名配置对象
        /// </summary>
        /// <param name="url">当前页面url</param>
        /// <param name="jsapi">JsApiEnum,如:JsApiEnum.scanQRCode|JsApiEnum.onMenuShareQQ</param>
        /// <returns>微信公众平台JsSdk的配置对象</returns>
        public async Task<SignPackage> GetSignPackageAsync(string url, JsApiEnum jsapi)
        {
            return await GetSignPackageAsync(url, jsapi.ToString().Replace(" ", "").Split(','));
        }

        /// <summary>
        /// 获取jssdk签名配置对象
        /// </summary>
        /// <param name="url">当前页面url</param>
        /// <param name="jsapi">JsApiEnum,如:scanQRCode,onMenuShareQQ</param>
        /// <returns>微信公众平台JsSdk的配置对象</returns>
        public async Task<SignPackage> GetSignPackageAsync(string url, string[] jsapi)
        {
            string noncestr = Path.GetRandomFileName();
            string jsapi_ticket = await GetJsApiTicketAsync();

            long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

            Dictionary<string, string> signData = new()
            {
                { "url", url },
                { "noncestr", noncestr },
                { "jsapi_ticket", jsapi_ticket },
                { "timestamp", timestamp.ToString() },
            };

            SignPackage result = new()
            {
                appId = appId,
                debug = debug,
                jsApiList = jsapi,
                nonceStr = noncestr,
                timestamp = timestamp,
                signature = new Signature().Sign(signData),
            };

            return result;
        }

        #region 蓝牙

        /// <summary>
        /// 获取蓝牙jssdk签名配置对象
        /// </summary>
        /// <param name="url">当前页面url</param>
        /// <returns>微信公众平台JsSdk的配置对象</returns>
        public async Task<SignPackage> GetSignPackageBluetoothAsync(string url)
        {
            return await GetSignPackageAsync(url, new[] {
                "openWXDeviceLib",
                "closeWXDeviceLib",
                "getWXDeviceInfos",
                "sendDataToWXDevice",
                "startScanWXDevice",
                "stopScanWXDevice",
                "connectWXDevice",
                "disconnectWXDevice",
                "getWXDeviceTicket",
                "getLocation",
                "scanQRCode",
                "onWXDeviceBindStateChange",
                "onWXDeviceStateChange",
                "onReceiveDataFromWXDevice",
                "onScanWXDeviceResult",
                "onWXDeviceBluetoothStateChange",
            });
        }

        #endregion
    }
}