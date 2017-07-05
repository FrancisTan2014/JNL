using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

            // @FrancisTan 2016-12-23
            // 修改提报人录入，改为以当前登录人信息填充
            // 并且取消手动输出提报人功能
            var loginUser = LoginStatus.GetLoginUser();
            ViewBag.ReportUserId = loginUser.Id;
            ViewBag.ReportUserInfo = $"{loginUser.SalaryId} | {loginUser.Name} | {loginUser.WorkId}";
            ViewBag.ReportDepart = loginUser.Department;

            return View();
        }

        [HttpPost]
        public JsonResult AddRisk(string risk, string responds)
        {
            // 启动每月为每个员工插入一条风险信息记录指标的任务
            var quotaAchievementBll = new QuotaAchievementBll();
            if (!quotaAchievementBll.Exists($"IsDelete=0 AND [Year]={DateTime.Now.Year} AND [Month]={DateTime.Now.Month}"))
            {
                AddQuotaAchievementForEveryStaffPerMonth();
            }

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

            if (riskInfo == null)
            {
                return Json(ErrorModel.InputError);
            }

            // 2016-12-18 将处置期限设置为当前时间+72小时
            riskInfo.DealTimeLimit = riskInfo.VerifyTime.AddDays(3);

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

        // 启动每月为每个员工插入一条风险信息记录指标的任务
        private void AddQuotaAchievementForEveryStaffPerMonth()
        {
            Task.Factory.StartNew(() =>
            {
                var quotaBll = new QuotaBll();
                var quotaAchievementBll = new QuotaAchievementBll();
                var quotaList = quotaBll.QueryList("IsDelete=0");

                var achieveList = new List<QuotaAchievement>();
                quotaList.ForEach(q =>
                {
                    achieveList.Add(new QuotaAchievement
                    {
                        AchieveAmmount = 0,
                        Month = DateTime.Now.Month,
                        Year = DateTime.Now.Year,
                        QuotaId = q.Id,
                        UpdateTime = DateTime.Now
                    });
                });

                quotaAchievementBll.BulkInsert(achieveList);
            });
        }

        /// <summary>
        /// 更新风险信息指标完成情况
        /// </summary>
        /// <since>2016-12-23 11:11</since>
        private bool UpdateQuotaAchievement(RiskInfo riskInfo)
        {
            var quotaBll = new QuotaBll();
            var quota = quotaBll.QuerySingle($"[QuotaType]=1 AND StaffId={riskInfo.ReportStaffId}");

            // 检查当前登录人是否被指定风险信息录入条数指标
            // 存在指标则更新指标完成情况，不存在则直接返回true
            if (quota != null)
            {
                var quotaAchievementBll = new QuotaAchievementBll();
                var condition = $"QuotaId={quota.Id} AND [Year]={DateTime.Now.Year} AND [Month]={DateTime.Now.Month}";
                var record = quotaAchievementBll.QuerySingle(condition);

                // 检查当前登录人本月指标完成情况记录是否存在
                // 不存在则创建新记录，存在则更新记录
                if (record == null)
                {
                    record = new QuotaAchievement
                    {
                        QuotaId = quota.Id,
                        AchieveAmmount = 1,
                        Year = DateTime.Now.Year,
                        Month = DateTime.Now.Month
                    };

                    return quotaAchievementBll.Insert(record).Id > 0;
                }

                record.AchieveAmmount += 1;
                return quotaAchievementBll.Update(record);
            }

            return true;
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

        /// <summary>
        /// 更新风险信息，同时，若此风险信息被置为需要进入
        /// 每日重点甄别队列，将此风险信息COPY到TraceInfo中
        /// </summary>
        private object UpdateRisk(RiskInfo risk, List<RiskResponseStaff> responds)
        {
            var riskBll = new RiskInfoBll();
            var respondBll = new RiskResponseStaffBll();

            var success = riskBll.ExecuteTransation(() =>
            {
                var res1 = riskBll.Update(risk);
                if (res1 && risk.VerifyStatus == 4)
                {
                    // @FrancisTan 2017-07-04
                    // 风险办审核通过后将个人添加风险记录条数+1
                    UpdateQuotaAchievement(risk);
                }

                var res2 = respondBll.Delete($"RiskId={risk.Id}");
                if (responds.Any())
                {
                    respondBll.BulkInsert(responds);
                }

                return res1 && res2;
            }, () =>
            {
                if (risk.ShowInStressPage)
                {
                    var viewRespondBll = new ViewRiskResponseStaffBll();
                    var respondIds = viewRespondBll.QueryList($"RiskId={risk.Id}").Select(t => t.DepartmentId ?? 0);
                    
                    // 插入每日重点甄别队友信息
                    var traceInfo = new TraceInfo
                    {
                        ResponseDepartmentIds = string.Join(",", respondIds),
                        TraceContent = risk.RiskDetails
                    };

                    return new TraceInfoBll().Insert(traceInfo).Id > 0;
                }
                return true;
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

        /// <summary>
        /// 研判预警/数据分析预警/综合分析
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryAll()
        {
            return View();
        }

        public ActionResult Detail()
        {
            var id = RouteData.Values["id"].ToString().ToInt32();
            //var model = new ViewRiskRespondRiskBll().QuerySingle(id);
            var model = new ViewRiskRespondRiskBll().QuerySingle($"RiskId={id}");

            if (model == null)
            {
                return Redirect("/Error/NotFound");
            }

            return View(model);
        }
    }
}
