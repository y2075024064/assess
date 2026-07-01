using System.Security.Cryptography;
using System.Text;

namespace WeChatTool.JSSDK
{
    /// <summary>
    /// 签名类
    /// </summary>
    public class Signature
    {
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="data">参与签名的参数字典</param>
        /// <returns>签名</returns>
        public string Sign(Dictionary<string, string> data)
        {
            var dataList = data.ToList();

            dataList.Sort(ParameterKeyComparison);
            var queryString = dataList.Aggregate(string.Empty, (query, item) => string.Concat(query, "&", item.Key, "=", item.Value)).TrimStart('&');

            return Sign(queryString);
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="str">签名字符串</param>
        /// <returns></returns>
        public string Sign(string str)
        {
            using var sha1 = SHA1.Create();
            var hashed = sha1.ComputeHash(Encoding.UTF8.GetBytes(str));
            return HexStringFromBytes(hashed);
        }

        private string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        private int ParameterKeyComparison(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
        {
            return x.Key.CompareTo(y.Key);
        }
    }
}