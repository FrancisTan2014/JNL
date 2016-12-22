using System;
using System.Collections.Generic;
using JNL.Utilities.Extensions;
using System.Configuration;
using System.Linq;
using JNL.Bll;
using WebGrease.Css.Extensions;

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
                case 2:
                    return ImagesPath;
                case 3:
                    return ExamFilesPath;
                case 4:
                    return TraceFilesUploadPath;
                case 5:
                    return ArticleFiles;
                default:
                    return string.Empty;
            }
        }

        public static string ArticleFiles => GetConfig("ArticleFiles");

        public static string TraceFilesUploadPath => GetConfig("TraceFiles");

        /// <summary>
        /// 获取配置的需要填写整改落实信息的风险等级
        /// </summary>
        public static string NeedReformRiskLevels => GetConfig("NeedReformRiskLevels");

        /// <summary>
        /// 获取安全科部门Id
        /// </summary>
        public static int SafeDepartId => GetConfig("SafeDepartId").ToInt32();

        /// <summary>
        /// 图片上传路径
        /// </summary>
        public static string ImagesPath => GetConfig("Images");

        public static string ExamFilesPath => GetConfig("Exam");

        /// <summary>
        /// 获取风险信息概述Id与其对应的所扣分值的字典集
        /// </summary>
        public static Dictionary<int, int> RiskMinusScoreDic
        {
            get
            {
                var config = GetConfig("RiskMinusScore");
                try
                {
                    var couples = config.Split(',');
                    var dic = new Dictionary<int, int>();
                    couples.ForEach(s =>
                    {
                        var temp = s.Split('-');
                        dic.Add(temp[0].ToInt32(), temp[1].ToInt32());
                    });

                    return dic;
                }
                catch (Exception ex)
                {
                    ExceptionLogBll.ExceptionPersistence(nameof(AppSettings), nameof(AppSettings), ex);

                    throw ex;
                }
            }
        }

        /// <summary>
        /// 研判预警/数据分析预警/构成分析中所需要分析的风险信息Id
        /// </summary>
        public static int[] ConstituteAnalysisRiskSummaryIds
        {
            get
            {
                var config = GetConfig("ConstituteAnalysisRiskSummaryIds");
                if (!string.IsNullOrEmpty(config))
                {
                    return config.Split(',').Select(s => s.ToInt32()).ToArray();
                }

                return new int[1];
            }
        }

        /// <summary>
        /// 研判预警/数据分析预警/阶段分析中所需要分析的风险信息Id
        /// </summary>
        public static int[] StageAnalysisRiskSummaryIds
        {
            get
            {
                var config = GetConfig("StageAnalysisRiskSummaryIds");
                if (!string.IsNullOrEmpty(config))
                {
                    return config.Split(',').Select(s => s.ToInt32()).ToArray();
                }

                return new int[1];
            }
        }
    }
}
