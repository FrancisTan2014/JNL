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
using JNL.Web.Utils;
using WebGrease.Css.Extensions;

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
            ViewBag.RiskLevels = AppSettings.NeedReformRiskLevels;

            return View();
        }

        [HttpPost]
        public JsonResult AddRisk(string risk, string responds)
        {
            if (string.IsNullOrEmpty(risk) || string.IsNullOrEmpty(responds))
            {
                return Json(ErrorModel.InputError);
            }

            var riskInfo = JsonHelper.Deserialize<RiskInfo>(risk);
            var riskResponds =
                responds.Split(new[] { "###" }, StringSplitOptions.RemoveEmptyEntries).Select(s => new RiskResponseStaff
                {
                    ResponseStaffId = s.ToInt32()
                }).ToList();

            var riskBll = new RiskInfoBll();
            var respondBll = new RiskResponseStaffBll();

            var success = riskBll.ExecuteTransation(() =>
            {
                riskInfo = riskBll.Insert(riskInfo);
                if (riskInfo.Id > 0)
                {
                    riskResponds.ForEach(r => r.RiskId = riskInfo.Id);
                    respondBll.BulkInsert(riskResponds);

                    return true;
                }

                return false;
            });

            if (success)
            {
                return Json(ErrorModel.OperateSuccess);
            }

            return Json(ErrorModel.OperateFailed);
        }

        public ActionResult UpdateRisk()
        {
            var id = RouteData.Values["id"].ToString().ToInt32();

            var riskBll = new ViewRiskInfoBll();
            var respondBll = new ViewRiskResponseStaffBll();

            var risk = riskBll.QuerySingle(id);
            var respondStaff = respondBll.QuerySingle($"RiskId={id}");

            if (risk == null || respondStaff == null)
            {
                return Redirect("/Error/NotFound");
            }
            
            // 验证当前登录用户是否具有修改此风险信息的权限
            if (CurrentLoginStaffHasNoAuth(risk))
            {
                return Redirect("/Error/NoAuth");
            }

            ViewBag.Title = risk.VerifyStatus == (int)RiskVerifyStatus.WaitingForReportDepartVerify ||
                            risk.VerifyStatus == (int)RiskVerifyStatus.IsVotedAndWaitingForVerify
                ? "本部门风险信息审核"
                : "风险办风险信息审核";
            ViewBag.Risk = risk;
            ViewBag.RespondStaff = respondStaff;
            ViewBag.RiskLevels = AppSettings.NeedReformRiskLevels;

            return View();
        }

        [HttpPost]
        public JsonResult UpdateRisk(string risk, string responds)
        {
            if (string.IsNullOrEmpty(risk) || string.IsNullOrEmpty(responds))
            {
                return Json(ErrorModel.InputError);
            }

            var riskInfo = JsonHelper.Deserialize<RiskInfo>(risk);
            if (riskInfo == null)
            {
                return Json(ErrorModel.InputError);
            }

            var riskBll = new RiskInfoBll();
            if (!riskBll.Exists("Id=" + riskInfo.Id))
            {
                return Json(ErrorModel.InputError);
            }
            
            var riskResponds =
                responds.Split(new[] { "###" }, StringSplitOptions.RemoveEmptyEntries).Select(s => new RiskResponseStaff
                {
                    RiskId = riskInfo.Id,
                    ResponseStaffId = s.ToInt32()
                }).ToList();

            var updateRes = UpdateRisk(riskInfo, riskResponds);

            return Json(updateRes);
        }

        private object UpdateRisk(RiskInfo risk, List<RiskResponseStaff> responds)
        {
            var riskBll = new RiskInfoBll();
            var respondBll = new RiskResponseStaffBll();

            var success = riskBll.ExecuteTransation(() =>
            {
                var res1 = riskBll.Update(risk);
                var res2 = respondBll.Delete($"RiskId={risk.Id}");
                if (responds.Any())
                {
                    respondBll.BulkInsert(responds);
                }

                return res1 && res2;
            });

            if (success)
            {
                return ErrorModel.OperateSuccess;
            }

            return ErrorModel.OperateFailed;
        }

        private bool CurrentLoginStaffHasNoAuth(ViewRiskInfo risk)
        {
            var loginStaff = LoginStatus.GetLoginUser();

            return risk.VerifyStatus == (int)RiskVerifyStatus.VerifyHasPassed ||
                   (risk.VerifyStatus == (int)RiskVerifyStatus.WaitingForReportDepartVerify &&
                    loginStaff.DepartmentId != risk.ReportStaffDepartId) ||
                   (risk.VerifyStatus == (int)RiskVerifyStatus.IsVotedAndWaitingForVerify &&
                    loginStaff.DepartmentId != risk.ReportStaffDepartId) ||
                   (risk.VerifyStatus == (int)RiskVerifyStatus.WaitingForSafeDepartVerify &&
                    loginStaff.DepartmentId != AppSettings.SafeDepartId);
        }

        [HttpPost]
        public JsonResult DeleteRisk(int id)
        {
            var viewRiskBll = new ViewRiskInfoBll();
            var risk = viewRiskBll.QuerySingle(id);

            if (risk == null)
            {
                return Json(ErrorModel.InputError);
            }

            var loginStaff = LoginStatus.GetLoginUser();
            if (loginStaff.DepartmentId != risk.ReportStaffDepartId &&
                loginStaff.DepartmentId != AppSettings.SafeDepartId)
            {
                return Json(ErrorModel.NoAuth);
            }

            var riskBll = new RiskInfoBll();
            var respondBll = new RiskResponseStaffBll();
            var success = viewRiskBll.ExecuteTransation(() =>
            {
                var res1 = riskBll.DeleteSoftly(id);
                var res2 = respondBll.DeleteSoftly("RiskId=" + id);

                return res1 && res2;
            });

            if (success)
            {
                return Json(ErrorModel.OperateSuccess);
            }

            return Json(ErrorModel.OperateFailed);
        }
        
        public ActionResult DepartVerify()
        {
            ViewBag.DepartId = LoginStatus.GetLoginUser().DepartmentId;

            return View();
        }

        public ActionResult FinalVerify()
        {
            return View();
        }

        public ActionResult Reform()
        {
            ViewBag.Depart = LoginStatus.GetLoginUser().DepartmentId;

            return View();
        }

        public ActionResult Fix()
        {
            ViewBag.Depart = LoginStatus.GetLoginUser().DepartmentId;

            return View();
        }

        public ActionResult Write()
        {
            var id = RouteData.Values["id"].ToString().ToInt32();
            var viewRiskBll = new ViewRiskInfoBll();
            var riskInfo = viewRiskBll.QuerySingle(id);

            var respondBll = new ViewRiskResponseStaffBll();
            var respondStaff = respondBll.QueryList("RiskId=" + id).FirstOrDefault();

            if (riskInfo == null || respondStaff == null)
            {
                return Redirect("/Error/NotFound");
            }

            ViewBag.Title = "整改处置";
            if (riskInfo.HasDealed)
            {
                ViewBag.Title = "落实销号";
            }

            ViewBag.Risk = riskInfo;
            ViewBag.Respond = respondStaff;

            return View();
        }

        [HttpPost]
        public JsonResult Write(string json)
        {
            var riskInfo = JsonHelper.Deserialize<RiskInfo>(json);
            if (riskInfo == null)
            {
                return Json(ErrorModel.InputError);
            }

            var riskBll = new RiskInfoBll();

            riskInfo.HasDealed = true;

            var success = riskBll.Update(riskInfo, new[] {"HasDealed", "RiskFix", "RiskReason"});
            if (success)
            {
                return Json(ErrorModel.OperateSuccess);
            }
            
            return Json(ErrorModel.OperateFailed);
        }

        /// <summary>
        /// 评价体系/干部履责评价
        /// </summary>
        public ActionResult StaffScore()
        {
            return View();
        }
    }
}
