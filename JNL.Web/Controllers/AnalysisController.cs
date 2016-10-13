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
            var hx = GetSingleTypeMonthAmmounts(new List<int> { 2 }, groupList);
            var jia = GetSingleTypeMonthAmmounts(new List<int> { 167, 298 }, groupList);
            var yi = GetSingleTypeMonthAmmounts(new List<int> { 657 }, groupList);
            var bing = GetSingleTypeMonthAmmounts(new List<int> { 1223 }, groupList);

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
                        rote = (hx[i] + jia[i]) / (monthTotal * 1.0);
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

        #region 研判预警/职工行为预警

        public ActionResult Action()
        {
            return View();
        }

        private class TempAction
        {
            public int StaffId { get; set; }
            public int DepartmentId { get; set; }
            public string Department { get; set; }
            public int Year { get; set; }
            public decimal MinusScore { get; set; }
        }

        [HttpPost]
        public JsonResult Action(int year)
        {
            var cmdText =
                $@"SELECT StaffId,DepartmentId,Department,[Year],SUM(MinusScore) AS MinusScore FROM ViewStaffScore
                   GROUP BY StaffId,DepartmentId,Department,[Year]
                   HAVING [Year]={year}";
            var list = new ViewRiskInfoBll().ExecuteModel<TempAction>(cmdText);

            var result = list.GroupBy(t => t.Department).Select(group =>
            {
                var yellow = group.Count(t => t.MinusScore >= 300 && t.MinusScore < 400);
                var orange = group.Count(t => t.MinusScore >= 400 && t.MinusScore < 500);
                var red = group.Count(t => t.MinusScore >= 500);

                return new
                {
                    name = group.Key,
                    yellow,
                    orange,
                    red
                };
            });

            return Json(result);
        }
        #endregion

        #region 研判预警/数据分析预警/分布分析

        public ActionResult Distribute()
        {
            return View();
        }

        private class TempDistribute
        {
            public string Name { get; set; }
            public decimal Hx { get; set; }
            public decimal Jy { get; set; }
            public decimal Je { get; set; }
            public decimal Yi { get; set; }
            public decimal Bi { get; set; }
            public decimal Xx { get; set; }
            public decimal Hj { get; set; }

            public decimal Zj { get; set; }
            public decimal Wj { get; set; }
        }

        private class TempDistributeModel1
        {
            public string RiskSecondLevelName { get; set; }
            public string ReportStaffDepart { get; set; }
            public int Count { get; set; }
        }

        [HttpPost]
        public JsonResult Distribute(int type, string start, string end)
        {
            var cmdText = BuildSqlForDistribute(type, start, end);

            object result = null;
            switch (type)
            {
                case 1: result = AbsoluteCount(cmdText); break;
                case 2: result = AbsolutePercent(cmdText); break;
            }

            return Json(result);
        }

        private object AbsolutePercent(string cmdText)
        {
            var countList = AbsoluteCount(cmdText) as List<TempDistribute>;
            if (countList != null)
            {
                var hxTotal = countList.Sum(t => t.Hx);
                var jyTotal = countList.Sum(t => t.Jy);
                var jeTotal = countList.Sum(t => t.Je);
                var yiTotal = countList.Sum(t => t.Yi);
                var biTotal = countList.Sum(t => t.Bi);
                var xxTotal = countList.Sum(t => t.Xx);
                var hjTotal = countList.Sum(t => t.Hj);

                countList.ForEach(t =>
                {
                    t.Hx = hxTotal == 0 ? 0 : Math.Round(t.Hx / hxTotal, 4);
                    t.Jy = jyTotal == 0 ? 0 : Math.Round(t.Jy / jyTotal, 4);
                    t.Je = jeTotal == 0 ? 0 : Math.Round(t.Je / jeTotal, 4);
                    t.Yi = yiTotal == 0 ? 0 : Math.Round(t.Yi / yiTotal, 4);
                    t.Bi = biTotal == 0 ? 0 : Math.Round(t.Bi / biTotal, 4);
                    t.Xx = xxTotal == 0 ? 0 : Math.Round(t.Xx / xxTotal, 4);
                    t.Hj = hjTotal == 0 ? 0 : Math.Round(t.Hj / hjTotal, 4);
                });

                return countList;
            }

            return ErrorModel.InputError;
        }

        private object AbsoluteCount(string cmdText)
        {
            var list = new ViewRiskInfoBll().ExecuteModel<TempDistributeModel1>(cmdText);
            var result = list.GroupBy(t => t.ReportStaffDepart).Select(group =>
            {
                var model = new TempDistribute { Name = group.Key };
                group.ForEach(item =>
                {
                    switch (item.RiskSecondLevelName)
                    {
                        case "红线": model.Hx = item.Count; break;
                        case "甲Ⅰ": model.Jy = item.Count; break;
                        case "甲Ⅱ": model.Je = item.Count; break;
                        case "乙": model.Yi = item.Count; break;
                        case "丙": model.Bi = item.Count; break;
                        case "信息": model.Xx = item.Count; break;
                    }

                    model.Hj += item.Count;
                });

                return model;
            }).ToList();

            return result;
        }

        private string BuildSqlForDistribute(int type, string start, string end)
        {
            switch (type)
            {
                case 1:
                case 2:
                    return $@"SELECT RiskSecondLevelId,RiskSecondLevelName,ReportStaffDepartId,ReportStaffDepart,COUNT(1) AS [Count] FROM
                              (SELECT RiskSecondLevelId,RiskSecondLevelName,ReportStaffDepartId,ReportStaffDepart FROM ViewRiskInfo WHERE OccurrenceTime>'{start}' AND OccurrenceTime<'{end}') AS NEWT
                              GROUP BY RiskSecondLevelId,RiskSecondLevelName,ReportStaffDepartId,ReportStaffDepart
                              HAVING RiskSecondLevelName IN('红线','甲Ⅰ','甲Ⅱ','乙','丙','信息')";
                default:
                    return "";
            }
        }
        #endregion

        #region 研判预警/数据分析预警/地域分析

        public ActionResult Area()
        {
            return View();
        }

        private class TempArea1
        {
            public string station { get; set; }
            public int count { get; set; }
        }

        private class TempArea2
        {
            public string first { get; set; }
            public string last { get; set; }
            public int count { get; set; }
        }

        [HttpPost]
        public JsonResult Area(int type, string start, string end)
        {
            var cmdText =
                $@"SELECT FirstStationName,LastStationName FROM ViewRiskInfo WHERE (FirstStationName IS NOT NULL OR LastStationName IS NOT NULL) AND OccurrenceTime BETWEEN '{start}' AND '{end}'";
            var list = new ViewRiskInfoBll().ExecuteModel<ViewRiskInfo>(cmdText);

            object result;
            if (type == 1)
            {
                result =
                    list.Where(t => !string.IsNullOrEmpty(t.FirstStationName) && (string.IsNullOrEmpty(t.LastStationName) || t.FirstStationName == t.LastStationName))
                    .GroupBy(t => t.FirstStationName)
                    .Select(group => new TempArea1 { station = group.Key, count = group.Count() })
                    .OrderByDescending(t => t.count);
            }
            else
            {
                var tempList = new List<TempArea2>();
                list.Where(
                    t =>
                        !string.IsNullOrEmpty(t.FirstStationName) && !string.IsNullOrEmpty(t.LastStationName) &&
                        t.FirstStationName != t.LastStationName)
                    .GroupBy(t => t.FirstStationName)
                    .ForEach(group =>
                    {
                        tempList.AddRange(
                            group.GroupBy(t => t.LastStationName)
                            .Select(g => new TempArea2
                            {
                                first = group.Key,
                                last = g.Key,
                                count = g.Count()
                            }));
                    });
                result = tempList.OrderByDescending(t => t.count);
            }

            return Json(result);
        }
        #endregion

        #region 研判预警/数据分析预警/天气情况分析

        public ActionResult Weather()
        {
            return View();
        }

        private class TempWeather
        {
            public string type { get; set; }
            public string level { get; set; }
            public string summary { get; set; }
            public int weatherId { get; set; }
            public string weather { get; set; }
            public int count { get; set; }
        }

        [HttpPost]
        public JsonResult Weather(int weatherId, string start, string end)
        {
            var cmdText =
                $@"SELECT RiskSecondLevelName AS level,RiskSummary AS summary,WeatherLike AS weather,WeatherId AS weatherId,RiskTopestName AS type, COUNT(1) AS [count] FROM
                (SELECT RiskSecondLevelName,RiskSummary,WeatherLike,WeatherId,RiskTopestName FROM ViewRiskInfo WHERE OccurrenceTime BETWEEN '{start}' AND '{end}') T
                GROUP BY RiskSecondLevelName,RiskSummary,WeatherLike,WeatherId,RiskTopestName";
            if (weatherId > 0)
            {
                cmdText += $" HAVING WeatherId={weatherId}";
            }

            var list = new ViewRiskInfoBll().ExecuteModel<TempWeather>(cmdText).OrderByDescending(t => t.count);

            return Json(list);
        }
        #endregion

        #region 研判预警/数据分析预警/时间段分析

        public ActionResult TimeSection()
        {
            return View();
        }

        private class TempTimeSection
        {
            public string RiskSecondLevelName { get; set; }
            public int Hour { get; set; }
        }

        [HttpPost]
        public JsonResult TimeSection(string start, string end)
        {
            var cmdText =
                $@"SELECT RiskSecondLevelName,DATEPART(HOUR, OccurrenceTime) AS [Hour] FROM ViewRiskInfo WHERE RiskSecondLevelName IS NOT NULL AND RiskSecondLevelName IN('红线','甲Ⅰ','甲Ⅱ','乙','丙','信息') AND OccurrenceTime BETWEEN '{start}' AND '{end}'";
            var list = new ViewRiskInfoBll().ExecuteModel<TempTimeSection>(cmdText).ToList();

            var result = new List<TempDistribute>();
            for (int i = 0; i < 24; i++)
            {
                var hour = i;
                var model = new TempDistribute { Name = hour + "时" };
                var subList = list.Where(t => t.Hour == hour);
                subList.ForEach(item =>
                {
                    switch (item.RiskSecondLevelName)
                    {
                        case "红线": model.Hx++; break;
                        case "甲Ⅰ": model.Jy++; break;
                        case "甲Ⅱ": model.Je++; break;
                        case "乙": model.Yi++; break;
                        case "丙": model.Bi++; break;
                        case "信息": model.Xx++; break;
                    }
                    model.Hj++;
                });

                result.Add(model);
            }

            return Json(result.Select(t => new
            {
                t.Name,
                t.Hx,
                t.Jy,
                t.Je,
                t.Yi,
                t.Bi,
                t.Xx,
                t.Hj
            }));
        }
        #endregion

        #region 研判预警/数据分析预警/构成分析

        public ActionResult Constitute()
        {
            return View();
        }

        private class TempConstitute
        {
            public int RiskSummaryId { get; set; }
            public string RiskSummary { get; set; }
            public int Count { get; set; }
        }

        [HttpPost]
        public JsonResult Constitute(int departId, string start, string end)
        {
            var cmdText = BuildSqlFormConstitute(departId, start, end);
            var list = new ViewRiskInfoBll().ExecuteModel<TempConstitute>(cmdText).ToList();
            var totalCount = list.Sum(t => t.Count);

            if (totalCount > 0)
            {
                var constituteConfig = AppSettings.ConstituteAnalysisRiskSummaryIds;

                var rest = totalCount;
                var result = new List<object>();

                list.ForEach(t =>
                {
                    if (constituteConfig.Contains(t.RiskSummaryId))
                    {
                        rest = rest - t.Count;
                        result.Add(new
                        {
                            name = t.RiskSummary,
                            value = Math.Round(t.Count / (totalCount * 1.0), 4) * 100
                        });
                    }
                });

                result.Add(new
                {
                    name = "其他",
                    value = Math.Round(rest / (totalCount * 1.0), 4) * 100
                });

                return Json(result);
            }

            return Json(ErrorModel.InputError);
        }

        private string BuildSqlFormConstitute(int departId, string start, string end)
        {
            if (departId <= 0)
            {
                return $@"SELECT RiskSummaryId,RiskSummary,COUNT(RiskSummaryId) AS [Count] FROM 
                          (SELECT RiskSummaryId,RiskSummary FROM ViewRiskInfo WHERE OccurrenceTime BETWEEN '{start}' AND '{end}' AND IsDelete=0) T
                          GROUP BY RiskSummaryId,RiskSummary";
            }

            return $@"SELECT RiskSummaryId,RiskSummary,COUNT(RiskSummaryId) AS [Count] FROM 
                      (SELECT RiskSummaryId,RiskSummary FROM ViewRiskRespondRisk WHERE OccurrenceTime BETWEEN '{start}' AND '{end}' AND DepartmentId={departId} AND IsDelete=0) T
                      GROUP BY RiskSummaryId,RiskSummary";
        }
        #endregion

        #region 研判预警/数据分析预警/阶段分析

        public ActionResult Stage()
        {
            return View();
        }

        private class TempStage1
        {
            public int RiskSummaryId { get; set; }
            public string RiskSummary { get; set; }
            public int Count { get; set; }
        }

        private class TempStage2
        {
            public string name { get; set; }
            public double daily1 { get; set; }
            public double daily2 { get; set; }
            public int total1 { get; set; }
            public int total2 { get; set; }
        }
        
        [HttpPost]
        public JsonResult Stage(int departId)
        {
            double daySpan1, daySpan2;
            var stage1 = GetStage1(departId, out daySpan1);
            var stage2 = GetStage2(departId, out daySpan2);

            var summaryIdList = new List<int>();
            summaryIdList.AddRange(stage1.Select(t => t.RiskSummaryId));
            summaryIdList.AddRange(stage2.Select(t => t.RiskSummaryId));

            var result = new List<TempStage2>();
            summaryIdList.Distinct().ForEach(id =>
            {
                var temp1 = stage1.FirstOrDefault(t => t.RiskSummaryId == id);
                var temp2 = stage2.FirstOrDefault(t => t.RiskSummaryId == id);

                var model = new TempStage2
                {
                    name = temp1?.RiskSummary ?? temp2?.RiskSummary,
                    daily1 = Math.Round(temp1?.Count / daySpan1 ?? 0, 2),
                    daily2 = Math.Round(temp2?.Count / daySpan2 ?? 0, 2),
                    total1 = temp1?.Count ?? 0,
                    total2 = temp2?.Count ?? 0
                };

                result.Add(model);
            });

            return Json(result);
        }

        private List<TempStage1> GetStage1(int departId, out double daySpan)
        {
            var start = Request["start1"];
            var end = Request["end1"];
            var cmdText = BuildSqlForStage(departId, start, end);

            daySpan = GetDaySpan(start, end);

            return new ViewRiskInfoBll().ExecuteModel<TempStage1>(cmdText).ToList();
        }

        private List<TempStage1> GetStage2(int departId, out double daySpan)
        {
            var start = Request["start2"];
            var end = Request["end2"];
            var cmdText = BuildSqlForStage(departId, start, end);

            daySpan = GetDaySpan(start, end);

            return new ViewRiskInfoBll().ExecuteModel<TempStage1>(cmdText).ToList();
        }

        private double GetDaySpan(string startTime, string endTime)
        {
            var start = DateTime.Parse(startTime);
            var end = DateTime.Parse(endTime);
            var timeSpan = end - start;

            return timeSpan.TotalDays;
        }

        private string BuildSqlForStage(int departId, string start, string end)
        {
            var config = AppSettings.StageAnalysisRiskSummaryIds;

            if (departId > 0)
            {
                return $@"SELECT RiskSummary,RiskSummaryId,COUNT(RiskSummaryId) AS [Count] FROM
                        (SELECT RiskSummary,RiskSummaryId FROM ViewRiskRespondRisk 
	                    WHERE RiskSummaryId<>0 AND OccurrenceTime BETWEEN '{start}' AND '{end}' AND DepartmentId={departId}) T
                      GROUP BY RiskSummary,RiskSummaryId
                      HAVING RiskSummaryId IN({string.Join(",", config)})";
            }

            return $@"SELECT RiskSummary,RiskSummaryId,COUNT(RiskSummaryId) AS [Count] FROM
                        (SELECT RiskSummary,RiskSummaryId FROM ViewRiskRespondRisk 
	                    WHERE RiskSummaryId<>0 AND OccurrenceTime BETWEEN '{start}' AND '{end}') T
                      GROUP BY RiskSummary,RiskSummaryId
                      HAVING RiskSummaryId IN({string.Join(",", config)})";
        }
        
        #endregion
    }
}
