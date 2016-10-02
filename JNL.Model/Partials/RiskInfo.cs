/**  版本信息模板在安装目录下，可自行修改。
* RiskInfo.cs
*
* 功 能： N/A
* 类 名： RiskInfo
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/22 13:26:36   N/A    初版
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
	/// 风险信息登记
	/// </summary>
	[Serializable]
	public partial class RiskInfo
	{
		public RiskInfo()
		{}
		#region Model
		private int _id;
		private int _reportstaffid;
		private int _occurrencelineid;
		private int _firststationid;
		private int _laststationid;
		private int _locoservicetypeid;
		private int _weatherid;
		private DateTime _occurrencetime;
		private int _risksummaryid;
		private string _riskdetails="";
		private string _riskreason="";
		private string _riskfix="";
		private bool _visible = false;
		private int _risktypeid;
		private int _verifystatus=1;
		private int _verifystaffid;
		private DateTime _verifytime= DateTime.Now;
		private bool _showinstresspage = false;
		private DateTime _dealtimelimit = DateTime.Now;
		private bool _hasdealed= false;
		private bool _needroomsign= false;
		private bool _needleadersign= false;
		private bool _needwritefixdesc= false;
		private bool _needstresstrack= false;
		private DateTime _addtime= DateTime.Now;
		private DateTime _updatetime= DateTime.Now;
		private bool _isdelete= false;
		/// <summary>
		/// 主键
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 提报人Id（关联人员信息表主键）
		/// </summary>
		public int ReportStaffId
		{
			set{ _reportstaffid=value;}
			get{return _reportstaffid;}
		}
		/// <summary>
		/// 事件发生线路Id（关联线路表主键）
		/// </summary>
		public int OccurrenceLineId
		{
			set{ _occurrencelineid=value;}
			get{return _occurrencelineid;}
		}
		/// <summary>
		/// 事件发生区间起点站Id
		/// </summary>
		public int FirstStationId
		{
			set{ _firststationid=value;}
			get{return _firststationid;}
		}
		/// <summary>
		/// 事件发生区间终点站Id
		/// </summary>
		public int LastStationId
		{
			set{ _laststationid=value;}
			get{return _laststationid;}
		}
		/// <summary>
		/// 列车类别（关联字典表主键）
		/// </summary>
		public int LocoServiceTypeId
		{
			set{ _locoservicetypeid=value;}
			get{return _locoservicetypeid;}
		}
		/// <summary>
		/// 天气情况（关联字典表主键）
		/// </summary>
		public int WeatherId
		{
			set{ _weatherid=value;}
			get{return _weatherid;}
		}
		/// <summary>
		/// 事件发生时间
		/// </summary>
		public DateTime OccurrenceTime
		{
			set{ _occurrencetime=value;}
			get{return _occurrencetime;}
		}
		/// <summary>
		/// 风险分类Id（关联风险分类表主键）
		/// </summary>
		public int RiskSummaryId
		{
			set{ _risksummaryid=value;}
			get{return _risksummaryid;}
		}
		/// <summary>
		/// 风险详情
		/// </summary>
		public string RiskDetails
		{
			set{ _riskdetails=value;}
			get{return _riskdetails;}
		}
		/// <summary>
		/// 问题原因
		/// </summary>
		public string RiskReason
		{
			set{ _riskreason=value;}
			get{return _riskreason;}
		}
		/// <summary>
		/// 落实情况
		/// </summary>
		public string RiskFix
		{
			set{ _riskfix=value;}
			get{return _riskfix;}
		}
		/// <summary>
		/// 对外是否可见
		/// </summary>
		public bool Visible
		{
			set{ _visible=value;}
			get{return _visible;}
		}
		/// <summary>
		/// 风险分类Id
		/// </summary>
		public int RiskTypeId
		{
			set{ _risktypeid=value;}
			get{return _risktypeid;}
		}
		/// <summary>
		/// 审核状态（1 待审核； 2 审核通过； 3 审核拒绝；）
		/// </summary>
		public int VerifyStatus
		{
			set{ _verifystatus=value;}
			get{return _verifystatus;}
		}
		/// <summary>
		/// 审核人Id
		/// </summary>
		public int VerifyStaffId
		{
			set{ _verifystaffid=value;}
			get{return _verifystaffid;}
		}
		/// <summary>
		/// 审核时间
		/// </summary>
		public DateTime VerifyTime
		{
			set{ _verifytime=value;}
			get{return _verifytime;}
		}
		/// <summary>
		/// 是否进入日分析重点甄选队列，或为真，则此风险信息将在每日重点安全信息甄选统计页面中展示
		/// </summary>
		public bool ShowInStressPage
		{
			set{ _showinstresspage=value;}
			get{return _showinstresspage;}
		}
		/// <summary>
		/// 处置期限
		/// </summary>
		public DateTime DealTimeLimit
		{
			set{ _dealtimelimit=value;}
			get{return _dealtimelimit;}
		}
		/// <summary>
		/// 是否已处置
		/// </summary>
		public bool HasDealed
		{
			set{ _hasdealed=value;}
			get{return _hasdealed;}
		}
		/// <summary>
		/// 是否需要科室签字
		/// </summary>
		public bool NeedRoomSign
		{
			set{ _needroomsign=value;}
			get{return _needroomsign;}
		}
		/// <summary>
		/// 是否需要领导签字
		/// </summary>
		public bool NeedLeaderSign
		{
			set{ _needleadersign=value;}
			get{return _needleadersign;}
		}
		/// <summary>
		/// 是否需要填写落实情况
		/// </summary>
		public bool NeedWriteFixDesc
		{
			set{ _needwritefixdesc=value;}
			get{return _needwritefixdesc;}
		}
		/// <summary>
		/// 是否重点追踪
		/// </summary>
		public bool NeedStressTrack
		{
			set{ _needstresstrack=value;}
			get{return _needstresstrack;}
		}
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		/// <summary>
		/// 删除标识
		/// </summary>
		public bool IsDelete
		{
			set{ _isdelete=value;}
			get{return _isdelete;}
		}
		#endregion Model

	}
}

