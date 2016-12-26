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
        #region 员工管理
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
        #endregion

        #region 风险概述管理
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
        #endregion

        #region 计算最近三年的得分
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
        #endregion

        #region 字典维护
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
                success = bll.Update(model, new[] { "Name" });
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
        #endregion

        #region 录入情况统计
        /// <summary>
        /// 指标管理
        /// </summary>
        public ActionResult Quota()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddQuota(int staffId, int ammount)
        {
            if (staffId <= 0 || ammount <= 0)
            {
                return Json(ErrorModel.InputError);
            }

            var quota = new Quota
            {
                QuotaType = 1,
                QuotaAmmount = ammount,
                StaffId = staffId
            };

            var quotaBll = new QuotaBll();
            if (quotaBll.Insert(quota).Id > 0)
            {
                return Json(ErrorModel.OperateSuccess);
            }

            return Json(ErrorModel.OperateFailed);
        }

        [HttpPost]
        public JsonResult UpdateQuota(int quotaId, int ammount)
        {
            if (quotaId <= 0 || ammount <= 0)
            {
                return Json(ErrorModel.InputError);
            }

            var quotaBll = new QuotaBll();
            var quota = quotaBll.QuerySingle(quotaId);
            if (quota != null)
            {
                quota.QuotaAmmount = ammount;
                quota.UpdateTime = DateTime.Now;

                if (quotaBll.Update(quota))
                {
                    return Json(ErrorModel.OperateSuccess);
                }

                return Json(ErrorModel.OperateFailed);
            }

            return Json(ErrorModel.InputError);
        }

        /// <summary>
        /// 指标完成情况统计
        /// </summary>
        public ActionResult QuotaAchievement()
        {
            return View();
        }
        #endregion

        #region 车站管理

        public ActionResult Stations()
        {
            return View();
        }

        /// <summary>
        /// 添加/修改车站
        /// </summary>
        [HttpPost]
        public JsonResult EditStation(string json)
        {
            var station = JsonHelper.Deserialize<Station>(json);
            if (station == null)
            {
                return Json(ErrorModel.InputError);
            }

            var stationBll = new StationBll();
            if (stationBll.Exists($"[Name]='{station.Name}'"))
            {
                return Json(ErrorModel.ExistSameItem);
            }

            bool success;
            if (station.Id == 0)
            {
                success = stationBll.Insert(station).Id > 0;
            }
            else
            {
                success = stationBll.Update(station);
            }

            return Json(success ? ErrorModel.OperateSuccess : ErrorModel.OperateFailed);
        }

        #endregion

        #region 线路管理

        public ActionResult Lines()
        {
            return View();
        }

        public ActionResult EditLine()
        {
            var id = RouteData.Values["id"];
            ViewBag.Id = id?.ToString().ToInt32() ?? 0;
            ViewBag.Title = id == null ? "添加线路" : "修改线路信息";

            return View();
        }

        [HttpPost]
        public JsonResult EditLine(int lineId)
        {
            var lineName = Request["lineName"];
            var firstStation = Request["firstStation"];
            var lastStation = Request["lastStation"];
            var stationIds = Request["stationIds"];

            if (string.IsNullOrEmpty(lineName) || string.IsNullOrEmpty(stationIds) || string.IsNullOrEmpty(firstStation) || string.IsNullOrEmpty(lastStation))
            {
                return Json(ErrorModel.InputError);
            }

            var lineBll = new LineBll();
            #region 执行事务，更新Line及LineStations表的数据
            var success = lineBll.ExecuteTransation(() =>
                {
                    // 更新Line表数据
                    var line = new Line
                    {
                        Id = lineId,
                        FirstStation = firstStation,
                        LastStation = lastStation,
                        Name = lineName
                    };

                    bool lineSuccess;
                    if (lineId == 0)
                    {
                        lineId = lineBll.Insert(line).Id;
                        lineSuccess = lineId > 0;
                    }
                    else
                    {
                        lineSuccess = lineBll.Update(line);
                    }

                    if (lineSuccess)
                    {
                        // 删除车站线路关联表数据
                        var relateBll = new LineStationsBll();
                        bool deleteSuccess = true;
                        if (relateBll.Exists($"LineId={lineId}"))
                        {
                            deleteSuccess = relateBll.Delete($"LineId={lineId}");
                        }

                        if (!deleteSuccess)
                        {
                            // 存在且删除失败，事务回滚
                            return false;
                        }

                        //插入新的车站线路关联表数据
                        var sort = 0;
                        var newRelations = stationIds.Split(new[] { "###" }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(str => str.ToInt32())
                                .Select(stationId => new LineStations
                                {
                                    LineId = lineId,
                                    StationId = stationId,
                                    Sort = ++sort
                                });

                        relateBll.BulkInsert(newRelations);

                        return true;
                    }

                    return false;
                });
            #endregion

            if (success)
            {
                return Json(ErrorModel.OperateSuccess);
            }

            return Json(ErrorModel.OperateFailed);
        }

        #endregion
    }
}
