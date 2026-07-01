namespace WeChatTool
{
    /// <summary>
    /// 微信相关异常类
    /// </summary>
    public class WeChatException : Exception
    {
        /// <summary>
        /// 异常信息
        /// </summary>
        public new string Message { get; set; } = string.Empty;

        /// <summary>
        /// 导致异常的异常
        /// </summary>
        public new Exception? InnerException { get; set; }

        /// <summary>
        /// 微信异常
        /// </summary>
        /// <param name="message">异常信息</param>
        public WeChatException(string message) : base(message)
        {
            Message = message;
        }

        /// <summary>
        /// 微信异常
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="innerException">导致异常的异常</param>
        public WeChatException(string message, Exception innerException) : base(message, innerException)
        {
            Message = message;
            InnerException = innerException;
        }
    }
}