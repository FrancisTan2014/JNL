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
using NPOI.OpenXmlFormats;

namespace JNL.Web.Controllers
{
    /// <summary>
    /// 后台管理
    /// </summary>
    public class AdminController : Controller
    {
        /// <summary>
        /// 员工列表管理
        /// </summary>
        public ActionResult StaffList()
        {
            return View();
        }

        /// <summary>
        /// 添加/修改员工信息
        /// </summary>
        /// <returns></returns>
        public ActionResult EditStaff()
        {
            var id = RouteData.Values["id"];
            var staffId = id?.ToString().ToInt32() ?? 0;
            var staff = new StaffBll().QuerySingle(staffId) ?? new Staff();

            return View(staff);
        }

        /// <summary>
        /// 保存员工信息
        /// </summary>
        [HttpPost]
        public JsonResult SaveStaff(string json)
        {
            var model = JsonHelper.Deserialize<Staff>(json);
            if (model == null)
            {
                return Json(ErrorModel.InputError);
            }

            // 将工号设置为密码
            model.Password = model.SalaryId.GetMd5();

            var bll = new StaffBll();

            bool success;
            if (model.Id > 0)
            {
                success = bll.Update(model);
            }
            else
            {
                success = bll.Insert(model).Id > 0;
            }

            if (success)
            {
                return Json(ErrorModel.OperateSuccess);
            }

            return Json(ErrorModel.OperateFailed);
        }

        /// <summary>
        /// 风险概述管理
        /// </summary>
        public ActionResult RiskSummary()
        {
            var riskSummaryBll = new RiskSummaryBll();
            var riskSummaryList = riskSummaryBll.QueryList("ParentId=0").ToList();

            ViewBag.SummaryList = riskSummaryList;

            return View();
        }

        /// <summary>
        /// 更新风险概述信息
        /// </summary>
        [HttpPost]
        public JsonResult UpdateSummary(int id, string desc)
        {
            var model = new RiskSummary
            {
                Id = id,
                Description = desc
            };

            var bll = new RiskSummaryBll();
            var success = bll.Update(model, new[] { "Description", "UpdateTime" });
            if (success)
            {
                return Json(ErrorModel.OperateSuccess);
            }

            return Json(ErrorModel.OperateFailed);
        }

        /// <summary>
        /// 添加风险概述信息
        /// </summary>
        [HttpPost]
        public JsonResult AddSummary(string json)
        {
            var model = JsonHelper.Deserialize<RiskSummary>(json);
            if (model == null)
            {
                return Json(ErrorModel.InputError);
            }

            var bll = new RiskSummaryBll();
            model.IsBottom = bll.IsChildrenBottom(model.ParentId);

            var success = bll.Insert(model).Id > 0;

            if (success)
            {
                return Json(ErrorModel.GetDataSuccess(new
                {
                    id = model.Id,
                    bottom = model.IsBottom
                }));
            }

            return Json(ErrorModel.OperateFailed);
        }

        /// <summary>
        /// 计算最近三年的得分
        /// </summary>
        /// <returns></returns>
        public JsonResult ComputeStaffScore(string password)
        {
            if (password != "yaozhilu")
            {
                return Json(ErrorModel.NoAuth, JsonRequestBehavior.AllowGet);
            }

            var currentYear = DateTime.Now.Year;
            StaffScoreHelper.ComputeWholeYearStaffScore(currentYear);
            StaffScoreHelper.ComputeWholeYearStaffScore(currentYear - 1);
            StaffScoreHelper.ComputeWholeYearStaffScore(currentYear - 2);

            return Json(ErrorModel.OperateSuccess, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 字典维护
        /// </summary>
        public ActionResult Dictionaries()
        {
            return View();
        }

        public JsonResult GetDictories(int? type, int parent)
        {
            var condition = type == null 
                ? $"ParentId={parent}" 
                : $"Type={type} AND ParentId={parent}";

            var bll = new DictionariesBll();
            var data = bll.QueryList(condition);
            var jsonArray = data.Select(item =>
            {
                var childType = 0;
                if (item.HasChildren)
                {
                    var children = bll.QueryList($"ParentId={item.Id}").First();
                    childType = children.Type;
                }

                return new
                {
                    item.Id,
                    item.HasChildren,
                    item.Type,
                    item.AddTime,
                    item.IsDelete,
                    item.Name,
                    item.ParentId,
                    childType
                };
            });

            return Json(ErrorModel.GetDataSuccess(jsonArray));
        }

        /// <summary>
        /// 更新字典表数据
        /// </summary>
        [HttpPost]
        public JsonResult EditDictionary(string json)
        {
            var model = JsonHelper.Deserialize<Dictionaries>(json);
            if (model == null)
            {
                return Json(ErrorModel.InputError);
            }

            var bll = new DictionariesBll();

            bool success;
            if (model.Id > 0)
            {
                success = bll.Update(model, new[] {"Name"});
            }
            else
            {
                success = bll.Insert(model).Id > 0;
            }

            if (success)
            {
                return Json(ErrorModel.GetDataSuccess(model));
            }

            return Json(ErrorModel.GetDataFailed);
        }
    }
}
