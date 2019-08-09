using System;
using System.Security.Cryptography;

namespace HttpHelper.Helper
{
    /// <summary>
    /// 生成随机数帮助类
    /// </summary>
    public class RandomHelper
    {
        /// <summary>
        /// 生成强随机数
        /// </summary>
        /// <returns>结果</returns>
        public static int Random()
        {
            byte[] array = new byte[4];
            RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            rNGCryptoServiceProvider.GetBytes(array);
            return BitConverter.ToInt32(array, 0);
        }
    }
}
