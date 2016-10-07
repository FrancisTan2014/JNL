using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JNL.Bll;
using JNL.Model;
using JNL.Utilities.Extensions;
using JNL.Web.Models;

namespace JNL.Web.Controllers
{
    /// <summary>
    /// 预警信息
    /// </summary>
    public class WarnController : Controller
    {
        public ActionResult AddWarn()
        {
            var id = RouteData.Values["id"] ?? 0;
            var warning = new WarningBll().QuerySingle(id);

            if (warning == null)
            {
                ViewBag.Title = "添加监督检查预警";
                warning = new Warning { CreateStaffId = LoginStatus.GetLoginUser().Id };
            }
            else
            {
                ViewBag.Title = "修改监督检查预警";
            }
            
            ViewBag.Warning = warning;

            return View();
        }

        public ActionResult NotImplement()
        {
            return View();
        }
    }
}
