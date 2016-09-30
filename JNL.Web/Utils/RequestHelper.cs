using System.Web;

namespace JNL.Web.Utils
{
    /// <summary>
    /// 请求工具类，提供对请求中的数据进行过滤或者其他公共操作的方法
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-07-15</since>
    public static class RequestHelper
    {
        /// <summary>
        /// 根据请求中的约定参数（async=true），判断当前请求是否是异步请求
        /// </summary>
        /// <returns>是异步请求返回<c>true</c>，否则返回<c>false</c></returns>
        public static bool IsAsyncRequest()
        {
            var async = HttpContext.Current.Request["async"];

            return async == "true";
        }
    }
}