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
    public class MySqlRiskSummary
    {
        public int id { get; set; }
        public int type { get; set; }
        public int level { get; set; }
        public int cat1 { get; set; }
        public int cat2 { get; set; }
        public int cat3 { get; set; }
        public int cid { get; set; }
    }

    public class Node
    {
        public int Id { get; set; }
        public RiskSummary RiskSummary { get; set; }
        public List<RiskSummary> ChildrenSummaries { get; set; } 
        public List<Node> Children { get; set; }

    }

    class Program
    {
        private static readonly string MySqlConnectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ToString();
        private static readonly IDbHelper MySqlHelper = DbHelperFactory.GetInstance(DatabaseType.MySql);

        private static readonly RiskSummaryBll RiskSummaryBll = new RiskSummaryBll();

        private static Dictionary<int, int> DicRelate = new Dictionary<int, int>();

        private static List<Station> MySqlStations;
        private static List<Station> SqlStations;
        private static Dictionary<int, int> SqlAndMySqlLineDic;

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
            // RailBureau();
            // Depots();
            // Accidents();
            // AccidentType();
            // RiskSummary();
            // NewRiskSummary();
            // Stations();
            // Lines();
            // LineStations();
            RiskInfo();
        }

        private static void RiskInfo()
        {
            if (!new RiskInfoBll().Exists())
            {
                // make staff dictionary
                var staffDic = new Dictionary<string, int>();
                var staffList = new StaffBll().QueryAll().ToList();
                staffList.ForEach(staff =>
                {
                    if (!staffDic.ContainsKey(staff.WorkId))
                    {
                        staffDic.Add(staff.WorkId, staff.Id);
                    }
                });


                // make type dictionary
                BuildDictionary();

                var riskSummaryDic = NewRiskSummary();

                var cmdText = "SELECT * FROM risk";
                var table = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var mysqlRiskList = EntityHelper.MapEntity<MySqlRisk>(table);

                var riskInfoList = mysqlRiskList.Select(risk =>
                {
                    if (!staffDic.ContainsKey(risk.resp_user_id) || !staffDic.ContainsKey(risk.report_user_id))
                    {
                        return new RiskNode();
                    }

                    var happenTime = risk.event_date_time == DateTime.MinValue
                        ? DateTime.Now
                        : risk.event_date_time;

                    var firstStationId = 0;
                    var lastStationId = 0;
                    if (risk.event_place_station > 0)
                    {
                        firstStationId = risk.event_place_station.Value;
                    }
                    else if (risk.event_place_start > 0 && risk.event_place_end > 0)
                    {
                        firstStationId = risk.event_place_start.Value;
                        lastStationId = risk.event_place_end.Value;
                    }

                    var trainType = 0;
                    if (risk.train_type > 0)
                    {
                        trainType = DicRelate[risk.train_type.Value];
                    }

                    var weather = 0;
                    if (!string.IsNullOrEmpty(risk.weather_type))
                    {
                        if (risk.weather_type.Contains(","))
                        {
                            risk.weather_type = risk.weather_type.Substring(0,
                                risk.weather_type.IndexOf(",", StringComparison.CurrentCulture));
                        }

                        weather = DicRelate[risk.weather_type.ToInt32()];
                    }

                    var riskSummaryId = 0;
                    if (risk.risk_outline_id > 0)
                    {
                        riskSummaryId = riskSummaryDic[risk.risk_outline_id.Value];
                    }

                    var verifyTime = DateTime.Now;
                    if (risk.audit_time > 1000000000)
                    {
                        verifyTime = new DateTime(1970, 1, 1).AddMilliseconds(risk.audit_time);
                    }

                    return new RiskNode
                    {
                        RiskInfo = new RiskInfo
                        {
                            ReportStaffId = staffDic[risk.report_user_id],
                            OccurrenceTime = happenTime,
                            OccurrenceLineId = GetSqlLineId(risk.event_place_line),
                            FirstStationId = firstStationId,
                            LastStationId = lastStationId,
                            LocoServiceTypeId = trainType,
                            WeatherId = weather,
                            RiskSummaryId = riskSummaryId,
                            RiskDetails = risk.risk_detail ?? string.Empty,
                            Visible = risk.risk_store == 1,
                            RiskTypeId = risk.menu_type + 2,
                            VerifyTime = verifyTime,
                            NeedRoomSign = risk.room_sign == 1,
                            NeedLeaderSign = risk.leader_sign == 1,
                            NeedStressTrack = risk.stress_trace == 1,
                            ShowInStressPage = risk.is_stress == 1,
                            NeedWriteFixDesc = risk.need_fix_text == 1,
                            RiskFix = risk.risk_fix ?? string.Empty,
                            HasDealed = risk.deal_status == 1,
                            RiskReason = risk.risk_reason ?? string.Empty
                        },
                        RiskResponseStaff = new RiskResponseStaff
                        {
                            ResponseStaffId = staffDic[risk.resp_user_id]
                        }
                    };
                }).ToList();

                var riskInfoBll = new RiskInfoBll();
                var riskRespBll = new RiskResponseStaffBll();
                riskInfoList.ForEach(risk =>
                {
                    if (risk.RiskInfo != null && risk.RiskResponseStaff != null)
                    {
                        riskInfoBll.ExecuteTransation(
                       () =>
                       {
                           risk.RiskInfo = riskInfoBll.Insert(risk.RiskInfo);
                           if (risk.RiskInfo.Id > 0)
                           {
                               risk.RiskResponseStaff.RiskId = risk.RiskInfo.Id;
                               risk.RiskResponseStaff = riskRespBll.Insert(risk.RiskResponseStaff);
                               if (risk.RiskResponseStaff.Id > 0)
                               {
                                   return true;
                               }
                           }

                           return false;
                       }
                   );
                    }
                });

                Console.WriteLine("风险信息数据导入成功");
                Console.ReadKey();
            }
        }

        private static void LineStations()
        {
            var linestationBll = new LineStationsBll();

            if (!linestationBll.Exists())
            {
                

                var lineStations = GetListFromMySql<LineStations>("SELECT station_id AS StationId, line_id AS LineId, POSITION AS Sort FROM line_stop");
                lineStations.ForEach(item =>
                {
                    item.LineId = GetSqlLineId(item.LineId);
                    item.StationId = GetSqlStationId(item.StationId);
                });

                linestationBll.BulkInsert(lineStations);

                Console.WriteLine("车站线路对应关系导入成功");
                Console.ReadKey();
            }
        }

        private static int GetSqlLineId(int mysqlLineId)
        {
            if (SqlAndMySqlLineDic == null)
            {
                var mysqlLines = GetListFromMySql<Line>("SELECT Id,Name FROM dictionary WHERE TYPE=9");
                var sqlLines = new LineBll().QueryAll();
                SqlAndMySqlLineDic =
                    mysqlLines.Join(sqlLines, inner => inner.Name, outer => outer.Name,
                        (inner, outer) => new KeyValuePair<int, int>(inner.Id, outer.Id))
                        .ToDictionary(pair => pair.Key, pair => pair.Value);
            }

            if (SqlAndMySqlLineDic.ContainsKey(mysqlLineId))
            {
                return SqlAndMySqlLineDic[mysqlLineId];
            }

            return 0;
        }

        private static int GetSqlStationId(int mysqlstationId)
        {
            if (MySqlStations == null)
            {
                MySqlStations = GetListFromMySql<Station>("SELECT Id,Name FROM dictionary WHERE TYPE=13");
                SqlStations = new StationBll().QueryAll().ToList();
            }

            var stationName = MySqlStations.Find(s => s.Id == mysqlstationId).Name;
            return SqlStations.Find(s => s.Name == stationName).Id;
        }

        private static void Lines()
        {
            var lineBll = new LineBll();
            if (!lineBll.Exists())
            {
                var cmdText = "SELECT Name FROM dictionary WHERE TYPE=9";
                var lines = GetListFromMySql<Line>(cmdText);

                lineBll.BulkInsert(lines);

                Console.WriteLine("线路数据导入成功");
                Console.ReadKey();
            }
        }

        private static void Stations()
        {
            var stationBll = new StationBll();
            if (!stationBll.Exists())
            {
                var cmdText = "SELECT Name FROM dictionary WHERE TYPE=13";
                var stations = GetListFromMySql<Station>(cmdText);

                stationBll.BulkInsert(stations);

                Console.WriteLine("车站数据导入成功");
                Console.ReadKey();
            }
        }

        private static List<T> GetListFromMySql<T>(string cmdText) where T: class, new()
        {
            var table = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
            return EntityHelper.MapEntity<T>(table).ToList();
        } 

        private static Dictionary<int, int> NewRiskSummary()
        {
            var mysqlRiskSummaryIdDic = new Dictionary<int, int>();

            if (!RiskSummaryBll.Exists())
            {
                #region 创建字典
                var dicCmdText = "SELECT * FROM dictionary WHERE type IN(12, 20, 21, 22, 23, 24)";
                DataTable table = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, dicCmdText);
                var nameDic = new Dictionary<int, string>();
                foreach (DataRow row in table.Rows)
                {
                    var id = row["id"].ToString().ToInt32();
                    var name = row["name"].ToString();
                    nameDic.Add(id, name);
                }
                #endregion

                var cmdText = "SELECT * FROM risk_summary";
                var dataTable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
                var summaryList = EntityHelper.MapEntity<MySqlRiskSummary>(dataTable).ToList();

                var insertedDic = new Dictionary<int, RiskSummary>();
                summaryList.ForEach(summary =>
                {
                    RiskSummary firstLevel;
                    if (!insertedDic.ContainsKey(summary.type))
                    {
                        firstLevel = new RiskSummary {Description = nameDic[summary.type]};
                        firstLevel = RiskSummaryBll.Insert(firstLevel);
                        if (firstLevel.Id > 0)
                        {
                            insertedDic.Add(summary.type, firstLevel);
                            mysqlRiskSummaryIdDic.Add(summary.type, firstLevel.Id);
                        }
                        else
                            return;
                    }
                    else
                    {
                        firstLevel = insertedDic[summary.type];
                    }

                    RiskSummary secondLevel;
                    if (!insertedDic.ContainsKey(summary.level))
                    {
                        secondLevel = new RiskSummary
                        {
                            Description = nameDic[summary.level],
                            ParentId = firstLevel.Id,
                            TopestTypeId = firstLevel.Id
                        };
                        secondLevel = RiskSummaryBll.Insert(secondLevel);
                        if (secondLevel.Id > 0)
                        {
                            insertedDic.Add(summary.level, secondLevel);
                            mysqlRiskSummaryIdDic.Add(summary.level, secondLevel.Id);
                        }
                            
                        else
                            return;
                    }
                    else
                    {
                        secondLevel = insertedDic[summary.level];
                    }

                    RiskSummary thirdLevel;
                    if (!insertedDic.ContainsKey(summary.cat1))
                    {
                        thirdLevel = new RiskSummary
                        {
                            Description = nameDic[summary.cat1],
                            ParentId = secondLevel.ParentId,
                            TopestTypeId = firstLevel.Id
                        };
                        thirdLevel = RiskSummaryBll.Insert(thirdLevel);
                        if (thirdLevel.Id > 0)
                        {
                            insertedDic.Add(summary.cat1, thirdLevel);
                            mysqlRiskSummaryIdDic.Add(summary.cat1, thirdLevel.Id);
                        }
                            
                    }
                    else
                    {
                        thirdLevel = insertedDic[summary.cat1];
                    }

                    RiskSummary fourthLevel;
                    if (!insertedDic.ContainsKey(summary.cat2))
                    {
                        fourthLevel = new RiskSummary
                        {
                            Description = nameDic[summary.cat2],
                            ParentId = thirdLevel.Id,
                            TopestTypeId = firstLevel.Id
                        };
                        fourthLevel = RiskSummaryBll.Insert(fourthLevel);
                        if (fourthLevel.Id > 0)
                        {
                            insertedDic.Add(summary.cat2, fourthLevel);
                            mysqlRiskSummaryIdDic.Add(summary.cat2, fourthLevel.Id);
                        }
                    }
                    else
                    {
                        fourthLevel = insertedDic[summary.cat2];
                    }

                    if (summary.cat3 == 0)
                    {
                        if (!insertedDic.ContainsKey(summary.cid))
                        {
                            RiskSummary bottom = new RiskSummary
                            {
                                Description = nameDic[summary.cid],
                                ParentId = fourthLevel.Id,
                                TopestTypeId = firstLevel.Id,
                                IsBottom = true
                            };
                            bottom = RiskSummaryBll.Insert(bottom);
                            if (bottom.Id > 0)
                            {
                                insertedDic.Add(summary.cid, bottom);
                                mysqlRiskSummaryIdDic.Add(summary.cid, bottom.Id);
                            }
                        }
                    }
                    else
                    {
                        RiskSummary fifthLevel;
                        if (!insertedDic.ContainsKey(summary.cat3))
                        {
                            fifthLevel = new RiskSummary { Description = nameDic[summary.cat3], ParentId = fourthLevel.Id, TopestTypeId = firstLevel.Id };
                            fifthLevel = RiskSummaryBll.Insert(fifthLevel);
                            if (fifthLevel.Id > 0)
                            {
                                insertedDic.Add(summary.cat3, fifthLevel);
                                mysqlRiskSummaryIdDic.Add(summary.cat3, fifthLevel.Id);
                            }
                            else
                                return;
                        }
                        else
                        {   
                            fifthLevel = insertedDic[summary.cat3];
                        }
                        
                        if (!insertedDic.ContainsKey(summary.cid))
                        {
                            var realBottom = new RiskSummary { Description = nameDic[summary.cid], ParentId = fifthLevel.Id, TopestTypeId = firstLevel.Id, IsBottom = true };
                            realBottom = RiskSummaryBll.Insert(realBottom);
                            if (realBottom.Id > 0)
                            {

                                insertedDic.Add(summary.cid, realBottom);
                                mysqlRiskSummaryIdDic.Add(summary.cid, realBottom.Id);
                            }
                        }
                    }
                });

                Console.WriteLine("风险概述数据导入成功");
            }

            return mysqlRiskSummaryIdDic;
        }

        /// <summary>
        /// 风险概述信息（代码略复杂）
        /// </summary>
        private static void RiskSummary()
        {
            #region 创建字典
            var dicCmdText = "SELECT * FROM dictionary WHERE type IN(12, 20, 21, 22, 23, 24)";
            DataTable table = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, dicCmdText);
            var nameDic = new Dictionary<int, string>();
            foreach (DataRow row in table.Rows)
            {
                var id = row["id"].ToString().ToInt32();
                var name = row["name"].ToString();
                nameDic.Add(id, name);
            } 
            #endregion

            var cmdText = "SELECT * FROM risk_summary";
            var dataTable = MySqlHelper.ExecuteDataTable(MySqlConnectionString, CommandType.Text, cmdText);
            var summaryList = EntityHelper.MapEntity<MySqlRiskSummary>(dataTable).ToList();

            var rootNode = new Node
            {
                Children = summaryList.Select(s => s.type).Distinct().Select(s => new Node
                {
                    RiskSummary = new RiskSummary { Description = nameDic[s] },
                    Id = s
                }).ToList()
            };

            rootNode.Children.ForEach(rootLevel =>
            {
                
                rootLevel.Children =
                    summaryList.Where(s => s.type == rootLevel.Id)
                        .Select(s => s.level)
                        .Distinct()
                        .Select(s => new Node {Id = s, RiskSummary = new RiskSummary { Description = nameDic[s] } })
                        .ToList();

                rootLevel.Children.ForEach(firstLevel =>
                {
                    firstLevel.Children =
                        summaryList.Where(m => m.type == rootLevel.Id && m.level == firstLevel.Id)
                            .Select(m => m.cat1)
                            .Distinct()
                            .Select(m => new Node {Id = m, RiskSummary = new RiskSummary { Description = nameDic[m]}})
                            .ToList();

                    firstLevel.Children.ForEach(secondLevel =>
                    {
                        secondLevel.Children =
                            summaryList.Where(m => m.type == rootLevel.Id && m.level == firstLevel.Id && m.cat1 == secondLevel.Id)
                                .Select(m => m.cat2)
                                .Distinct()
                                .Select(m => new Node {Id = m, RiskSummary = new RiskSummary { Description = nameDic[m]}})
                                .ToList();

                        secondLevel.Children.ForEach(thirdLevel =>
                        {
                            var subList =
                                summaryList.Where(
                                    m =>
                                        m.type == rootLevel.Id && m.level == firstLevel.Id && m.cat1 == secondLevel.Id &&
                                        m.cat2 == thirdLevel.Id).ToList();
                            
                            subList.ForEach(sub =>
                            {
                                if (sub.cat3 == 0)
                                {
                                    thirdLevel.ChildrenSummaries =
                                        subList.Where(m => m.cat2 == thirdLevel.Id)
                                            .Select(m => new RiskSummary {IsBottom = true, Description = nameDic[m.cid]}).ToList();
                                }
                                else
                                {
                                    thirdLevel.Children =
                                        subList.Where( m => m.cat3 == sub.cat3 )
                                            .Select(m => m.cat3)
                                            .Distinct()
                                            .Select(
                                                m =>
                                                    new Node
                                                    {
                                                        Id = m,
                                                        RiskSummary = new RiskSummary {Description = nameDic[m]}
                                                    })
                                            .ToList();


                                    thirdLevel.Children.ForEach(forthLevel =>
                                    {
                                        forthLevel.ChildrenSummaries = subList.Where(m => m.cid == rootLevel.Id).Select(m=>new RiskSummary {IsBottom = true, Description = nameDic[m.cid]}).ToList();
                                    });
                                }
                            });
                        });
                    });
                });
            });

            if (!RiskSummaryBll.Exists())
            {
                rootNode.Children.ForEach(node =>
                {
                    InsertRiskSummaryToDb(node, 0, 0);
                });

                Console.WriteLine("风险概述数据导入成功");
            }
        }

        private static void InsertRiskSummaryToDb(Node node, int parentId, int topestId)
        {
            if (node.RiskSummary != null)
            {
                node.RiskSummary.TopestTypeId = topestId;
                node.RiskSummary.ParentId = parentId;
                node.RiskSummary = RiskSummaryBll.Insert(node.RiskSummary);
                if (node.RiskSummary.Id > 0)
                {
                    if (topestId == 0)
                    {
                        topestId = node.RiskSummary.Id;
                    }

                    node.Children?.ForEach(subNode =>
                    {
                        InsertRiskSummaryToDb(subNode, node.RiskSummary.Id, topestId);
                    });

                    node.ChildrenSummaries?.ForEach(summary =>
                    {
                        summary.ParentId = node.RiskSummary.Id;
                        summary.TopestTypeId = topestId;
                    });

                    if (node.ChildrenSummaries != null)
                    {
                        RiskSummaryBll.BulkInsert(node.ChildrenSummaries);
                    }
                }
            }
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
