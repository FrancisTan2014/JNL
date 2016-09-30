using JNL.Utilities.Extensions;
using System.Configuration;

namespace JNL.Web.Utils
{
    /// <summary>
    /// 提供对appSettings中的配置项统一访问的静态属性
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-07-28</since>
    public static class AppSettings
    {
        public static readonly string BasicFiles = ConfigurationManager.AppSettings["BasicFiles"];

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
    }
}
