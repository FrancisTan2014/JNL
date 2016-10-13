/**  版本信息模板在安装目录下，可自行修改。
* ViewRiskRespondRisk.cs
*
* 功 能： N/A
* 类 名： ViewRiskRespondRisk
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/10/12 18:02:39   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace JNL.Model
{
	/// <summary>
	/// ViewRiskRespondRisk:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ViewRiskRespondRisk
	{
		public ViewRiskRespondRisk()
		{}
		#region Model
		private int _id;
		private int _riskid;
		private int _responsestaffid;
		private DateTime _addtime;
		private bool _isdelete;
		private string _name;
		private int? _departmentid;
		private string _department;
		private string _salaryid;
		private string _workid;
		private int? _reportstaffid;
		private int? _occurrencelineid;
		private int? _firststationid;
		private int? _laststationid;
		private int? _locoservicetypeid;
		private int? _weatherid;
		private DateTime? _occurrencetime;
		private int? _risksummaryid;
		private string _riskdetails;
		private string _riskreason;
		private string _riskfix;
		private bool _visible;
		private int? _risktypeid;
		private int? _verifystaffid;
		private int? _verifystatus;
		private DateTime? _verifytime;
		private bool _showinstresspage;
		private DateTime? _dealtimelimit;
		private bool _hasdealed;
		private bool _needroomsign;
		private bool _needleadersign;
		private bool _needwritefixdesc;
		private bool _needstresstrack;
		private string _reportstaffname;
		private int? _reportstaffdepartid;
		private string _reportstaffdepart;
		private string _reportstaffworkid;
		private string _reportstaffsalaryid;
		private string _occurrencelinename;
		private string _firststationname;
		private string _laststationname;
		private string _locoservicetype;
		private string _weatherlike;
		private string _risksummary;
		private int? _risktopestlevelid;
		private string _risktopestname;
		private int? _risksecondlevelid;
		private string _risksecondlevelname;
		private string _risktype;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int RiskId
		{
			set{ _riskid=value;}
			get{return _riskid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ResponseStaffId
		{
			set{ _responsestaffid=value;}
			get{return _responsestaffid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool IsDelete
		{
			set{ _isdelete=value;}
			get{return _isdelete;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? DepartmentId
		{
			set{ _departmentid=value;}
			get{return _departmentid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Department
		{
			set{ _department=value;}
			get{return _department;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SalaryId
		{
			set{ _salaryid=value;}
			get{return _salaryid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WorkId
		{
			set{ _workid=value;}
			get{return _workid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ReportStaffId
		{
			set{ _reportstaffid=value;}
			get{return _reportstaffid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OccurrenceLineId
		{
			set{ _occurrencelineid=value;}
			get{return _occurrencelineid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? FirstStationId
		{
			set{ _firststationid=value;}
			get{return _firststationid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LastStationId
		{
			set{ _laststationid=value;}
			get{return _laststationid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LocoServiceTypeId
		{
			set{ _locoservicetypeid=value;}
			get{return _locoservicetypeid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? WeatherId
		{
			set{ _weatherid=value;}
			get{return _weatherid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? OccurrenceTime
		{
			set{ _occurrencetime=value;}
			get{return _occurrencetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RiskSummaryId
		{
			set{ _risksummaryid=value;}
			get{return _risksummaryid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RiskDetails
		{
			set{ _riskdetails=value;}
			get{return _riskdetails;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RiskReason
		{
			set{ _riskreason=value;}
			get{return _riskreason;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RiskFix
		{
			set{ _riskfix=value;}
			get{return _riskfix;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Visible
		{
			set{ _visible=value;}
			get{return _visible;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RiskTypeId
		{
			set{ _risktypeid=value;}
			get{return _risktypeid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? VerifyStaffId
		{
			set{ _verifystaffid=value;}
			get{return _verifystaffid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? VerifyStatus
		{
			set{ _verifystatus=value;}
			get{return _verifystatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? VerifyTime
		{
			set{ _verifytime=value;}
			get{return _verifytime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool ShowInStressPage
		{
			set{ _showinstresspage=value;}
			get{return _showinstresspage;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? DealTimeLimit
		{
			set{ _dealtimelimit=value;}
			get{return _dealtimelimit;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool HasDealed
		{
			set{ _hasdealed=value;}
			get{return _hasdealed;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool NeedRoomSign
		{
			set{ _needroomsign=value;}
			get{return _needroomsign;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool NeedLeaderSign
		{
			set{ _needleadersign=value;}
			get{return _needleadersign;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool NeedWriteFixDesc
		{
			set{ _needwritefixdesc=value;}
			get{return _needwritefixdesc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool NeedStressTrack
		{
			set{ _needstresstrack=value;}
			get{return _needstresstrack;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReportStaffName
		{
			set{ _reportstaffname=value;}
			get{return _reportstaffname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ReportStaffDepartId
		{
			set{ _reportstaffdepartid=value;}
			get{return _reportstaffdepartid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReportStaffDepart
		{
			set{ _reportstaffdepart=value;}
			get{return _reportstaffdepart;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReportStaffWorkId
		{
			set{ _reportstaffworkid=value;}
			get{return _reportstaffworkid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReportStaffSalaryId
		{
			set{ _reportstaffsalaryid=value;}
			get{return _reportstaffsalaryid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OccurrenceLineName
		{
			set{ _occurrencelinename=value;}
			get{return _occurrencelinename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FirstStationName
		{
			set{ _firststationname=value;}
			get{return _firststationname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LastStationName
		{
			set{ _laststationname=value;}
			get{return _laststationname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LocoServiceType
		{
			set{ _locoservicetype=value;}
			get{return _locoservicetype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WeatherLike
		{
			set{ _weatherlike=value;}
			get{return _weatherlike;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RiskSummary
		{
			set{ _risksummary=value;}
			get{return _risksummary;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RiskTopestLevelId
		{
			set{ _risktopestlevelid=value;}
			get{return _risktopestlevelid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RiskTopestName
		{
			set{ _risktopestname=value;}
			get{return _risktopestname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RiskSecondLevelId
		{
			set{ _risksecondlevelid=value;}
			get{return _risksecondlevelid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RiskSecondLevelName
		{
			set{ _risksecondlevelname=value;}
			get{return _risksecondlevelname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RiskType
		{
			set{ _risktype=value;}
			get{return _risktype;}
		}
		#endregion Model

	}
}

