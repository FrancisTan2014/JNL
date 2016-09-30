using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JNL.Bll
{
    /// <summary>
    /// 描述登录结果的枚举
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-07-14</since>
    public enum LoginResult
    {
        /// <summary>
        /// 登录成功
        /// </summary>
        Success,

        /// <summary>
        /// 用户不存在
        /// </summary>
        NotExists,

        /// <summary>
        /// 密码错误
        /// </summary>
        PasswordError
    }
}
