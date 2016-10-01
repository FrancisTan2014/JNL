using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JNL.Web.Models
{
    /// <summary>
    /// 公共接口GetList的参数模型
    /// </summary>
    public class GetListParams
    {
        [Required]
        public string TableName { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderField { get; set; }
        public bool Desending { get; set; }
        public string Conditions { get; set; }
        public string Fields { get; set; }
    }
}