using System;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using JNL.Utilities.Extensions;
using JNL.Web.Models;

namespace JNL.Web.Filters
{
    /// <summary>
    /// 本站页面身份验证类，提供对指定控制器或者方法执行登录验证的方法
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-07-15</since>
    public class Authority
    {
        /// <summary>
        /// 映射配置文件filter.config的根节点
        /// </summary>
        private static XElement LoginFilterRoot { get; }

        /// <summary>
        /// 
        /// </summary>
        private static XElement AuthFilterRoot { get; }

        /// <summary>
        /// 在本类第一次被使用时加载过滤器配置文件到内存中
        /// </summary>
        static Authority()
        {
            try
            {
                var loginConfigPath = HttpContext.Current.Server.MapPath("/login-filter.config");
                LoginFilterRoot = XElement.Load(loginConfigPath);
            }
            catch
            {
                throw new Exception("加载login-filter.config出现异常，请检查此配置文件");
            }

            try
            {
                var authConfigPath = HttpContext.Current.Server.MapPath("/auth-filter.config");
                AuthFilterRoot = XElement.Load(authConfigPath);
            }
            catch (Exception)
            {
                throw new Exception("加载auth-filter.config出现异常，请检查此配置文件");
            }
        }

        /// <summary>
        /// 判断执行指定控制器下的指定方法是否需要登录
        /// </summary>
        /// <param name="controller">指定控制器名称</param>
        /// <param name="action">指定方法名称</param>
        /// <returns>表示是否需要登录的布尔值</returns>
        public static bool WhetherNeedLogin(string controller, string action)
        {
            var controllerElements = LoginFilterRoot.Elements().ToList();
            if (!controllerElements.Any())
            {
                return true;
            }

            var controllerElem = controllerElements.FirstOrDefault(e => e.Attribute("name").Value == controller);
            if (controllerElem == null)
            {
                return true;
            }

            // controller节点中hole属性为true，表示此控制器下所有方法均不需要做登录验证
            var holeAttr = controllerElem.Attribute("hole");
            if (holeAttr != null && holeAttr.Value == "true")
            {
                return false;
            }

            var actionElements = controllerElem.Elements().ToList();
            if (!actionElements.Any())
            {
                return true;
            }

            var actionElem = actionElements.FirstOrDefault(e => e.Value == action);
            if (actionElem == null)
            {
                return true;
            }

            // 当前请求的方法在配置文件中存在，则表示不需要做登录验证
            return false;
        }

        /// <summary>
        /// 判断当前登录用户是否具有访问指定action的权限
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool CanVisitThisAction(string controller, string action)
        {
            var controllerElements = AuthFilterRoot.Elements().ToList();
            if (!controllerElements.Any())
            {
                // 没有出现在配置文件中的控制器方法默认都可以访问
                return true;
            }

            var controllerElem = controllerElements.SingleOrDefault(e => e.Attribute("name").Value == controller);

            var actionElem = controllerElem?.Elements().SingleOrDefault(e=>e.Attribute("name").Value == action);
            if (actionElem == null)
            {
                return true;
            }

            var authType = actionElem.Attribute("authtype").Value.ToInt32(1);
            var authIdString = actionElem.Value;
            if (string.IsNullOrEmpty(authIdString))
            {
                return true;
            }
            
            var currentUser = LoginStatus.GetLoginUser();
            if (currentUser == null)
            {
                return false;
            }

            var authIdList = authIdString.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries).Select(s=>s.ToInt32());
            if (authType == 1)
            {
                return authIdList.Contains(currentUser.DepartmentId);
            }
            else
            {
                return authIdList.Contains(currentUser.Id);
            }
        }
    }
}