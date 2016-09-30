using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JNL.Utilities.Extensions;

namespace JNL.Web.Controllers
{
    public class BasicController : Controller
    {
        //
        // GET: /Basic/

        public ActionResult Files()
        {
            var fileType = RouteData.Values["fileType"].ToString().ToInt32();
            var level = RouteData.Values["level"].ToString().ToInt32();

            if (fileType < 1 || fileType > 3 || level < 1 || level > 3)
            {
                return Redirect("/Error/NotFound");
            }

            var fileTypes = new [] { "技术规章", "企业标准", "制度措施" };
            var levels = new[] {"总公司", "铁路局", "机务段"};

            var title = $"{fileTypes[fileType - 1]} - {levels[level-1]}";
            ViewBag.Title = title;
            ViewBag.FileType = fileType;
            ViewBag.Level = level;

            return View();
        }

        public ActionResult AddFile()
        {
            return View();
        }

        public ActionResult Accidents()
        {
            return View();
        }
    }
}
