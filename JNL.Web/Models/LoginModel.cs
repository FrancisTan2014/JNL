using System.ComponentModel.DataAnnotations;

namespace JNL.Web.Models
{
    /// <summary>
    /// 登录表单实体
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-07-14</since>
    public class LoginModel
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string WorkNo { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
        
        /// <summary>
        /// 标识是否需要记住密码
        /// </summary>
        public bool Remember { get; set; }
    }
}