using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using JNL.Bll;
using JNL.DbProvider;
using JNL.Model;
using JNL.Utilities.Extensions;

namespace JNL.DataMigration
{

    class Program
    {
        private static readonly string MySqlConnectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ToString();
        private static readonly IDbHelper MySqlHelper = DbHelperFactory.GetInstance(DatabaseType.MySql);

        private static Dictionary<int, int> DicRelate = new Dictionary<int, int>();

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
            // Department();
            // BuildDictionary();
            // Staves();
            RailBureau();
            // Depots();
            // Accidents();
            // AccidentType();
        }

        private static void AccidentType()
        {
            var dicBll = new DictionariesBll();
            if (!dicBll.Exists("Type=7"))
            {
                var cmdText = "SELECT DISTINCT `Type` AS Name FROM basic WHERE `Type`<>''";
                var table = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var list = EntityHelper.MapEntity<Dictionaries>(table).ToList();

                list.ForEach(elem =>
                {
                    elem.Type = 7;
                });

                dicBll.BulkInsert(list);

                Console.WriteLine("事故类别插入成功");
                Console.ReadKey();
            }
        }

        private static void Accidents()
        {
            var accidentBll = new AccidentBll();
            if (!accidentBll.Exists())
            {
                var cmdText = @"SELECT STARTTIME AS OccurrenceTime, address AS Place, company AS ResponseBureau, blance AS ResponseDepot, TYPE AS AccidentType, cartype AS LocoType, weather AS WeatherLike, world AS Keywords, `desc` AS Summary, measures AS `Help`, responsibility AS Responsibility, lesson AS Lesson, reason AS Reason FROM basic";
                var table = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var list = EntityHelper.MapEntity<Accident>(table).ToList();

                list.ForEach(e =>
                {
                    if (e.OccurrenceTime == DateTime.MinValue)
                    {
                        e.OccurrenceTime = DateTime.Now;
                    }
                });

                accidentBll.BulkInsert(list);

                Console.WriteLine("典型事故导入成功");
            }
        }

        /// <summary>
        /// 铁路局
        /// </summary>
        private static void RailBureau()
        {
            var bureausBll = new DictionariesBll();

            if (!bureausBll.Exists("Type=16"))
            {
                var cmdText = "SELECT DISTINCT address AS `Name` FROM basic WHERE address<>''";
                var dataTable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var bureaus = EntityHelper.MapEntity<Dictionaries>(dataTable).ToList();
                foreach (var bureau in bureaus)
                {
                    bureau.Type = 16;
                }

                bureausBll.BulkInsert(bureaus);

                Console.WriteLine("铁路局数据插入成功");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// 机务段
        /// </summary>
        private static void Depots()
        {
            var depotsBll = new DictionariesBll();

            if (!depotsBll.Exists("Type=15"))
            {
                var cmdText = "SELECT DISTINCT company AS Name FROM basic WHERE company<>''";
                var dataTable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var depots = EntityHelper.MapEntity<Dictionaries>(dataTable).ToList();
                foreach (var depot in depots)
                {
                    depot.Type = 15;
                }

                depotsBll.BulkInsert(depots);
            }
        }

        /// <summary>
        /// 构造mysql中的字典表与sqlserver中字典表的对应关系
        /// </summary>
        private static void BuildDictionary()
        {
            var typeDic = new Dictionary<int, int> { { 1, 1 }, { 2, 2 }, { 3, 3 }, { 4, 4 }, { 5, 5 }, { 6, 6 }, { 14, 10 }, { 11, 9 }, { 10, 8 } };

            var cmdText = "SELECT Id,Name,Type FROM dictionary";
            var dataTable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
            var mySqlDic = EntityHelper.MapEntity<Dictionaries>(dataTable).ToList();

            var mysqlNameDic = mySqlDic.ToDictionary(dictionariese => dictionariese.Id, dictionariese => dictionariese.Name);

            var sqlDic = new DictionariesBll().QueryAll().ToList();
            var sqlNameDic = sqlDic.ToDictionary(dic => dic.Id, dic => dic.Name);

            foreach (var typePair in typeDic)
            {
                var mlist = mySqlDic.Where(d => d.Type == typePair.Key);
                var slist = sqlDic.Where(d => d.Type == typePair.Value);
                var dic =
                    mlist.Join(slist, d => d.Name, d => d.Name,
                        (inner, outer) => new KeyValuePair<int, int>(inner.Id, outer.Id))
                        .ToDictionary(k => k.Key, k => k.Value);
                foreach (var keyPair in dic)
                {
                    DicRelate.Add(keyPair.Key, keyPair.Value);
                }
            }
        }

        /// <summary>
        /// 员工数据
        /// </summary>
        private static void Staves()
        {
            var cmdText = "SELECT Id,Name,Type FROM dictionary WHERE type=7";
            var table = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
            var departList = EntityHelper.MapEntity<Department>(table);

            var departs = new DepartmentBll().QueryAll();
            var departDic = departList.Join(departs, depart => depart.Name, depart => depart.Name,
                (inner, outer) => new KeyValuePair<int, int>(inner.Id, outer.Id)).ToDictionary(key => key.Key, key => key.Value);

            var staffBll = new StaffBll();
            if (!staffBll.Exists())
            {
                var staffCmdText = @"SELECT work_id AS WorkId, salary_id AS SalaryId, NAME, 
                                    (CASE sex WHEN 1 THEN '男' WHEN 2 THEN '女' END) AS Gender,
                                    hireday AS HireDate, birthday AS BirthDate, 
                                    work_flag AS WorkFlagId, work_type AS WorkTypeId,
                                    political_status AS PoliticalStatusId,position_level AS PositionId,
                                    depart_id AS DepartmentId
                                    FROM STAFF";
                var datatable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, staffCmdText);
                var staves = EntityHelper.MapEntity<Staff>(datatable).ToList();
                foreach (var staff in staves)
                {
                    if (staff.HireDate == DateTime.MinValue)
                    {
                        staff.HireDate = DateTime.Now;
                    }
                    if (staff.BirthDate == DateTime.MinValue)
                    {
                        staff.BirthDate = new DateTime(1970, 1, 1);
                    }

                    staff.AddTime = DateTime.Now;
                    staff.Password = staff.SalaryId.GetMd5();

                    try
                    {
                        staff.WorkFlagId = DicRelate[staff.WorkFlagId];
                    }
                    catch (Exception)
                    {
                        //
                    }

                    try
                    {

                        staff.WorkTypeId = DicRelate[staff.WorkTypeId];
                    }
                    catch (Exception)
                    {
                        // 
                    }

                    try
                    {

                        staff.PoliticalStatusId = DicRelate[staff.PoliticalStatusId];

                    }
                    catch (Exception)
                    {
                        // 
                    }

                    try
                    {
                        staff.DepartmentId = departDic[staff.DepartmentId];

                    }
                    catch (Exception)
                    {
                        //
                    }

                    try
                    {
                        staff.PositionId = DicRelate[staff.PositionId];
                    }
                    catch (Exception)
                    {
                        //
                    }
                }

                staffBll.BulkInsert(staves);

                Console.WriteLine("员工数据迁移成功");
            }
        }

        private static void Department()
        {
            var departBll = new DepartmentBll();
            if (!departBll.Exists())
            {
                var cmdText = "SELECT Name FROM dictionary WHERE type=7";
                var table = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var departList = EntityHelper.MapEntity<Department>(table);

                departBll.BulkInsert(departList);
                Console.WriteLine("部门数迁移成功");
            }
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
