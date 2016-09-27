using System;
using System.Security.Cryptography;
using System.Text;

namespace JNL.Utilities.Extensions
{
    /// <summary>
    /// 对string类的常用扩展
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-07-13</since>
    public static class StringExtension
    {
        /// <summary>
        /// 将字符串转换为int类型，转换失败则返回指定默认值
        /// </summary>
        /// <param name="value">待转换的字符串</param>
        /// <param name="def">转换失败时返回的默认值</param>
        /// <returns>返回指定字符串对应的int型值或者指定默认值</returns>
        public static int ToInt32(this string value, int def = default(int))
        {
            int res;
            if (int.TryParse(value, out res))
            {
                return res;
            }

            return def;
        }

        /// <summary>
        /// 将字符串转换为DateTime类型，转换失败则返回指定默认值
        /// </summary>
        /// <param name="value">待转换的字符串</param>
        /// <param name="def">指定默认值</param>
        /// <returns>返回指定字符串对应的DateTime型值或者指定默认值</returns>
        public static DateTime ToDateTime(this string value, DateTime def = default(DateTime))
        {
            DateTime res;
            if (DateTime.TryParse(value, out res))
            {
                return res;
            }

            return def;
        }

        /// <summary>
        /// 获取本字符串的MD5值
        /// </summary>
        public static string GetMd5(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            var md5 = new MD5CryptoServiceProvider();
            var bytes = Encoding.Default.GetBytes(value);
            var hashBytes = md5.ComputeHash(bytes);

            var stringBuilder = new StringBuilder();
            foreach (var hashByte in hashBytes)
            {
                stringBuilder.Append(hashByte.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
