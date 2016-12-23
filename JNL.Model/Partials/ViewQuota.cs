/**  版本信息模板在安装目录下，可自行修改。
* ViewQuota.cs
*
* 功 能： N/A
* 类 名： ViewQuota
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/12/23 10:48:23   N/A    初版
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
	/// 指标完成情况
	/// </summary>
	[Serializable]
	public partial class ViewQuota
	{
		public ViewQuota()
		{}
		#region Model
		private int _id;
		private int _quotatype;
		private int _staffid;
		private decimal _quotaammount;
		private DateTime _updatetime;
		private bool _isdelete;
		private string _name;
		private int? _departmentid;
		private string _department;
		private int? _positionid;
		private string _position;
		private string _workid;
		private string _salaryid;
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
		public int QuotaType
		{
			set{ _quotatype=value;}
			get{return _quotatype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int StaffId
		{
			set{ _staffid=value;}
			get{return _staffid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal QuotaAmmount
		{
			set{ _quotaammount=value;}
			get{return _quotaammount;}
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
		public int? PositionId
		{
			set{ _positionid=value;}
			get{return _positionid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Position
		{
			set{ _position=value;}
			get{return _position;}
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
		public string SalaryId
		{
			set{ _salaryid=value;}
			get{return _salaryid;}
		}
		#endregion Model

	}
}

