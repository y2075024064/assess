namespace WeChatTool.Model
{
    /// <summary>
    /// 小程序码返回模型
    /// </summary>
    public class MiniWxACodeResult : BaseResult
    {
        /// <summary>
        /// 图片对象
        /// </summary>
        public byte[] image { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// 数据类型
        /// </summary>
        public string contentType { get; set; } = string.Empty;
    }
}