namespace WeChatTool.Model
{
    #region 群发消息相关模型

    /// <summary>
    /// 推送消息基础模型
    /// <typeparam name="Type"> OpenID string[]：群发 string：预览 </typeparam>
    /// </summary>
    public abstract class PushMsgBaseModel<Type>
    {
        /// <summary>
        /// 消息的接收者（群发：一串OpenID列表，OpenID最少2个，最多10000个、预览：OpenID）
        /// </summary>
        public Type? touser { get; protected set; }

        /// <summary>
        /// 群发的消息类型，
        /// 图文消息为mpnews，文本消息为text，语音为voice，音乐为music，图片为image，视频为video，卡券为wxcard
        /// </summary>
        public string msgtype { get; protected set; } = string.Empty;
    }

    #region 图文消息

    /// <summary>
    /// 图文推送模型
    /// </summary>
    public class ImageTextModel<Type> : PushMsgBaseModel<Type>
    {
        /// <summary>
        /// 图文推送
        /// </summary>
        /// <param name="openId">消息接收者</param>
        /// <param name="media_id">媒体Id</param>
        /// <param name="send_ignore_reprint">图文消息被判定为转载时，是否继续群发。1为继续，0为停止。</param>
        public ImageTextModel(Type openId, string media_id, int send_ignore_reprint = 0)
        {
            touser = openId;
            msgtype = "mpnews";
            mpnews = new { media_id };
            this.send_ignore_reprint = send_ignore_reprint;
        }

        /// <summary>
        /// 图文信息
        /// </summary>
        public object mpnews { get; private set; }

        /// <summary>
        /// 图文消息被判定为转载时，是否继续群发。 
        /// 1为继续群发（转载），0为停止群发。
        /// </summary>
        public int send_ignore_reprint { get; private set; }
    }

    #endregion

    #region 文本消息

    /// <summary>
    /// 文本推送模型
    /// </summary>
    public class TextModel<Type> : PushMsgBaseModel<Type>
    {
        /// <summary>
        /// 文本推送
        /// </summary>
        /// <param name="openId">消息接收者</param>
        /// <param name="content">文本内容</param>
        public TextModel(Type openId, string content)
        {
            touser = openId;
            msgtype = "text";
            text = new { content };
        }

        /// <summary>
        /// 文本信息
        /// </summary>
        public object text { get; private set; }
    }

    #endregion

    #region 语音消息

    /// <summary>
    /// 语音推送模型
    /// </summary>
    public class VoiceModel<Type> : PushMsgBaseModel<Type>
    {
        /// <summary>
        /// 语音推送
        /// </summary>
        /// <param name="openId">消息接收者</param>
        /// <param name="media_id">媒体Id</param>
        public VoiceModel(Type openId, string media_id)
        {
            touser = openId;
            msgtype = "voice";
            voice = new { media_id };
        }

        /// <summary>
        /// 语音信息
        /// </summary>
        public object voice { get; private set; }
    }

    #endregion

    #region 图片消息

    /// <summary>
    /// 图片推送模型
    /// </summary>
    public class ImageModel<Type> : PushMsgBaseModel<Type>
    {
        /// <summary>
        /// 图片推送
        /// </summary>
        /// <param name="openId">消息接收者</param>
        /// <param name="media_ids">媒体Id集合</param>
        /// <param name="recommend">推荐语，不填则默认为“分享图片”</param>
        /// <param name="openComment">是否打开评论</param>
        /// <param name="onlyFansComment">是否仅粉丝可评论</param>
        public ImageModel(Type openId, string[] media_ids, string recommend, bool openComment = true, bool onlyFansComment = false)
        {
            touser = openId;
            msgtype = "image";
            images = new
            {
                media_ids,
                recommend,
                need_open_comment = openComment ? 1 : 0,
                only_fans_can_comment = onlyFansComment ? 1 : 0,
            };
        }

        /// <summary>
        /// 图片信息
        /// </summary>
        public object images { get; private set; }
    }

    #endregion

    #region 音乐消息

