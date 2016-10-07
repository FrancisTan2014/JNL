using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JNL.Model
{
    /// <summary>
    /// 字典分类枚举
    /// </summary>
    public enum DictionaryType
    {
        /// <summary>
        /// 干部工人标识
        /// </summary>
        WorkFlag = 1,

        /// <summary>
        /// 劳动班制（如：倒班制、轮乘制等）
        /// </summary>
        Rest = 2,

        /// <summary>
        /// 工种
        /// </summary>
        WorkType = 3,

        /// <summary>
        /// 政治面貌
        /// </summary>
        PoliticalStatus = 4,

        /// <summary>
        /// 职务级别（如：正处级）
        /// </summary>
        PositionLevel = 5,

        /// <summary>
        /// 职务名称
        /// </summary>
        PositionName = 6,

        /// <summary>
        /// 事故类别
        /// </summary>
        AccidentType = 7,

        /// <summary>
        /// 列车类别（如：货车、客车等）
        /// </summary>
        LocoServiceType = 8,

        /// <summary>
        /// 天气情况
        /// </summary>
        Weather = 9,

        /// <summary>
        /// 活项归属
        /// </summary>
        LivingItems = 10,

        /// <summary>
        /// 修程
        /// </summary>
        RepairProcess = 11,

        /// <summary>
        /// 机车类型（如内燃机车等）
        /// </summary>
        LocoEngineType = 12,

        /// <summary>
        /// 机车车型（如DF8B）
        /// </summary>
        LocoModel = 13,

        /// <summary>
        /// 机车车号（如44142)
        /// </summary>
        LocoNumber = 14,
        
        /// <summary>
        /// 机务段名称
        /// </summary>
        Depots = 15,

        /// <summary>
        /// 铁路局
        /// </summary>
        RailBuearu = 16,

        /// <summary>
        /// 事故发生地点
        /// </summary>
        Place = 17,

        /// <summary>
        /// 预警来源（如：总公司预警）
        /// </summary>
        WarningSource = 18
    }
}
