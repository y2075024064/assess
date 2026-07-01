using WeChatTool.Business.Base;
using Infrastructure.Web;
using WeChatTool.Model;
using System.Text.Json;
using WeChatTool.Lib;
using System.Text;

namespace WeChatTool.Business
{
    /// <summary>
    /// 公众号群发消息服务类
    /// </summary>
    public class WxMassMsg : BaseService
    {
        /// <summary>
        /// 实例群发消息服务
        /// </summary>
        /// <param name="cacheManage">缓存管理器</param>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">AppSecret</param>
        public WxMassMsg(IWXCacheManage cacheManage, string appId, string appSecret)
            : base(cacheManage, appId, appSecret)
        {
        }

        /// <summary>
        /// 发送群发消息
        /// </summary>
        /// <param name="param">推送信息</param>
        /// <returns>发送结果信息</returns>
        public async Task<MsgSendResult> Send(PushMsgBaseModel<string[]> param)
        {
            MsgSendResult result = new();

            try
            {
                string token = await cacheManage.GetTokenAsync(appId, appSecret);

                string url = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token=" + token;

                string jsonParam = JsonSerializer.Serialize(param);
                HttpContent content = new StringContent(jsonParam, Encoding.UTF8, "application/json");
                string res = await HttpRequestTool.HttpPostAsync(url, content);
                if (string.IsNullOrWhiteSpace(res))
                {
                    result.errcode = -1;
                    result.errmsg = "微信请求失败！";
                    return result;
                }

                MsgSendResult? _result = JsonSerializer.Deserialize<MsgSendResult>(res);
                if (_result != null) { result = _result; }
            }
            catch (Exception ex)
            {
                result.errcode = 1;
                result.errmsg = "发送群发消息失败。" + ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 预览群发消息
        /// </summary>
        /// <param name="param">推送信息</param>
        /// <returns>预览结果信息</returns>
        public async Task<MsgSendResult> PreviewPushMsg(PushMsgBaseModel<string> param)
        {
            MsgSendResult result = new();

            try
            {
                string token = await cacheManage.GetTokenAsync(appId, appSecret);

                string url = "https://api.weixin.qq.com/cgi-bin/message/mass/preview?access_token=" + token;

                string jsonParam = JsonSerializer.Serialize(param);
                HttpContent content = new StringContent(jsonParam, Encoding.UTF8, "application/json");
                string res = await HttpRequestTool.HttpPostAsync(url, content);
                if (string.IsNullOrWhiteSpace(res))
                {
                    result.errcode = -1;
                    result.errmsg = "微信请求失败！";
                    return result;
                }

                MsgSendResult? _result = JsonSerializer.Deserialize<MsgSendResult>(res);
                if (_result != null) { result = _result; }
            }
            catch (Exception ex)
            {
                result.errcode = 1;
                result.errmsg = "预览群发消息失败。" + ex.Message;
            }

            return result;
        }
    }
}