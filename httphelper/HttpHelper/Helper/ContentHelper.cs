using System.Text.RegularExpressions;

namespace HttpHelper
{
    /// <summary>
    /// 文本内容帮助类
    /// </summary>
    public static class ContentHelper
    {
        /// <summary>
        /// 替换\r|\n|\s|\t为空字符串
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <returns>结果</returns>
        public static string ReplaceBlank(this string content)
        {
            return Regex.Replace(content, @"\r|\n|\s|\t", string.Empty);
        }
    }
}
