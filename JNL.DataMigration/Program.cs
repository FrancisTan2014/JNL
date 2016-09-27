using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using JNL.Bll;
using JNL.DbProvider;
using JNL.Model;

namespace JNL.DataMigration
{
    class Program
    {
        private static readonly string MySqlConnectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ToString();
        private static readonly IDbHelper MySqlHelper = DbHelperFactory.GetInstance(DatabaseType.MySql);

        static void Main(string[] args)
        {
            // BasicDownload();
            // WorkFlag();
            // Rest();
            // WorkType();
            // PoliticalStatus();
            // PositionLevel();
            // PositionName();
            // Dictionaries(10, "列车类别", 8);
            // Dictionaries(11, "天气情况", 9);
            // Dictionaries(14, "活项归属", 10);
            // Dictionaries(15, "修程", 11);
            // Dictionaries(16, "机车类型", 12);
            // Locomotive();
        }

        /// <summary>
        /// 机车型号数据迁移
        /// </summary>
        private static void Locomotive()
        {
            var engineList = new Dictionary<int, KeyValuePair<int, string>>
            {
                { 638, new KeyValuePair<int, string>(310, "电力机车") },
                { 639, new KeyValuePair<int, string>(311, "内燃机车") }
            };

            var dicBll = new DictionariesBll();
            foreach (var keyValuePair in engineList)
            {
                var sourceEnginType = keyValuePair.Key;
                var targetEnginType = keyValuePair.Value.Key;
                var cmdText = $"SELECT DISTINCT Model AS Name FROM LOCOMOTIVE  WHERE Type={sourceEnginType}";

                var modelTable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var modelList = EntityHelper.MapEntity<Dictionaries>(modelTable).ToList();
                modelList.ForEach(m =>
                {
                    m.ParentId = targetEnginType;
                    m.Type = 13;
                });

                foreach (var model in modelList)
                {
                    dicBll.Insert(model);
                    if (model.Id > 0)
                    {
                        var numberCmdText = $"SELECT DISTINCT Number AS Name FROM LOCOMOTIVE WHERE Model='{model.Name}'";
                        var numberTable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text,
                            numberCmdText);
                        var numberList = EntityHelper.MapEntity<Dictionaries>(numberTable).ToList();

                        numberList.ForEach(n =>
                        {
                            n.ParentId = model.Id;
                            n.Type = 14;
                        });
                        dicBll.BulkInsert(numberList);
                    }
                }
            }

            Console.WriteLine("机车型号迁移成功");
            Console.ReadKey();
        }

        /// <summary>
        /// 字典表的数据迁移
        /// </summary>
        private static void Dictionaries(int sourceType, string desc, int targetType)
        {
            var bll = new DictionariesBll();
            if (!bll.Exists($"Type={sourceType}"))
            {
                var cmdText =
                    $"SELECT Type,Name FROM dictionary WHERE Type={sourceType}";
                var dataTable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var list = EntityHelper.MapEntity<Dictionaries>(dataTable).ToList();

                list.ForEach(item => item.Type = targetType);

                bll.BulkInsert(list);

                Console.WriteLine($"dictionary表的\"{desc}\"的数据迁移成功");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 职务名称
        /// </summary>
        private static void PositionName()
        {
            var type = 6;
            var bll = new DictionariesBll();
            if (!bll.Exists($"Type={type}"))
            {
                var cmdText =
                    $"SELECT Type,Name FROM dictionary WHERE Type={type}";
                var dataTable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var list = EntityHelper.MapEntity<Dictionaries>(dataTable);

                bll.BulkInsert(list);

                Console.WriteLine("dictionary表的\"职务名称\"的数据迁移成功");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 职务级别
        /// </summary>
        private static void PositionLevel()
        {
            var bll = new DictionariesBll();
            if (!bll.Exists("Type=5"))
            {
                var cmdText =
                    "SELECT Type,Name FROM dictionary WHERE Type=5";
                var dataTable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var list = EntityHelper.MapEntity<Dictionaries>(dataTable);

                bll.BulkInsert(list);

                Console.WriteLine("dictionary表的\"职务级别\"的数据迁移成功");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 政治面貌
        /// </summary>
        private static void PoliticalStatus()
        {
            var bll = new DictionariesBll();
            if (!bll.Exists("Type=4"))
            {
                var cmdText =
                    "SELECT Type,Name FROM dictionary WHERE Type=4";
                var dataTable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var list = EntityHelper.MapEntity<Dictionaries>(dataTable);

                bll.BulkInsert(list);

                Console.WriteLine("dictionary表的\"政治面貌\"的数据迁移成功");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 工种
        /// </summary>
        private static void WorkType()
        {
            var bll = new DictionariesBll();
            if (!bll.Exists("Type=3"))
            {
                var cmdText =
                    "SELECT Type,Name FROM dictionary WHERE Type=3";
                var dataTable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var list = EntityHelper.MapEntity<Dictionaries>(dataTable);

                bll.BulkInsert(list);

                Console.WriteLine("dictionary表的\"工种\"的数据迁移成功");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 基础文件
        /// </summary>
        private static void BasicDownload()
        {
            var basicBll = new BasicFileBll();
            if (!basicBll.Exists(string.Empty))
            {
                var cmdText =
                    "SELECT NAME AS FileName,NUMBER AS FileNumber,URL AS FilePath,TYPE AS FileType,(CASE LEVEL WHEN '总公司' THEN 1 WHEN '铁路局' THEN 2 WHEN '机务段' THEN 3 END) AS PublishLevel FROM basic_download";
                var dataTable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var basicFileList = EntityHelper.MapEntity<BasicFile>(dataTable);

                basicBll.BulkInsert(basicFileList);

                Console.WriteLine("basic_download表的数据迁移成功");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 干部工人标识
        /// </summary>
        private static void WorkFlag()
        {
            var bll = new DictionariesBll();
            if (!bll.Exists("Type=1"))
            {
                var cmdText =
                    "SELECT Type,Name FROM dictionary WHERE Type=1";
                var dataTable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var list = EntityHelper.MapEntity<Dictionaries>(dataTable);

                bll.BulkInsert(list);

                Console.WriteLine("dictionary表的\"干部工人标识迁移成功\"的数据迁移成功");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 劳动班制
        /// </summary>
        private static void Rest()
        {
            var bll = new DictionariesBll();
            if (!bll.Exists("Type=2"))
            {
                var cmdText =
                    "SELECT Type,Name FROM dictionary WHERE Type=2";
                var dataTable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var list = EntityHelper.MapEntity<Dictionaries>(dataTable);

                bll.BulkInsert(list);

                Console.WriteLine("dictionary表的\"劳动班制\"的数据迁移成功");
                Console.ReadLine();
            }
        }
    }
}
