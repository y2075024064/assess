using WeChatTool.Business.Base;
using Infrastructure.Web;
using WeChatTool.Model;
using System.Text.Json;
using WeChatTool.Lib;
using System.Text;

namespace WeChatTool.Business
{
    /// <summary>
    /// 公众号模板消息服务类
    /// </summary>
    public class WxTemplateMsg : BaseService
    {
        /// <summary>
        /// 实例公众号模板消息服务
        /// </summary>
        /// <param name="cacheManage">缓存管理器</param>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">AppSecret</param>
        public WxTemplateMsg(IWXCacheManage cacheManage, string appId, string appSecret)
            : base(cacheManage, appId, appSecret)
        {
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="param">推送信息</param>
        /// <returns>发送结果信息</returns>
        public async Task<MsgPushResult> Send(TemplateMsgModel param)
        {
            MsgPushResult result = new();

            try
            {
                string token = await cacheManage.GetTokenAsync(appId, appSecret);

                string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token;

                string jsonParam = JsonSerializer.Serialize(param);
                HttpContent content = new StringContent(jsonParam, Encoding.UTF8, "application/json");
                string res = await HttpRequestTool.HttpPostAsync(url, content);
                if (string.IsNullOrWhiteSpace(res))
                {
                    result.errcode = -1;
                    result.errmsg = "微信请求失败！";
                    return result;
                }

                MsgPushResult? _result = JsonSerializer.Deserialize<MsgPushResult>(res);
                if (_result != null) { result = _result; }
            }
            catch (Exception ex)
            {
                result.errcode = 1;
                result.errmsg = "发送模板消息失败。" + ex.Message;
            }

            return result;
        }
    }
}