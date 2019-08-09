using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HttpHelper.Base
{
    /// <summary>
    /// http请求基础信息
    /// </summary>
    public class BaseHttpRequest
    {
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求方式，默认Get
        /// </summary>
        public string Method { get; set; } = "GET";

        /// <summary>
        /// 发送请求超时时间,默认5分钟
        /// </summary>
        public int TimeOut { get; set; } = 5 * 60 * 1000;

        /// <summary>
        /// 读写数据超时时间,默认5分钟
        /// </summary>
        public int ReadWriteTimeOut { get; set; } = 5 * 60 * 1000;

        /// <summary>
        /// 主机信息
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 保持长连接
        /// </summary>
        public bool KeepLive { get; set; }

        /// <summary>
        /// 接受类型
        /// </summary>
        public string Accept { get; set; } = "text/html, application/xhtml+xml, */*";

        /// <summary>
        /// 请求返回类型默认 text/html
        /// </summary>
        public string ContentType { get; set; } = "text/html";

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.87 Safari/537.36";

        /// <summary>
        /// 来源地址，上次访问地址
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// 获取或设置一个 System.Boolean 值，该值确定是否使用 100-Continue 行为。如果 POST 请求需要 100-Continue 响应，则为 true；否则为 false。默认值为 true。
        /// </summary>
        public bool Expect100Continue { get; set; }

        /// <summary>
        /// 允许自动跳转
        /// </summary>
        public bool AllowRedirect { get; set; }

        /// <summary>
        /// 连接数，默认值1024
        /// </summary>
        public int ConnectionLimit { get; set; } = 1024;

        /// <summary>
        /// cookie
        /// </summary>
        public string Cookie { get; set; }

        /// <summary>
        /// 设置请求将跟随的重定向的最大数目
        /// </summary>
        public int MaximumAutomaticRedirections { get; set; }

        /// <summary>
        /// 发送请求和返回的编码
        /// </summary>
        public Encoding PostEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 表示指定 System.Net.ServicePoint 的本地 Internet 协议地址和端口号的方法(本地的出口ip和端口)
        /// </summary>
        public IPEndPoint IPEndPoint { get; set; }

        /// <summary>
        /// 返回类型
        /// </summary>
        public ResponseType ResponseType { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public WebHeaderCollection Header { get; set; }

        /// <summary>
        ///   获取或设置用于请求的 HTTP 版本。返回结果:用于请求的 HTTP 版本。默认为 System.Net.HttpVersion.Version11。
        /// </summary>
        public Version ProtocolVersion { get; set; }
    }
}
