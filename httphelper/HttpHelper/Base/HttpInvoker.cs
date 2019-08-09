using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace HttpHelper
{
    /// <summary>
    /// http调用类
    /// </summary>
    internal class HttpInvoker
    {
        /// <summary>
        /// HttpWebRequest对象用来发起请求
        /// </summary>
        private HttpWebRequest WebRequest = null;

        /// <summary>
        /// 获取影响流的数据对象
        /// </summary>
        private HttpWebResponse WebResponse = null;

        /// <summary>
        /// 出口ip和端口
        /// </summary>
        private IPEndPoint IPEndPoint = null;

        /// <summary>
        /// 编码方式
        /// </summary>
        private Encoding Encoding = null;

        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>结果</returns>
        public HttpResponse Get(HttpRequest request)
        {
            HttpResponse response = new HttpResponse(); ;
            try
            {
                request.Method = "GET";
                SetRequestData(request);
                using (this.WebResponse = (HttpWebResponse)WebRequest.GetResponse())
                {
                    GetData(request, response);
                }
            }
            catch(Exception e)
            {
                response.StatusDescription = e.Message;
            }

            return response;
        }

        /// <summary>
        /// http post请求
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>结果</returns>
        public HttpResponse Post(HttpRequest request)
        {
            HttpResponse response = new HttpResponse();
            request.Method = "POST";
            try
            {
                SetRequestData(request);
                using (this.WebResponse = (HttpWebResponse)WebRequest.GetResponse())
                {
                    GetData(request, response);
                }
            }
            catch (Exception e)
            {
                response.StatusDescription = e.Message;
            }

            return response;
        }

        /// <summary>
        /// 设置请求参数
        /// </summary>
        private void SetRequestData(HttpRequest request)
        {
            ////初始化
            WebRequest = (HttpWebRequest)System.Net.WebRequest.Create(request.Url);

            ////不包含证书，直接认证通过
            if (string.IsNullOrWhiteSpace(request.CerPath))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                ServicePointManager.DefaultConnectionLimit = request.ConnectionLimit;
            }

            if (request.IPEndPoint != null)
            {
                this.IPEndPoint = request.IPEndPoint;
                WebRequest.ServicePoint.BindIPEndPointDelegate = new BindIPEndPoint(BindIPEndPointCallback);
            }

            if (request.IsUseIpCheat)
            {
                WebRequest.Headers.Add("X-Forwarded-For", GetVirtualIP());
                WebRequest.Headers.Add("Client-Ip", GetVirtualIP());
            }

            if (request.Header != null && request.Header.Count > 0)
            {
                foreach (var item in request.Header.AllKeys)
                {
                    WebRequest.Headers.Add(item, request.Header[item]);
                }
            }

            if (request.ProtocolVersion != null)
            {
                WebRequest.ProtocolVersion = request.ProtocolVersion;
            }

            WebRequest.ServicePoint.Expect100Continue = request.Expect100Continue;
            WebRequest.Method = request.Method;
            WebRequest.Timeout = request.TimeOut;
            WebRequest.KeepAlive = request.KeepLive;
            WebRequest.ReadWriteTimeout = request.ReadWriteTimeOut;
            if (!string.IsNullOrWhiteSpace(request.Host))
            {
                WebRequest.Host = request.Host;
            }

            WebRequest.Accept = request.Accept;
            WebRequest.ContentType = request.ContentType;
            WebRequest.UserAgent = request.UserAgent;
            WebRequest.Credentials = request.Credentials;
            WebRequest.Referer = request.Referer;
            WebRequest.AllowAutoRedirect = request.AllowRedirect;
            if (request.MaximumAutomaticRedirections > 0)
            {
                WebRequest.MaximumAutomaticRedirections = request.MaximumAutomaticRedirections;
            }

            if (request.ConnectionLimit > 0)
            {
                WebRequest.ServicePoint.ConnectionLimit = request.ConnectionLimit;
            }


            SetCert(request);
            SetCerList(request);
            SetProxy(request);
            SetCookie(request);
            SetPostData(request);
        }

        /// <summary>
        /// 获取数据的并解析的方法
        /// </summary>
        /// <param name="item"></param>
        /// <param name="result"></param>
        private void GetData(HttpRequest item, HttpResponse result)
        {
            if (this.WebResponse == null)
            {
                return;
            }
            #region base
            ////获取StatusCode
            result.StatusCode = this.WebResponse.StatusCode;

            ////获取最后访问的URl
            result.ResponseUri = this.WebResponse.ResponseUri;

            ////获取StatusDescription
            result.StatusDescription = this.WebResponse.StatusDescription;

            ////获取Headers
            result.Header = this.WebResponse.Headers;

            ////获取CookieCollection
            if (this.WebResponse.Cookies != null)
            {
                result.CookieCollection = this.WebResponse.Cookies;
            } 

            ////获取set-cookie
            if (this.WebResponse.Headers["set-cookie"] != null)
            {
                result.Cookie = this.WebResponse.Headers["set-cookie"];
            }

            ////全部重新对cookie赋值
            item.Cookie = result.Cookie;
            #endregion

            #region 用户设置用编码
            //处理网页Byte
            byte[] responseByte = this.GetResponseByte();

            if (responseByte != null && responseByte.Length > 0)
            {
                ////设置编码
                //SetEncoding(item, result, responseByte);

                //设置返回的Byte
                SetResult(item, result, responseByte);
            }
            else
            {
                //没有返回任何Html代码
                result.Html = string.Empty;
            }

            #endregion
        }

        #region 发送请求
        /// <summary>
        /// 设置证书
        /// </summary>
        /// <param name="request">请求</param>
        private void SetCert(HttpRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.CerPath))
            {
                if (!string.IsNullOrWhiteSpace(request.CerPwd))
                {
                    WebRequest.ClientCertificates.Add(new X509Certificate(request.CerPath, request.CerPwd));
                }
                else
                {
                    WebRequest.ClientCertificates.Add(new X509Certificate(request.CerPath));
                }
            }
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="item">Http参数</param>
        private void SetCookie(HttpRequest item)
        {
            if (!string.IsNullOrEmpty(item.Cookie))
            {
                WebRequest.Headers[HttpRequestHeader.Cookie] = item.Cookie;
            }

            //设置CookieCollection
            ////if (item.ResultCookieType == ResultCookieType.CookieCollection)
            ////{
            ////    request.CookieContainer = new CookieContainer();
            ////    if (item.CookieCollection != null && item.CookieCollection.Count > 0)
            ////        request.CookieContainer.Add(item.CookieCollection);
            ////}
            ////else if (item.ResultCookieType == ResultCookieType.CookieContainer)
            ////{
            ////    request.CookieContainer = item.CookieContainer;
            ////}
        }

        /// <summary>
        /// 设置多个证书
        /// </summary>
        /// <param name="item"></param>
        private void SetCerList(HttpRequest request)
        {
            if (request.ClentCertificates != null && request.ClentCertificates.Count > 0)
            {
                foreach (X509Certificate c in request.ClentCertificates)
                {
                    WebRequest.ClientCertificates.Add(c);
                }
            }
        }

        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="item">参数对象</param>
        private void SetProxy(HttpRequest item)
        {
            if (!string.IsNullOrWhiteSpace(item.ProxyIp))
            {
                //设置代理服务器
                if (item.ProxyIp.Contains(":"))
                {
                    string[] plist = item.ProxyIp.Split(':');
                    WebProxy myProxy = new WebProxy(plist[0].Trim(), Convert.ToInt32(plist[1].Trim()));
                    //建议连接
                    myProxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);
                    //给当前请求对象
                    WebRequest.Proxy = myProxy;
                }
                else
                {
                    WebProxy myProxy = new WebProxy(item.ProxyIp, false);
                    //建议连接
                    myProxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);
                    //给当前请求对象
                    WebRequest.Proxy = myProxy;
                }
            }
            else
            {
                WebRequest.Proxy = item.WebProxy;
            }
        }

        /// <summary>
        /// 设置请求数据
        /// </summary>
        /// <param name="item"></param>
        private void SetPostData(HttpRequest request)
        {
            //验证在得到结果时是否有传入数据
            if (!request.Method.Trim().ToLower().Contains("get"))
            {
                byte[] buffer = null;
                //写入Byte类型
                if (request.PostDataType == PostDataType.Byte && request.PostDataByte != null && request.PostDataByte.Length > 0)
                {
                    //验证在得到结果时是否有传入数据
                    buffer = request.PostDataByte;
                }//写入文件
                else if (request.PostDataType == PostDataType.FilePath && !string.IsNullOrWhiteSpace(request.PostData))
                {
                    StreamReader r = new StreamReader(request.PostData, request.PostEncoding);
                    buffer = request.PostEncoding.GetBytes(r.ReadToEnd());
                    r.Close();
                }
                else if (!string.IsNullOrWhiteSpace(request.PostData))
                {
                    //写入字符串
                    buffer = request.PostEncoding.GetBytes(request.PostData);
                }

                if (buffer != null)
                {
                    this.WebRequest.ContentLength = buffer.Length;
                    this.WebRequest.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
                else
                {
                    this.WebRequest.ContentLength = 0;
                }
            }
        }

        /// <summary>
        /// 通过设置这个属性，可以在发出连接的时候绑定客户端发出连接所使用的IP地址。 
        /// </summary>
        /// <param name="servicePoint">servicePoint</param>
        /// <param name="remoteEndPoint">remoteEndPoint</param>
        /// <param name="retryCount">retryCount</param>
        /// <returns>结果</returns>
        public IPEndPoint BindIPEndPointCallback(ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount)
        {
            return this.IPEndPoint;
        }

        /// <summary>
        /// 总是通过
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="certificate">certificate</param>
        /// <param name="chain">chain"</param>
        /// <param name="errors">errors</param>
        /// <returns>true</returns>
        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }

        /// <summary>
        /// 构建假ip
        /// </summary>
        /// <returns>结果</returns>
        private string GetVirtualIP()
        {
            string text = RandomHelper.GetRandom(0, 255).ToString();
            string text2 = RandomHelper.GetRandom(0, 255).ToString();
            string text3 = RandomHelper.GetRandom(0, 255).ToString();
            string text4 = RandomHelper.GetRandom(0, 255).ToString();
            return string.Format("{0}.{1}.{2}.{3}", text, text2, text3, text4);
        }
        #endregion

        #region 处理返回
        private byte[] GetResponseByte()
        {
            string encoding = string.IsNullOrWhiteSpace(this.WebResponse.CharacterSet) ? "utf-8" : this.WebResponse.CharacterSet;
            using (MemoryStream resStream = new MemoryStream())
            {
                if (this.WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                {
                    using (GZipStream stream = new GZipStream(this.WebResponse.GetResponseStream(), CompressionMode.Decompress))
                    {
                        stream.CopyToAsync(resStream, 10240);
                    }
                }
                else if (this.WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                {
                    using (DeflateStream stream = new DeflateStream(this.WebResponse.GetResponseStream(), CompressionMode.Decompress))
                    {
                        stream.CopyToAsync(resStream, 10240);
                    }
                }
                else
                {
                    using (Stream stream = this.WebResponse.GetResponseStream())
                    {
                        stream.CopyToAsync(resStream, 10240);
                    }
                }

                return resStream.ToArray();
            }
        }

        /// <summary>
        /// 设置结果
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="response">结果</param>
        private void SetResult(HttpRequest request, HttpResponse response, byte[] enByte)
        {

            //是否返回Byte类型数据
            if (request.ResponseType == ResponseType.Byte)
            {
                //Byte数据
                response.Byte = enByte;
            }
            else if (request.ResponseType == ResponseType.Html)
            {
                //得到返回的HTML
                response.Html = request.PostEncoding.GetString(enByte);
            }
            else if (request.ResponseType == ResponseType.HtmlByte)
            {
                //Byte数据
                response.Byte = enByte;
                //得到返回的HTML
                response.Html = request.PostEncoding.GetString(enByte);
            }
        }

        /// <summary>
        /// 设置编码
        /// </summary>
        /// <param name="item">HttpItem</param>
        /// <param name="result">HttpResult</param>
        /// <param name="ResponseByte">byte[]</param>
        private void SetEncoding(HttpRequest request, HttpResponse response, byte[] enByte)
        {
            if (request.PostEncoding == null)
            {
                Match meta = Regex.Match(Encoding.Default.GetString(enByte), "<meta[^<]*charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
                string c = string.Empty;
                if (meta != null && meta.Groups.Count > 0)
                {
                    c = meta.Groups[1].Value.ToLower().Trim();
                }
                string cs = string.Empty;
                if (!string.IsNullOrWhiteSpace(this.WebResponse.CharacterSet))
                {
                    cs = this.WebResponse.CharacterSet.Trim().Replace("\"", "").Replace("\'", "");
                }

                if (c.Length > 2)
                {
                    try
                    {
                        this.Encoding = Encoding.GetEncoding(c.Replace("\"", string.Empty).Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk").Trim());
                    }
                    catch
                    {
                        if (string.IsNullOrEmpty(cs))
                        {
                            this.Encoding = Encoding.UTF8;
                        }
                        else
                        {
                            this.Encoding = Encoding.GetEncoding(cs);
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(cs))
                    {
                        this.Encoding = Encoding.UTF8;
                    }
                    else
                    {
                        this.Encoding = Encoding.GetEncoding(cs);
                    }
                }
            }
        }
        #endregion
    }
}
