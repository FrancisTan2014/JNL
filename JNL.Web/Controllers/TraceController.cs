using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JNL.Bll;
using JNL.Model;
using JNL.Utilities.Extensions;

namespace JNL.Web.Controllers
{
    public class TraceController : Controller
    {
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Edit()
        {
            var id = RouteData.Values["id"];
            var traceId = id?.ToString().ToInt32() ?? 0;

            ViewBag.Trace = new TraceInfoBll().QuerySingle(traceId) ?? new TraceInfo();

            return View();
        }

        public ActionResult Query()
        {
            return View();
        }
    }
}
