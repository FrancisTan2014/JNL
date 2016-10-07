using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JNL.Web.Models;

namespace JNL.Web.Controllers
{
    public class ArticleController : Controller
    {
        public ActionResult Add()
        {
            ViewBag.Staff = LoginStatus.GetLoginUser().Id;

            return View();
        }
    }
}
