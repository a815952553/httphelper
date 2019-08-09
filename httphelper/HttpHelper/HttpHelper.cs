using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HttpHelper
{
    /// <summary>
    /// http请求帮助类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 初始化invoker
        /// </summary>
        private HttpInvoker Invoker = new HttpInvoker();

        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>结果</returns>
        public HttpResponse Get(HttpRequest request)
        {
            return Invoker.Get(request);
        }

        /// <summary>
        /// http post请求
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>结果</returns>
        public HttpResponse Post(HttpRequest request)
        {
            return Invoker.Post(request);
        }

        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>结果</returns>
        public async Task<HttpResponse> GetAsync(HttpRequest request)
        {
            return Invoker.Get(request);
        }

        /// <summary>
        /// http post请求
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>结果</returns>
        public async Task<HttpResponse> PostAsync(HttpRequest request)
        {
            return Invoker.Post(request);
        }
    }
}
