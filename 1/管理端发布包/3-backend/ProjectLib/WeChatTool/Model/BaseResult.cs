namespace WeChatTool.Model
{
    /// <summary>
    /// 微信基础结果模型
    /// </summary>
    public class BaseResult
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int errcode { get; set; } = 0;

        /// <summary>
        /// 错误字符串代码
        /// </summary>
        public string code { get; set; } = "SUCCESS";

        /// <summary>
        /// 错误说明
        /// </summary>
        public string errmsg { get; set; } = "OK";

        /// <summary>
        /// 错误说明
        /// </summary>
        public string message { get; set; } = "成功";

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSucc => errcode == 0 && code == "SUCCESS";
    }
}