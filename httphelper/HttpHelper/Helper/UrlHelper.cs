using System.Text;
using System.Web;

namespace HttpHelper
{
    /// <summary>
    /// url相关操作帮助类
    /// </summary>
    public static class UrlHelper
    {
        /// <summary>
        /// url 编码,默认编码方式为Utf8
        /// </summary>
        /// <param name="content">待编码内容</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>编码之后的结果</returns>
        public static string UrlEncode(this string content, Encoding encoding = null)
        {
            if (encoding == null)
            {
                return HttpUtility.UrlEncode(content, Encoding.UTF8);
            }

            return HttpUtility.UrlEncode(content, encoding);
        }

        /// <summary>
        /// url解码，默认解码方式为Utf8
        /// </summary>
        /// <param name="content">待编码内容</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>编码之后的结果</returns>
        public static string UrlDecode(this string content, Encoding encoding = null)
        {
            if (encoding == null)
            {
                return HttpUtility.UrlDecode(content, Encoding.UTF8);
            }

            return HttpUtility.UrlDecode(content, encoding);
        }
    }
}
