namespace JNL.Web.Utils
{
    /// <summary>
    /// 对cookie名称进行统一管理
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-07-14</since>
    public sealed class CookieNames
    {
        /// <summary>
        /// 记录当前用户登录状态的cookie名称
        /// </summary>
        public static readonly string LoginCookie = "iongl";

        /// <summary>
        /// 记录当前登录管理员的id
        /// </summary>
        public static readonly string LoginUserId = "ndgi";
    }
}