    /// <summary>
    /// 音乐推送模型
    /// </summary>
    public class MusicModel<Type> : PushMsgBaseModel<Type>
    {
        /// <summary>
        /// 音乐推送
        /// </summary>
        /// <param name="openId">消息接收者</param>
        /// <param name="media_id">媒体Id</param>
        public MusicModel(Type openId, string media_id)
        {
            touser = openId;
            msgtype = "music";
            music = new { media_id };
        }

        /// <summary>
        /// 音乐信息
        /// </summary>
        public object music { get; private set; }
    }

    #endregion

    #region 视频消息

    /// <summary>
    /// 视频推送模型
    /// </summary>
    public class VideoModel<Type> : PushMsgBaseModel<Type>
    {
        /// <summary>
        /// 视频推送
        /// </summary>
        /// <param name="openId">消息接收者</param>
        /// <param name="media_id">媒体Id</param>
        /// <param name="title">视频标题</param>
        /// <param name="description">视频描述</param>
        public VideoModel(Type openId, string media_id, string title, string description)
        {
            touser = openId;
            msgtype = "mpvideo";
            mpvideo = new { media_id, title, description };
        }

        /// <summary>
        /// 视频信息
        /// </summary>
        public object mpvideo { get; private set; }
    }

    #endregion

    #region 卡券消息

    /// <summary>
    /// 卡券推送模型
    /// </summary>
    public class CardModel<Type> : PushMsgBaseModel<Type>
    {
        /// <summary>
        /// 卡券推送
        /// </summary>
        /// <param name="openId">消息接收者</param>
        /// <param name="card_id">卡券Id</param>
        public CardModel(Type openId, string card_id)
        {
            touser = openId;
            msgtype = "wxcard";
            wxcard = new { card_id };
        }

        /// <summary>
        /// 卡券信息
        /// </summary>
        public object wxcard { get; private set; }
    }

    #endregion

    /// <summary>
    /// 消息发送结果模型
    /// </summary>
    public class MsgSendResult : BaseResult
    {
        /// <summary>
        /// 媒体文件类型
        /// </summary>
        public string type { get; set; } = string.Empty;

        /// <summary>
        /// 消息发送任务的ID
        /// </summary>
        public int msg_id { get; set; }

        /// <summary>
        /// 消息的数据ID
        /// </summary>
        public int msg_data_id { get; set; }
    }

    #endregion

    #region 模板消息相关模型

    /// <summary>
    /// 模板消息参数模型
    /// </summary>
    public class TemplateMsgModel
    {
        /// <summary>
        /// 模板跳转链接（海外帐号没有跳转能力）
        /// </summary>
        public string url { get; set; } = string.Empty;

        /// <summary>
        /// 接收者openid
        /// </summary>
        public string touser { get; set; } = string.Empty;

        /// <summary>
        /// 模板ID
        /// </summary>
        public string template_id { get; set; } = string.Empty;

        /// <summary>
        /// 跳小程序所需数据，不需跳小程序可不用传该数据
        /// </summary>
        public Miniprogram miniprogram { get; set; } = new Miniprogram();

        /// <summary>
        /// 模板数据
        /// </summary>
        public Dictionary<string, DataInfoModel> data { get; set; } = new Dictionary<string, DataInfoModel>();
    }

    /// <summary>
    /// 小程序所需数据模型
    /// </summary>
    public class Miniprogram
    {
        /// <summary>
        /// 所需跳转到的小程序appid
        /// </summary>
        public string appid { get; set; } = string.Empty;

        /// <summary>
        /// 所需跳转到小程序的具体页面路径，支持带参数,（示例index?foo=bar），要求该小程序已发布，暂不支持小游戏
        /// </summary>
        public string pagepath { get; set; } = string.Empty;
    }

    /// <summary>
    /// 模板数据信息模型
    /// </summary>
    public class DataInfoModel
    {
        /// <summary>
        /// 模板字段值
        /// </summary>
        public string value { get; set; } = string.Empty;

        /// <summary>
        /// 模板内容字体颜色，不填默认为黑色
        /// </summary>
        public string color { get; set; } = string.Empty;
    }

    /// <summary>
    /// 消息推送结果模型
    /// </summary>
    public class MsgPushResult : BaseResult
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public long msgid { get; set; }
    }

    #endregion
}