using HttpHelper.Base;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace HttpHelper
{
    /// <summary>
    /// 请求参数
    /// </summary>
    public class HttpRequest : BaseHttpRequest
    {
        #region 代理相关
        /// <summary>
        /// 设置代理对象，不想使用IE默认配置就设置为Null，而且不要设置ProxyIp
        /// </summary>
        public WebProxy WebProxy { get; set; }

        /// <summary>
        /// 代理Proxy 服务器用户名
        /// </summary>
        public string ProxyUserName { get; set; }

        /// <summary>
        /// 代理 服务器密码
        /// </summary>
        public string ProxyPwd { get; set; }

        /// <summary>
        /// 代理 服务IP,如果要使用IE代理就设置为ieproxy
        /// </summary>
        public string ProxyIp { get; set; }
        #endregion

        #region 请求数据
        /// <summary>
        /// Post请求时要发送的字符串Post数据
        /// </summary>
        public string PostData { get; set; }

        /// <summary>
        /// Post请求时要发送的Byte类型的Post数据
        /// </summary>
        public byte[] PostDataByte { get; set; }
        #endregion

        #region 身份/证书相关
        /// <summary>
        /// 证书绝对路径
        /// </summary>
        public string CerPath { get; set; }

        /// <summary>
        /// 证书密码
        /// </summary>
        public string CerPwd { get; set; }

        /// <summary>
        /// X509证书
        /// </summary>
        public X509CertificateCollection ClentCertificates { get; set; }

        /// <summary>
        /// 获取或设置请求的身份验证信息
        /// </summary>
        public ICredentials Credentials { get; set; }
        #endregion
    }
}
