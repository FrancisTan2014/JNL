/**  版本信息模板在安装目录下，可自行修改。
* ViewStaff.cs
*
* 功 能： N/A
* 类 名： ViewStaff
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/10/1 21:26:36   N/A    初版
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
	/// ViewStaff:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ViewStaff
	{
		public ViewStaff()
		{}
		#region Model
		private int _id;
		private string _workid;
		private string _salaryid;
		private string _identity;
		private string _name;
		private string _gender;
		private DateTime _hiredate;
		private DateTime _birthdate;
		private int _workflagid;
		private string _workflag;
		private int _worktypeid;
		private string _worktype;
		private int _politicalstatusid;
		private string _politicalstatus;
		private int _positionid;
		private string _position;
		private int _departmentid;
		private string _department;
		private string _password;
		private DateTime _addtime;
		private DateTime _updatetime;
		private bool _isdelete;
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
		/// <summary>
		/// 
		/// </summary>
		public string Identity
		{
			set{ _identity=value;}
			get{return _identity;}
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
		public string Gender
		{
			set{ _gender=value;}
			get{return _gender;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime HireDate
		{
			set{ _hiredate=value;}
			get{return _hiredate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime BirthDate
		{
			set{ _birthdate=value;}
			get{return _birthdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int WorkFlagId
		{
			set{ _workflagid=value;}
			get{return _workflagid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WorkFlag
		{
			set{ _workflag=value;}
			get{return _workflag;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int WorkTypeId
		{
			set{ _worktypeid=value;}
			get{return _worktypeid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WorkType
		{
			set{ _worktype=value;}
			get{return _worktype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int PoliticalStatusId
		{
			set{ _politicalstatusid=value;}
			get{return _politicalstatusid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PoliticalStatus
		{
			set{ _politicalstatus=value;}
			get{return _politicalstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int PositionId
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
		public int DepartmentId
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
		public string Password
		{
			set{ _password=value;}
			get{return _password;}
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
		#endregion Model

	}
}

