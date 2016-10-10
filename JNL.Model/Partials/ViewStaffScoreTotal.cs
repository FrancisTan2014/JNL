/**  版本信息模板在安装目录下，可自行修改。
* ViewStaffScoreTotal.cs
*
* 功 能： N/A
* 类 名： ViewStaffScoreTotal
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/10/10 22:10:46   N/A    初版
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
	/// ViewStaffScoreTotal:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ViewStaffScoreTotal
	{
		public ViewStaffScoreTotal()
		{}
		#region Model
		private int _staffid;
		private int _year;
		private decimal? _minusscore;
		private string _staffname;
		private int? _departmentid;
		private string _department;
		private int? _positionid;
		private string _position;
		private string _salaryid;
		private string _workid;
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
		public int Year
		{
			set{ _year=value;}
			get{return _year;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? MinusScore
		{
			set{ _minusscore=value;}
			get{return _minusscore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string StaffName
		{
			set{ _staffname=value;}
			get{return _staffname;}
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
		#endregion Model

	}
}

