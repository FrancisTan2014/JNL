using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JNL.Bll;
using JNL.Model;
using JNL.Web.Models;
using JNL.Web.Utils;
using WebGrease.Css.Extensions;

namespace JNL.Web.Controllers
{
    public class AnalysisController : Controller
    {
        #region 研判预警/数据分析预警/来源分析
        public ActionResult Source()
        {
            return View();
        }

        private class TempSourceResult
        {
            public string Name { get; set; }
            /// <summary>
            /// 红线
            /// </summary>
            public int Red { get; set; }
            /// <summary>
            /// 甲一
            /// </summary>
            public int One { get; set; }
            /// <summary>
            /// 甲二
            /// </summary>
            public int Two { get; set; }
            /// <summary>
            /// 乙
            /// </summary>
            public int Three { get; set; }
            /// <summary>
            /// 丙
            /// </summary>
            public int Four { get; set; }
            /// <summary>
            /// 信息
            /// </summary>
            public int Info { get; set; }
            /// <summary>
            /// 总 计
            /// </summary>
            public int Total { get; set; }
        }

        private class TempSourceModel
        {
            public string Name1 { get; set; }
            public string Name2 { get; set; }
            public int Count { get; set; }
        }

        [HttpPost]
        public JsonResult Source(int type, string start, string end)
        {
            if (string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
            {
                return Json(ErrorModel.InputError);
            }

            var cmdText = string.Empty;
            if (type == 1)
            {
                cmdText = $@"SELECT RiskType AS Name1,RiskSecondLevelName AS Name2,COUNT(1) AS [Count] FROM
                           (SELECT RiskTypeId,RiskType,RiskSecondLevelId,RiskSecondLevelName FROM ViewRiskInfo WHERE OccurrenceTime>'{start}' AND OccurrenceTime<'{end}') AS NEWT
                            GROUP BY RiskTypeId,RiskType,RiskSecondLevelId,RiskSecondLevelName
                            HAVING RiskSecondLevelName IN('红线','甲Ⅰ','甲Ⅱ','乙','丙','信息')";
            }
            else if (type == 2)
            {
                cmdText = $@"SELECT ReportStaffDepart AS Name1,RiskSecondLevelName AS Name2,COUNT(1) AS [Count] FROM
                (SELECT ReportStaffDepartId,ReportStaffDepart,RiskSecondLevelId,RiskSecondLevelName FROM ViewRiskInfo WHERE OccurrenceTime>'{start}' AND OccurrenceTime<'{end}') AS NEWT
                GROUP BY ReportStaffDepartId,ReportStaffDepart,RiskSecondLevelId,RiskSecondLevelName
                HAVING RiskSecondLevelName IN('红线','甲Ⅰ','甲Ⅱ','乙','丙','信息')";
            }
            else
            {
                return Json(ErrorModel.InputError);
            }

            var bll = new RiskInfoBll();
            var list = bll.ExecuteModel<TempSourceModel>(cmdText);

            var analysisList = list.GroupBy(t => t.Name1).Select(group =>
            {
                var model = new TempSourceResult { Name = group.Key };
                group.ForEach(item =>
                {
                    switch (item.Name2)
                    {
                        case "红线": model.Red = item.Count; break;
                        case "甲Ⅰ": model.One = item.Count; break;
                        case "甲Ⅱ": model.Two = item.Count; break;
                        case "乙": model.Three = item.Count; break;
                        case "丙": model.Four = item.Count; break;
                        case "信息": model.Info = item.Count; break;
                    }
                    model.Total += item.Count;
                });

                return model;
            });

            return Json(ErrorModel.GetDataSuccess(analysisList));
        }
        #endregion

        #region 研判预警/机车质量跟踪/JT6分析 JT28分析

        public ActionResult Live6()
        {
            return View();
        }

        public ActionResult Live28()
        {
            return View();
        }

        private class TempLive
        {
            public string Model { get; set; }
            public string Type { get; set; }
            public int Gydq { get; set; }
            public int Dydqjdxl { get; set; }
            public int Dj { get; set; }
            public int Dzxl { get; set; }
            public int Zdxt { get; set; }
            public int Zxb { get; set; }
            public int Aqzb { get; set; }
            public int Ctjqt { get; set; }
            public int Cyj { get; set; }
            public int Fzcd { get; set; }
        }

        [HttpPost]
        public JsonResult Live6(int year, int month)
        {
            var cmdText =
                "SELECT Id,LocoModelId,LocoModel,LocoType,LocoTypeId,LivingItemId,LivingItem FROM ViewLocoQuality6 WHERE LocomotiveId<>0";
            if (year > 0)
            {
                cmdText += " AND DATEPART(YEAR, StartRepair)=" + year;
            }
            if (month > 0)
            {
                cmdText += " AND DATEPART(MONTH, StartRepair)=" + month;
            }

            var list = new ViewLocoQuality6Bll().ExecuteModel<ViewLocoQuality6>(cmdText);
            var result = new List<TempLive>();

            list.GroupBy(item => item.LocoModel).ForEach(group =>
              {
                  var temp = new TempLive
                  {
                      Model = group.Key
                  };

                  group.ForEach(item =>
                  {
                      temp.Type = item.LocoType;
                      switch (item.LivingItemId)
                      {
                          case 283: temp.Gydq++; break;
                          case 284: temp.Dydqjdxl++; break;
                          case 285: temp.Dj++; break;
                          case 286: temp.Dzxl++; break;
                          case 287: temp.Zdxt++; break;
                          case 288: temp.Zxb++; break;
                          case 289: temp.Aqzb++; break;
                          case 290: temp.Ctjqt++; break;
                          case 291: temp.Cyj++; break;
                          case 292: temp.Fzcd++; break;
                      }
                  });

                  result.Add(temp);
              });

            return Json(result);
        }

        [HttpPost]
        public JsonResult Live28(int year, int month)
        {
            var cmdText =
                "SELECT Id,LocoModelId,LocoModel,LocoType,LocoTypeId,LivingItemId,LivingItem FROM ViewLocoQuality28 WHERE LocomotiveId<>0";
            if (year > 0)
            {
                cmdText += " AND DATEPART(YEAR, ReportTime)=" + year;
            }
            if (month > 0)
            {
                cmdText += " AND DATEPART(MONTH, ReportTime)=" + month;
            }

            var list = new ViewLocoQuality28Bll().ExecuteModel<ViewLocoQuality28>(cmdText);
            var result = new List<TempLive>();

            list.GroupBy(item => item.LocoModel).ForEach(group =>
            {
                var temp = new TempLive
                {
                    Model = group.Key
                };

                group.ForEach(item =>
                {
                    temp.Type = item.LocoType;
                    switch (item.LivingItemId)
                    {
                        case 283: temp.Gydq++; break;
                        case 284: temp.Dydqjdxl++; break;
                        case 285: temp.Dj++; break;
                        case 286: temp.Dzxl++; break;
                        case 287: temp.Zdxt++; break;
                        case 288: temp.Zxb++; break;
                        case 289: temp.Aqzb++; break;
                        case 290: temp.Ctjqt++; break;
                        case 291: temp.Cyj++; break;
                        case 292: temp.Fzcd++; break;
                    }
                });

                result.Add(temp);
            });

            return Json(result);
        }
        #endregion

        #region 评价体系/安全发展趋势
        public ActionResult SafeTrend()
        {
            return View();
        }

        private class TempTrend
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int RiskSecondLevelId { get; set; }
            public int ReportStaffDepartId { get; set; }
            public int MonthAmmount { get; set; }
        }

