/**  版本信息模板在安装目录下，可自行修改。
* ViewLocoQuality6.cs
*
* 功 能： N/A
* 类 名： ViewLocoQuality6
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/10/8 23:41:52   N/A    初版
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
	/// ViewLocoQuality6:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ViewLocoQuality6
	{
		public ViewLocoQuality6()
		{}
		#region Model
		private int _id;
		private int _locomotiveid;
		private DateTime _reporttime;
		private int _repairstaffid;
		private int _livingitemid;
		private string _brokenplace;
		private string _repairmethod;
		private DateTime _startrepair;
		private DateTime _endrepair;
		private string _repairdesc;
		private DateTime _addtime;
		private DateTime _updatetime;
		private bool _isdelete;
		private string _locomodel;
		private int? _locomodelid;
		private string _loconumber;
		private string _locotype;
		private int? _locotypeid;
		private string _repairstaff;
		private int? _departmentid;
		private string _department;
		private string _livingitem;
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
		public int LocomotiveId
		{
			set{ _locomotiveid=value;}
			get{return _locomotiveid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ReportTime
		{
			set{ _reporttime=value;}
			get{return _reporttime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int RepairStaffId
		{
			set{ _repairstaffid=value;}
			get{return _repairstaffid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int LivingItemId
		{
			set{ _livingitemid=value;}
			get{return _livingitemid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BrokenPlace
		{
			set{ _brokenplace=value;}
			get{return _brokenplace;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RepairMethod
		{
			set{ _repairmethod=value;}
			get{return _repairmethod;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime StartRepair
		{
			set{ _startrepair=value;}
			get{return _startrepair;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime EndRepair
		{
			set{ _endrepair=value;}
			get{return _endrepair;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RepairDesc
		{
			set{ _repairdesc=value;}
			get{return _repairdesc;}
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
		public DateTime UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
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
		public string LocoModel
		{
			set{ _locomodel=value;}
			get{return _locomodel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LocoModelId
		{
			set{ _locomodelid=value;}
			get{return _locomodelid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LocoNumber
		{
			set{ _loconumber=value;}
			get{return _loconumber;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LocoType
		{
			set{ _locotype=value;}
			get{return _locotype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LocoTypeId
		{
			set{ _locotypeid=value;}
			get{return _locotypeid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RepairStaff
		{
			set{ _repairstaff=value;}
			get{return _repairstaff;}
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
		public string LivingItem
		{
			set{ _livingitem=value;}
			get{return _livingitem;}
		}
		#endregion Model

	}
}

