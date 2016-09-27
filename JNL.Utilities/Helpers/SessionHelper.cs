using System.Web;

namespace JNL.Utilities.Helpers
{
    /// <summary>
    /// Session工具类，提供对Session的便捷操作
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-07-14</since>
    public static class SessionHelper
    {
        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="name">session名称</param>
        /// <param name="value">session所保存的对象</param>
        public static void Set(string name, object value)
        {
            var session = HttpContext.Current.Session;
            if (session[name] != null)
            {
                session.Remove(name);
            }

            session.Add(name, value);
        }

        /// <summary>
        /// 获取指定session所保存的对象
        /// </summary>
        /// <param name="name">session名称</param>
        /// <returns>返回session所保存的对象或者null</returns>
        public static object Get(string name)
        {
            return HttpContext.Current.Session[name];
        }

        /// <summary>
        /// 获取指定session所保存的指定类型的对象
        /// </summary>
        /// <param name="name">session名称</param>
        /// <returns>返回指定类型的对象或者null</returns>
        public static T Get<T>(string name) where T: class
        {
            var value = Get(name);

            T res = null;
            if (value != null)
            {
                res = (T) value;
            }

            return res;
        }

        /// <summary>
        /// 移除指定Session
        /// </summary>
        /// <param name="name">待移除的Session名称</param>
        public static void Remove(string name)
        {
            HttpContext.Current.Session.Remove(name);
        }
    }
}
