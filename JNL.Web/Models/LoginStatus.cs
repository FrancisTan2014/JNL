using System.Web;
using JNL.Bll;
using JNL.Model;
using JNL.Utilities.Extensions;
using JNL.Utilities.Helpers;
using JNL.Web.Utils;

namespace JNL.Web.Models
{
    /// <summary>
    /// 提供对管理员登录状态进行管理的方法
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-07-14</since>
    public class LoginStatus
    {
        /// <summary>
        /// 将用户登录状态记录到cookie中（有效期7天）
        /// </summary>
        public static void WriteCookieFor7Days(int userId)
        {
            var encryptId = userId.ToString();
            var expire = 7*24*60*60; // 7天

            CookieHelper.Set(CookieNames.LoginCookie, encryptId, expire);
        }

        /// <summary>
        /// 将用户登录状态记录到cookie中（当前会话有效）
        /// </summary>
        public static void WriteCookieForSession(int userId)
        {
            var encryptId = userId.ToString();

            CookieHelper.Set(CookieNames.LoginCookie, encryptId);
        }

        /// <summary>
        /// 将用户登录状态记录从cookie中移除
        /// </summary>
        public static void RemoveCookie()
        {
            CookieHelper.Remove(CookieNames.LoginCookie);
        }

        /// <summary>
        /// 从cookie中获取当前登录用户id
        /// </summary>
        /// <returns>当前登录管理员id</returns>
        public static int GetLoginId()
        {
            var encryptId = CookieHelper.Get(CookieNames.LoginCookie);

            return encryptId.ToInt32();
        }

        /// <summary>
        /// 获取当前登录的用户实体信息
        /// </summary>
        /// <returns></returns>
        public static Staff GetLoginUser()
        {
            var userId = GetLoginId();
            var staff = new StaffBll().QuerySingle(userId);

            if (staff == null)
            {
                // 登录过期，需重新登录
                var requestUrl = HttpContext.Current.Request.RawUrl;
                HttpContext.Current.Response.Redirect($"/Home/Login?backUrl={requestUrl}");
            }

            return staff;
        }

        /// <summary>
        /// 判断当前是否已经登录
        /// </summary>
        /// <returns>返回表示是否已登录的布尔值</returns>
        public static bool IsLogin()
        {
            var userId = GetLoginId();
            if (userId > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 将当前请求终止，并重定向到登录页面
        /// </summary>
        public static void RedirectToLogin()
        {
            var context = HttpContext.Current;
            var requestUrl = context.Request.Url.ToString();

            if (RequestHelper.IsAsyncRequest())
            {
                var msg = ErrorModel.NeedLoginFirst(requestUrl);
                var json = JsonHelper.Serialize(msg);

                context.Response.Write(json);
                context.Response.End();
            }
            else
            {
                var redirectUrl = $"/Home/Login?backUrl={requestUrl}";
                context.Response.Redirect(redirectUrl);
            }
        }
    }
}