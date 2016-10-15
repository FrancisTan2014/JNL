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
                    Thread.Sleep(24 * 60 * 60 * 1000); // 每天执行一次
                }
            });
        }

        /// <summary>
        /// 计算整年各月份的员工扣分
        /// </summary>
        /// <param name="year"></param>
        public static void ComputeWholeYearStaffScore(int year)
        {
            for (int i = 0; i < 12; i++)
            {
                ComputeStaffScore(year, i + 1);
            }
        }

        /// <summary>
        /// 计算本月每个风险责任人所扣的分数
        /// </summary>
        public static void ComputeStaffScore(int year = 0, int month = 0)
        {
            if (year <= 0)
            {
                year = DateTime.Now.Year;
            }
            if (month <= 0)
            {
                month = DateTime.Now.Month;
            }

            try
            {
                // 查询本月风险信息集合
                var riskBll = new ViewRiskInfoBll();
                var monthRisks =
                    riskBll.QueryList(
                        $"DATEPART(YEAR, OccurrenceTime)={year} AND DATEPART(MONTH, OccurrenceTime)={month}",
                        new[] { "Id", "RiskSecondLevelId" }).ToList();
                if (!monthRisks.Any())
                {
                    return;
                }

                // 查询本月风险责任人
                var respondBll = new RiskResponseStaffBll();
                var riskIdList = monthRisks.Select(r => r.Id);
                var respondList = respondBll.QueryList($"RiskId IN({string.Join(",", riskIdList)})", new[] { "RiskId", "ResponseStaffId" });

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
                staffScoreBll.ExecuteTransation(
                    () =>
                    {
                        var condition = $"Year={year} AND Month={month}";
                        if (staffScoreBll.Exists(condition))
                        {
                            return staffScoreBll.Delete(condition);
                        }

                        return true;
                    },
                    () =>
                    {
                        staffScoreBll.BulkInsert(staffScoreList);
                        return true;
                    });
            }
            catch (Exception ex)
            {
                ExceptionLogBll.ExceptionPersistence(nameof(StaffScoreHelper), nameof(StaffScoreHelper), ex);
            }
        }
    }
}