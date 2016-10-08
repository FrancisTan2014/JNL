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
                var model = new TempSourceResult { Name = group.Key};
                group.ForEach(item =>
                {
                    switch (item.Name2)
                    {
                        case "红线":  model.Red = item.Count;  break;
                        case "甲Ⅰ":  model.One = item.Count;  break;
                        case "甲Ⅱ":  model.Two = item.Count;  break;
                        case "乙":  model.Three = item.Count;  break;
                        case "丙":  model.Four = item.Count;  break;
                        case "信息":  model.Info = item.Count;  break;
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
            
            list.GroupBy(item=>item.LocoModel).ForEach(group =>
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
    }
}
