using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JNL.Bll;
using JNL.Utilities.Extensions;
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

        public ActionResult List()
        {
            var category = RouteData.Values["id"].ToString().ToInt32();

            if (category < 2 || category > 4)
            {
                return Redirect("/Error/NotFound");
            }

            var titleDic = new Dictionary<int, string>
            {
                { 2, "非正常情况应急处理" },
                { 3, "应急管理" },
                { 4,"应急预案" }
            };

            ViewBag.Title = titleDic[category];
            ViewBag.CateTory = category;

            return View();
        }

        public ActionResult Scan()
        {
            var id = RouteData.Values["id"].ToString().ToInt32();

            var articleBll = new ViewArticleBll();
            var article = articleBll.QuerySingle(id);
            if (article == null)
            {
                return Redirect("/Error/NotFound");
            }

            ViewBag.Model = article;

            return View();
        }
    }
}
