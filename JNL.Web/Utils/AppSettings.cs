using System;
using System.Collections.Generic;
using JNL.Utilities.Extensions;
using System.Configuration;
using System.Linq;

namespace JNL.Web.Utils
{
    /// <summary>
    /// 提供对appSettings中的配置项统一访问的静态属性
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-07-28</since>
    public static class AppSettings
    {
        /// <summary>
        /// 获取指定键的AppSettings项的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfig(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? string.Empty;
        }

        public static readonly string BasicFiles = GetConfig("BasicFiles");

        /// <summary>
        /// 根据要保存的文件类型，返回对应的文件保存路径配置信息
        /// </summary>
        /// <param name="fileType">文件类型</param>
        /// <returns></returns>
        public static string GetFileSavePath(int fileType)
        {
            switch (fileType)
            {
                case 1:
                    return BasicFiles;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 获取配置的需要填写整改落实信息的风险等级
        /// </summary>
        public static string NeedReformRiskLevels => GetConfig("NeedReformRiskLevels");

        /// <summary>
        /// 获取安全科部门Id
        /// </summary>
        public static int SafeDepartId => GetConfig("SafeDepartId").ToInt32();
    }
}
