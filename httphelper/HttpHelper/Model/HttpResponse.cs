using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HttpHelper
{
    /// <summary>
    /// 爬虫请求的返回值
    /// </summary>
    public class HttpResponse
    {
        /// <summary>
        /// cookie字符串
        /// </summary>
        public string Cookie { get; set; }

        /// <summary>
        /// 返回的String类型数据 只有ResultType.String时才返回数据，其它情况为空
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// 返回的Byte数组 只有ResultType.Byte 时才返回数据，其它情况为空
        /// </summary>
        public byte[] Byte { get; set; }

        /// <summary>
        /// 返回状态说明
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>
        /// 返回状态码,默认为OK
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// 返回uri
        /// </summary>
        public Uri ResponseUri { get; set; }

        /// <summary>
        /// header对象
        /// </summary>
        public WebHeaderCollection Header { get; set; }

        /// <summary>
        /// Cookie对象集合，个别网站解析string类型cookie有误时备用
        /// </summary>
        public CookieCollection CookieCollection { get; set; }
    }
}