        [HttpPost]
        public JsonResult SafeTrend(int year, int departId)
        {
            var cmdText = BuildSqlForTrend(year, departId);
            var modelList = new ViewRiskInfoBll().ExecuteModel<TempTrend>(cmdText).ToList();

            // 每月总数字典
            var monthAmmountsDic = modelList.GroupBy(t => t.Month)
                .ToDictionary(t => t.Key, t => t.Sum(item => item.MonthAmmount));

            var groupList = modelList.GroupBy(t => t.RiskSecondLevelId).ToList();

            // 红线：2 甲类：167，298 乙：657 丙：1223
            var hx = GetSingleTypeMonthAmmounts(new List<int> {2}, groupList);
            var jia = GetSingleTypeMonthAmmounts(new List<int> {167, 298}, groupList);
            var yi = GetSingleTypeMonthAmmounts(new List<int> {657}, groupList);
            var bing = GetSingleTypeMonthAmmounts(new List<int> {1223}, groupList);

            // 甲类及以上占比
            var roteList = new double[12];
            for (var i = 0; i < 12; i++)
            {
                var rote = 0d;
                var month = i + 1;
                if (monthAmmountsDic.ContainsKey(month))
                {
                    var monthTotal = monthAmmountsDic[month];
                    if (monthTotal != 0)
                    {
                        rote = (hx[i] + jia[i])/(monthTotal*1.0);
                    }
                }

                roteList[i] = rote;
            }

            return Json(new
            {
                ammounts = new List<object>
                {
                    new { name = "红线", value = hx },
                    new { name = "甲", value = jia },
                    new { name = "乙", value = yi },
                    new { name = "丙", value = bing }
                },
                rotes = new { name = "甲类及以上占比", value = roteList }
            });
        }

