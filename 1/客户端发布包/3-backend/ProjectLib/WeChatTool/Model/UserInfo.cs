namespace WeChatTool.Model
{
    /// <summary>
    /// 用户高级信息模型
    /// </summary>
    public class UserAInfoModel : BaseResult
    {
        /// <summary>
        /// 用户是否订阅该公众号（0：没有关注、1：已关注）
        /// </summary>
        public int subscribe { get; set; }

        /// <summary>
        /// 用户公众号唯一标识
        /// </summary>
        public string openid { get; set; } = string.Empty;

        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string nickname { get; set; } = string.Empty;

        /// <summary>
        /// 用户的性别（1：男性、2：女性、0：未知）
        /// </summary>
        public int sex { get; set; }

        /// <summary>
        /// 用户的语言，简体中文为zh_CN
        /// </summary>
        public string language { get; set; } = string.Empty;

        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string city { get; set; } = string.Empty;

        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string province { get; set; } = string.Empty;

        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string country { get; set; } = string.Empty;

        /// <summary>
        /// 用户头像
        /// </summary>
        public string headimgurl { get; set; } = string.Empty;

        /// <summary>
        /// 用户关注时间
        /// </summary>
        public int? subscribe_time { get; set; }

        /// <summary>
        /// 开放平台唯一标识
        /// </summary>
        public string unionid { get; set; } = string.Empty;

        /// <summary>
        /// 公众号运营者对粉丝的备注
        /// </summary>
        public string remark { get; set; } = string.Empty;

        /// <summary>
        /// 用户所在的分组ID（兼容旧的用户分组接口）
        /// </summary>
        public int groupid { get; set; }

        /// <summary>
        /// 用户被打上的标签ID列表
        /// </summary>
        public int[] tagid_list { get; set; } = Array.Empty<int>();

        /// <summary>
        /// 用户关注的渠道来源
        /// </summary>
        public string subscribe_scene { get; set; } = string.Empty;

        /// <summary>
        /// 二维码扫码场景
        /// </summary>
        public int? qr_scene { get; set; }

        /// <summary>
        /// 二维码扫码场景描述
        /// </summary>
        public string qr_scene_str { get; set; } = string.Empty;
    }

    /// <summary>
    /// 用户中级信息模型
    /// </summary>
    public class UserMInfoModel : BaseResult
    {
        /// <summary>
        /// 用户公众号唯一标识
        /// </summary>
        public string openid { get; set; } = string.Empty;

        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string nickname { get; set; } = string.Empty;

        /// <summary>
        /// 用户的性别（1：男性、2：女性、0：未知）
        /// </summary>
        public int sex { get; set; }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string city { get; set; } = string.Empty;

        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string province { get; set; } = string.Empty;

        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string country { get; set; } = string.Empty;

        /// <summary>
        /// 用户头像
        /// </summary>
        public string headimgurl { get; set; } = string.Empty;

        /// <summary>
        /// 用户特权信息
        /// </summary>
        public string[] privilege { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 开放平台唯一标识
        /// </summary>
        public string unionid { get; set; } = string.Empty;
    }

    /// <summary>
    /// 微信登录用户信息模型
    /// </summary>
    public class WxLoginUserInfo : BaseResult
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string openid { get; set; } = string.Empty;

        /// <summary>
        /// 会话密钥
        /// </summary>
        public string session_key { get; set; } = string.Empty;

        /// <summary>
        /// 用户在开放平台的唯一标识符，在满足 UnionID 下发条件的情况下会返回
        /// </summary>
        public string unionid { get; set; } = string.Empty;
    }

    /// <summary>
    /// 小程序用户信息
    /// </summary>
    public class MiniUserInfoModel
    {
        public string openId { get; set; } = string.Empty;

        public string? unionId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string nickName { get; set; } = string.Empty;

        /// <summary>
        /// 用户性别
        /// </summary>
        public int gender { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        public string language { get; set; } = string.Empty;

        /// <summary>
        /// 用户城市
        /// </summary>
        public string city { get; set; } = string.Empty;

        /// <summary>
        /// 用户省份
        /// </summary>
        public string province { get; set; } = string.Empty;

        /// <summary>
        /// 用户国籍
        /// </summary>
        public string country { get; set; } = string.Empty;

        /// <summary>
        /// 头像地址
        /// </summary>
        public string avatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// 水印信息
        /// </summary>
        public WaterMarkModel watermark { get; set; } = new WaterMarkModel();
    }

    /// <summary>
    /// 小程序手机号码信息
    /// </summary>
    public class MiniPhoneInfoModel : BaseResult
    {
        /// <summary>
        /// 手机号信息
        /// </summary>
        public PhoneInfoModel phone_info { get; set; } = new PhoneInfoModel();
    }

    /// <summary>
    /// 手机号详情模型
    /// </summary>
    public class PhoneInfoModel
    {
        /// <summary>
        /// 用户绑定的手机号（国外手机号会有区号）
        /// </summary>
        public string phoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// 没有区号的手机号
        /// </summary>
        public string purePhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// 区号
        /// </summary>
        public string countryCode { get; set; } = string.Empty;

        /// <summary>
        /// 水印信息
        /// </summary>
        public WaterMarkModel watermark { get; set; } = new WaterMarkModel();
    }

    /// <summary>
    /// 水印信息模型
    /// </summary>
    public class WaterMarkModel
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string appid { get; set; } = string.Empty;

        /// <summary>
        /// 时间戳
        /// </summary>
        public int timestamp { get; set; }
    }
}