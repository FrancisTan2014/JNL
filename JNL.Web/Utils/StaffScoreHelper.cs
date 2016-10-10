using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using JNL.Bll;
using JNL.Model;

namespace JNL.Web.Utils
{
    public static class StaffScoreHelper
    {
        public static void StartTask()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    ComputeStaffScore();
                    Thread.Sleep(24*60*60*1000); // 每天执行一次
                }
            });
        }

        /// <summary>
        /// 计算本月每个风险责任人所扣的分数
        /// </summary>
        public static void ComputeStaffScore()
        {
            try
            {
                // 查询本月风险信息集合
                var riskBll = new ViewRiskInfoBll();
                var monthRisks =
                    riskBll.QueryList(
                        $"DATEPART(YEAR, OccurrenceTime)={DateTime.Now.Year} AND DATEPART(MONTH, OccurrenceTime)={DateTime.Now.Month}",
                        new[] { "Id", "RiskSecondLevelId" }).ToList();
                if (!monthRisks.Any())
                {
                    return;
                }

                // 查询本月风险责任人
                var respondBll = new RiskResponseStaffBll();
                var riskIdList = monthRisks.Select(r => r.Id);
                var respondList = respondBll.QueryList($"RiskId IN({string.Join(",", riskIdList)})", new[] { "RiskId", "ResponseStaffId" });

                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;
                var minusScoreDic = AppSettings.RiskMinusScoreDic;

                // 计算每个责任人所扣的分数
                var staffScoreList = monthRisks.Join(respondList, outer => outer.Id, inner => inner.RiskId, (outer, inner) => new StaffScore
                {
                    StaffId = inner.ResponseStaffId,
                    MinusScore = minusScoreDic.ContainsKey(outer.RiskSecondLevelId ?? 0) ? minusScoreDic[outer.RiskSecondLevelId.Value] : 0
                })
                .GroupBy(s => s.StaffId)
                .Select(group => new StaffScore
                {
                    StaffId = group.Key,
                    Year = year,
                    Month = month,
                    MinusScore = group.Sum(s => s.MinusScore)
                });

                var staffScoreBll = new StaffScoreBll();
                staffScoreBll.ExecuteTransation(() =>
                {
                    if (staffScoreBll.Delete($"Year={year} AND Month={month}"))
                    {
                        staffScoreBll.BulkInsert(staffScoreList);
                        return true;
                    }

                    return false;
                });
            }
            catch (Exception ex)
            {
                ExceptionLogBll.ExceptionPersistence(nameof(StaffScoreHelper), nameof(StaffScoreHelper), ex);
            }
        }
    }
}