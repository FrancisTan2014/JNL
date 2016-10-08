using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JNL.Bll;
using JNL.Model;
using JNL.Utilities.Extensions;
using JNL.Utilities.Helpers;
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
                var user = LoginStatus.GetLoginUser();
                warning = new Warning { WarningStaffId = user.Id, CreateStaffId = user.Id };
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

        public ActionResult PreImplement()
        {
            return View();
        }

        public ActionResult AddImplement()
        {
            var id = RouteData.Values["id"].ToString().ToInt32();

            var warnBll = new ViewWarningBll();
            var warn = warnBll.QuerySingle(id);
            if (warn == null)
            {
                return Redirect("/Error/NotFound");
            }

            var loginUser = LoginStatus.GetLoginUser();
            var implementDeparts = warn.ImplementDeparts.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (!implementDeparts.Contains(loginUser.DepartmentId.ToString()))
            {
                return Redirect("/Error/NoAuth");
            }

            var implement = new ViewWarningImplementBll().QuerySingle($"ImplementDepartmentId={loginUser.DepartmentId} AND WarningId={id}") ?? new ViewWarningImplement();

            ViewBag.Warn = warn;
            ViewBag.Implement = implement;
            ViewBag.DepartId = loginUser.DepartmentId;

            return View();
        }

        [HttpPost]
        public JsonResult UpdateImplement(string json)
        {
            var implement = JsonHelper.Deserialize<WarningImplement>(json);
            if (implement == null)
            {
                return Json(ErrorModel.InputError);
            }

            var warn = new Warning
            {
                Id = implement.WarningId,
                HasBeganImplement = true
            };

            var implementBll = new WarningImplementBll();
            var warnBll = new WarningBll();

            var success = implementBll.ExecuteTransation(() =>
            {
                bool res1;
                if (implement.Id > 0)
                {
                    res1 = implementBll.Update(implement);
                }
                else
                {
                    res1 = implementBll.Insert(implement).Id > 0;
                }

                var res2 = warnBll.Update(warn, new[] { "HasBeganImplement" });

                return res1 && res2;
            });

            if (success)
            {
                return Json(ErrorModel.OperateSuccess);
            }

            return Json(ErrorModel.OperateFailed);
        }

        public ActionResult VerifyImplement()
        {
            return View();
        }

        public ActionResult Verify()
        {
            var id = RouteData.Values["id"].ToString().ToInt32();

            var warnBll = new ViewWarningBll();
            var warn = warnBll.QuerySingle(id);
            if (warn == null)
            {
                return Redirect("/Error/NotFound");
            }

            var implements = new ViewWarningImplementBll().QueryList($"WarningId={id}").ToList();

            ViewBag.Warn = warn;
            ViewBag.Implements = implements;
            ViewBag.Staff = LoginStatus.GetLoginUser().Id;

            return View();
        }

        [HttpPost]
        public JsonResult DoVerify(int id, int status)
        {
            var implementBll = new WarningImplementBll();
            var implement = implementBll.QuerySingle(id);
            if (implement == null)
            {
                return Json(ErrorModel.InputError);
            }

            implement.ResponseVerifyStatus = status;
            implement.VerifyStaffId = LoginStatus.GetLoginId();
            implement.VerifyTime = DateTime.Now;

            var warnBll = new WarningBll();
            var warn = warnBll.QuerySingle(implement.WarningId);

            var success = implementBll.ExecuteTransation(() =>
            {
                var res1 = implementBll.Update(implement, new[] {"ResponseVerifyStatus", "VerifyStaffId", "VerifyTime"});
                if (res1)
                {
                    var implementedCount =
                        implementBll.QueryList($"WarningId={implement.WarningId} AND ResponseVerifyStatus=2", new[] {"Id"})
                            .Count();
                    var departCount = warn.ImplementDeparts.Split(',').Length;
                    if (implementedCount == departCount)
                    {
                        // 将预警信息标记为全部审核通过
                        warn.HasImplementedAll = true;
                        return warnBll.Update(warn, new[] {"HasImplementedAll"});
                    }
                }

                return res1;
            });

            if (success)
            {
                return Json(ErrorModel.OperateSuccess);
            }

            return Json(ErrorModel.OperateFailed);
        }

        public ActionResult Finished()
        {
            return View();
        }
    }
}
