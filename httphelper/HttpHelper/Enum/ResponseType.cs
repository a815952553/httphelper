using System;
using System.Collections.Generic;
using System.Text;

namespace HttpHelper
{
    /// <summary>
    /// 返回类型
    /// </summary>
    public enum ResponseType
    {
        /// <summary>
        /// 返回html页面
        /// </summary>
        Html,

        /// <summary>
        /// 返回byte数组    
        /// </summary>
        Byte,

        /// <summary>
        /// html和byte数组同时返回
        /// </summary>
        HtmlByte
    }
}
