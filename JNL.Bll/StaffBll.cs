using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JNL.Utilities.Extensions;

namespace JNL.Bll
{
    public partial class StaffBll
    {
        public LoginResult Login(string workNo, string password, out int userId)
        {
            var user = DalInstance.QuerySingle(
                "WorkId=@workId AND IsDelete=0",
                null,
                new Dictionary<string, object> { { "@workId", workNo } });

            userId = 0;
            if (user == null)
            {
                return LoginResult.NotExists;
            }

            userId = user.Id;

            var encrytPwd = password.GetMd5();
            if (encrytPwd == user.Password)
            {
                return LoginResult.Success;
            }

            return LoginResult.PasswordError;
        }
    }
}
