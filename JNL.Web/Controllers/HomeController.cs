using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JNL.Bll;
using JNL.Utilities.Helpers;
using JNL.Web.Models;
using JNL.Web.Utils;

namespace JNL.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(LoginModel userInput)
        {
            if (ModelState.IsValid)
            {
                var staffBll = new StaffBll();

                int userId;
                var loginResult = staffBll.Login(userInput.WorkNo, userInput.Password, out userId);

                if (loginResult == LoginResult.Success)
                {
                    // 记住登录状态
                    if (userInput.Remember)
                    {
                        LoginStatus.WriteCookieFor7Days(userId);
                    }
                    else
                    {
                        LoginStatus.WriteCookieForSession(userId);
                    }

                    var loginUser = staffBll.QuerySingle(userId);
                    CookieHelper.Set(CookieNames.LoginUserName, loginUser.Name);

                    return Json(ErrorModel.LoginSuccess);
                }

                return Json(ErrorModel.LoginFailed);
            }

            return Json(ErrorModel.InputError);
        }

        public ActionResult Logout()
        {
            LoginStatus.RemoveCookie();

            return RedirectToAction("Login");
        }
    }
}
