using System;
using System.Web;

namespace JNL.Utilities.Helpers
{
    /// <summary>
    /// Cookie工具类，提供便捷操作cookie的方法
    /// </summary>
    /// 
    public static class CookieHelper
    {
        /// <summary>
        /// 设置cookie的值、过期时间以及作用域
        /// </summary>
        /// <param name="name">cookie名称</param>
        /// <param name="value">cookie的值</param>
        /// <param name="expireSeconds">cookie的过期时间（单位：秒）</param>
        /// <param name="path">cookie生效的路径（页面）</param>
        /// <param name="domain">cookie的作用域（域名）</param>
        public static void Set(string name, string value, int expireSeconds, string path, string domain)
        {
            var cookie = new HttpCookie(name) { Value = value };

            if (!string.IsNullOrEmpty(domain))
            {
                cookie.Domain = domain;
            }

            if (expireSeconds > 0)
            {
                cookie.Expires = DateTime.Now.AddSeconds(expireSeconds);
            }

            if (!string.IsNullOrEmpty(path))
            {
                cookie.Path = path;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 设置cookie的值、过期时间
        /// </summary>
        /// <param name="name">cookie名称</param>
        /// <param name="value">cookie的值</param>
        /// <param name="expireSeconds">cookie的过期时间（单位：秒）</param>
        public static void Set(string name, string value, int expireSeconds)
        {
            Set(name, value, expireSeconds, null, null);
        }

        /// <summary>
        /// 设置cookie的值
        /// </summary>
        /// <param name="name">cookie名称</param>
        /// <param name="value">cookie的值</param>
        public static void Set(string name, string value)
        {
            Set(name, value, 0, null, null);
        }

        /// <summary>
        /// 获取指定cookie的值
        /// </summary>
        /// <param name="name">cookie名称</param>
        /// <returns>指定cookie的值或者string.Empty</returns>
        public static string Get(string name)
        {
            var cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
            {
                return string.Empty;
            }

            return cookie.Value;
        }

        /// <summary>
        /// 移除指定cookie
        /// </summary>
        /// <param name="name">待移除cookie名称</param>
        public static void Remove(string name)
        {
            Set(name, null, -24*60);
        }
    }
}
