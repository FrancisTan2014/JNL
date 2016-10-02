using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JNL.Bll;
using JNL.Utilities.Extensions;

namespace JNL.Web.Controllers
{
    public class RiskController : Controller
    {
        public ActionResult AddRisk()
        {
            var type = RouteData.Values["id"].ToString().ToInt32();
            var riskType = new RiskTypeBll().QuerySingle(type);

            if (riskType == null)
            {
                return Redirect("/Error/NotFound");
            }

            ViewBag.Title = riskType.Name;
            ViewBag.RiskType = riskType.Id;

            return View();
        }
    }
}
