using WeChatTool.Business.Base;
using Infrastructure.Web;
using WeChatTool.Model;
using System.Text.Json;
using WeChatTool.Lib;
using System.Text;

namespace WeChatTool.Business
{
    /// <summary>
    /// 小程序订阅消息服务类
    /// </summary>
    public class MiniSubscribeMsg : BaseService
    {
        /// <summary>
        /// 实例小程序订阅消息服务
        /// </summary>
        /// <param name="cacheManage">缓存管理器</param>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">AppSecret</param>
        public MiniSubscribeMsg(IWXCacheManage cacheManage, string appId, string appSecret)
            : base(cacheManage, appId, appSecret)
        {
        }

        /// <summary>
        /// 发送订阅消息
        /// </summary>
        /// <param name="model">消息信息</param>
        /// <returns>是否成功</returns>
        public async Task<BaseResult> SendAsync(SubscribeMsgModel model)
        {
            BaseResult result = new();

            try
            {
                string token = await cacheManage.GetTokenAsync(appId, appSecret);

                string url = "https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token=" + token;

                string jsonParam = JsonSerializer.Serialize(model);
                HttpContent content = new StringContent(jsonParam, Encoding.UTF8, "application/json");
                string res = await HttpRequestTool.HttpPostAsync(url, content);
                if (string.IsNullOrWhiteSpace(res))
                {
                    result.errcode = -1;
                    result.errmsg = "微信请求失败！";
                    return result;
                }

                BaseResult? _result = JsonSerializer.Deserialize<BaseResult>(res);
                if (_result != null) { result = _result; }
            }
            catch (Exception ex)
            {
                result.errcode = 1;
                result.errmsg = "发送订阅消息失败。" + ex.Message;
            }

            return result;
        }
    }
}