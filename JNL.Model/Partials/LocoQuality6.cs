/**  版本信息模板在安装目录下，可自行修改。
* LocoQuality6.cs
*
* 功 能： N/A
* 类 名： LocoQuality6
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
	/// 机车质量登登记-机统6
	/// </summary>
	[Serializable]
	public partial class LocoQuality6
	{
		public LocoQuality6()
		{}
		#region Model
		private int _id;
		private int _locomotiveid;
		private DateTime _reporttime;
		private int _repairstaffid;
		private int _livingitemid;
		private string _brokenplace="";
		private string _repairmethod="";
		private DateTime _startrepair;
		private DateTime _endrepair;
		private string _repairdesc;
		private DateTime _addtime= DateTime.Now;
		private DateTime _updatetime= DateTime.Now;
		private bool _isdelete= false;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 机车信息Id
		/// </summary>
		public int LocomotiveId
		{
			set{ _locomotiveid=value;}
			get{return _locomotiveid;}
		}
		/// <summary>
		/// 预报时间
		/// </summary>
		public DateTime ReportTime
		{
			set{ _reporttime=value;}
			get{return _reporttime;}
		}
		/// <summary>
		/// 施修人
		/// </summary>
		public int RepairStaffId
		{
			set{ _repairstaffid=value;}
			get{return _repairstaffid;}
		}
		/// <summary>
		/// 活项归属（关联字典表主键）
		/// </summary>
		public int LivingItemId
		{
			set{ _livingitemid=value;}
			get{return _livingitemid;}
		}
		/// <summary>
		/// 破损处所
		/// </summary>
		public string BrokenPlace
		{
			set{ _brokenplace=value;}
			get{return _brokenplace;}
		}
		/// <summary>
		/// 修理方法
		/// </summary>
		public string RepairMethod
		{
			set{ _repairmethod=value;}
			get{return _repairmethod;}
		}
		/// <summary>
		/// 施修开始时间
		/// </summary>
		public DateTime StartRepair
		{
			set{ _startrepair=value;}
			get{return _startrepair;}
		}
		/// <summary>
		/// 施修结束时间
		/// </summary>
		public DateTime EndRepair
		{
			set{ _endrepair=value;}
			get{return _endrepair;}
		}
		/// <summary>
		/// 施修情况
		/// </summary>
		public string RepairDesc
		{
			set{ _repairdesc=value;}
			get{return _repairdesc;}
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