        private int[] GetSingleTypeMonthAmmounts(List<int> types, IEnumerable<IGrouping<int, TempTrend>> groupList)
        {
            var result = new int[12];

            var groups = groupList.Where(g => types.Contains(g.Key));
            groups.ForEach(group =>
            {
                group.ForEach(trend =>
                {
                    result[trend.Month - 1] += trend.MonthAmmount;
                });
            });

            return result;
        }

        private string BuildSqlForTrend(int year, int departId)
        {
            if (departId > 0)
            {
                return $@"SELECT [Year],[Month],[RiskSecondLevelId],ReportStaffDepartId,COUNT(Id) AS MonthAmmount FROM
                          (SELECT DATEPART(YEAR, OccurrenceTime) AS [Year], DATEPART(MONTH, OccurrenceTime) AS [Month],Id, ReportStaffDepartId,RiskSecondLevelId FROM ViewRiskInfo) T
                          GROUP BY [Year],[Month],RiskSecondLevelId,ReportStaffDepartId
                          HAVING [Year]={year} AND ReportStaffDepartId={departId}";
            }

            return $@"SELECT [Year],[Month],[RiskSecondLevelId],COUNT(Id) AS MonthAmmount FROM
                          (SELECT DATEPART(YEAR, OccurrenceTime) AS [Year], DATEPART(MONTH, OccurrenceTime) AS [Month],Id, ReportStaffDepartId,RiskSecondLevelId FROM ViewRiskInfo) T
                          GROUP BY [Year],[Month],RiskSecondLevelId
                          HAVING [Year]={year}";
        }
        #endregion

        #region 研判预警/预警信息管理/预警信息统计

        public ActionResult Warn()
        {
            return View();
        }

        private class TempWarn
        {
            public int DepartId { get; set; }
            public string Depart { get; set; }
            public int Count { get; set; }
        }

        private class TempWarnResult
        {
            public string Name { get; set; }
            public int Warn { get; set; }
            public int Risk { get; set; }
        }

        [HttpPost]
        public JsonResult Warn(string startTime, string endTime)
        {
            var warningBll = new WarningBll();
            var riskCmdText = BuildSqlForWarn(1, startTime, endTime);
            var riskList = warningBll.ExecuteModel<TempWarn>(riskCmdText).ToList();

            var warningCmdText = BuildSqlForWarn(2, startTime, endTime);
            var warningList = warningBll.ExecuteModel<TempWarn>(warningCmdText).ToList();

            var dic = new Dictionary<int, TempWarnResult>();
            riskList.ForEach(r =>
            {
                if (!dic.ContainsKey(r.DepartId) && r.DepartId != 0)
                {
                    var warnModel = warningList.FirstOrDefault(w => w.DepartId == r.DepartId);
                    dic.Add(r.DepartId, new TempWarnResult
                    {
                        Name = r.Depart,
                        Risk = r.Count,
                        Warn = warnModel?.Count ?? 0
                    });
                }
            });
            warningList.ForEach(w =>
            {
                if (!dic.ContainsKey(w.DepartId) && w.DepartId != 0)
                {
                    var riskModel = riskList.FirstOrDefault(r => r.DepartId == w.DepartId);
                    dic.Add(w.DepartId, new TempWarnResult
                    {
                        Name = w.Depart,
                        Risk = riskModel?.Count ?? 0,
                        Warn = w.Count
                    });
                }
            });

            return Json(dic.Values.ToList());
        }

        private string BuildSqlForWarn(int type, string startTime, string endTime)
        {
            if (string.IsNullOrEmpty(startTime))
            {
                startTime = "1970-01-01";
            }
            if (string.IsNullOrEmpty(endTime))
            {
                endTime = "2050-01-01";
            }

            return type == 1 
                ? $@"SELECT ReportStaffDepartId AS DepartId,ReportStaffDepart AS Depart,COUNT(Id) AS [Count] FROM
                     (SELECT Id,ReportStaffDepartId,ReportStaffDepart,OccurrenceTime FROM ViewRiskInfo WHERE OccurrenceTime BETWEEN '{startTime}' AND '{endTime}') T
                     GROUP BY ReportStaffDepartId,ReportStaffDepart"
                : $@"SELECT WarningDepartId AS DepartId,WarningDepart AS Depart,COUNT(Id) AS [Count] FROM
                    (SELECT Id,WarningDepartId,WarningDepart,WarningTime FROM ViewWarning WHERE WarningTime BETWEEN '{startTime}' AND '{endTime}') T
                    GROUP BY WarningDepartId,WarningDepart";
        }
        #endregion
    }
}
