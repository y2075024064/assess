using WeChatTool.Business.Base;
using Infrastructure.Web;
using WeChatTool.Model;
using System.Text.Json;
using WeChatTool.Lib;
using System.Text;

namespace WeChatTool.Business
{
    /// <summary>
    /// 小程序码服务类
    /// </summary>
    public class MiniWxACode : BaseService
    {
        /// <summary>
        /// 实例小程序码服务
        /// </summary>
        /// <param name="cacheManage">缓存管理器</param>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">AppSecret</param>
        public MiniWxACode(IWXCacheManage cacheManage, string appId, string appSecret)
            : base(cacheManage, appId, appSecret)
        {
        }

        /// <summary>
        /// 获取小程序码，适用于需要的码数量极多的业务场景
        /// </summary>
        /// <param name="scene">携带参数</param>
        /// <param name="page">跳转页面，默认 主页</param>
        /// <param name="width">二维码的宽度，单位 px，最小 280px，最大 1280px，默认 430</param>
        /// <param name="auto_color">自动配置线条颜色，如果颜色依然是黑色，则说明不建议配置主色调，默认 false</param>
        /// <param name="line_color">auto_color 为 false 时生效，使用 rgb 设置颜色 例如 {r:xxx,g:xxx,b:xxx} 十进制表示，默认 {r:0,g:0,b:0}</param>
        /// <param name="is_hyaline">是否需要透明底色，为 true 时，生成透明底色的小程序，默认 false</param>
        /// <returns>获取结果信息</returns>
        public async Task<MiniWxACodeResult> GetUnlimited(string scene, string page, int width = 430, bool auto_color = false, object? line_color = null, bool is_hyaline = false)
        {
            MiniWxACodeResult result = new();

            try
            {
                string token = await cacheManage.GetTokenAsync(appId, appSecret);

                string url = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + token;

                line_color ??= new { r = 0, g = 0, b = 0 };
                object data = new { scene, page, width, auto_color, line_color, is_hyaline };

                string jsonParam = JsonSerializer.Serialize(data);
                HttpContent content = new StringContent(jsonParam, Encoding.UTF8, "application/json");
                HttpResponseMessage res = await HttpRequestTool.HttpSendRetrunResponseAsync(HttpMethod.Post, url, content);

                string type = "";
                Stream stream = await res.Content.ReadAsStreamAsync();
                if (res.Headers.TryGetValues("Content-Type", out var values))
                {
                    type = values?.FirstOrDefault() ?? "";
                }

                if (type.Contains("image"))
                {
                    result.contentType = type;
                    MemoryStream memory = new();
                    stream.CopyTo(memory);
                    result.image = memory.ToArray();
                    memory.Close();
                }
                else
                {
                    StreamReader reader = new(stream);
                    string resStr = reader.ReadToEnd();
                    MiniWxACodeResult? _result = JsonSerializer.Deserialize<MiniWxACodeResult>(resStr);
                    if (_result != null) { result = _result; }

                    reader.Close();
                }
                stream.Close();
            }
            catch (Exception ex)
            {
                result.errcode = 1;
                result.errmsg = "获取小程序码请求失败。" + ex.Message;
            }

            return result;
        }
    }
}