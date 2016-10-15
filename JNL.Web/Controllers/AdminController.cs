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
    }
}
