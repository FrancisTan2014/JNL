using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JNL.Web.Models
{
    public enum RiskVerifyStatus
    {
        /// <summary>
        /// 待提报部门审核
        /// </summary>
        WaitingForReportDepartVerify = 1,

        /// <summary>
        /// 待风险办审核
        /// </summary>
        WaitingForSafeDepartVerify = 2,

        /// <summary>
        /// 被风险办否决，等待提报部门再一次审核
        /// </summary>
        IsVotedAndWaitingForVerify = 3,

        /// <summary>
        /// 审核已通过
        /// </summary>
        VerifyHasPassed = 4,

        /// <summary>
        /// 已填写整改处置信息
        /// </summary>
        HasReformed = 5,

        /// <summary>
        /// 已填写落实销号信息
        /// </summary>
        HasFixed = 6
    }
}