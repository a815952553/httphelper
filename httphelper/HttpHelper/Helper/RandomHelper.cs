using System;
using System.Security.Cryptography;

namespace HttpHelper
{
    /// <summary>
    /// 生成随机数帮助类
    /// </summary>
    public class RandomHelper
    {
        /// <summary>
        /// 随机数
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns>结果</returns>
        public static int GetRandom(int minValue, int maxValue)
        {
            int result;
            try
            {
                Random random = new Random(RandomSeed());
                result = random.Next(minValue, maxValue);
            }
            catch
            {
                result = minValue;
            }

            return result;
        }

        /// <summary>
        /// 生成强随机数
        /// </summary>
        /// <returns>结果</returns>
        public static int RandomSeed()
        {
            byte[] array = new byte[4];
            RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            rNGCryptoServiceProvider.GetBytes(array);
            return BitConverter.ToInt32(array, 0);
        }
    }
}
