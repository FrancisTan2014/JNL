/**  版本信息模板在安装目录下，可自行修改。
* Staff.cs
*
* 功 能： N/A
* 类 名： Staff
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/22 13:26:37   N/A    初版
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
	/// 人员信息
	/// </summary>
	[Serializable]
	public partial class Staff
	{
		public Staff()
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
		private int _worktypeid;
		private int _politicalstatusid;
		private int _positionid;
		private int _departmentid;
		private string _password="";
		private DateTime _addtime;
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
		/// 工作证号
		/// </summary>
		public string WorkId
		{
			set{ _workid=value;}
			get{return _workid;}
		}
		/// <summary>
		/// 工资号
		/// </summary>
		public string SalaryId
		{
			set{ _salaryid=value;}
			get{return _salaryid;}
		}
		/// <summary>
		/// 身份证号码
		/// </summary>
		public string Identity
		{
			set{ _identity=value;}
			get{return _identity;}
		}
		/// <summary>
		/// 姓名
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 性别
		/// </summary>
		public string Gender
		{
			set{ _gender=value;}
			get{return _gender;}
		}
		/// <summary>
		/// 参加工作时间
		/// </summary>
		public DateTime HireDate
		{
			set{ _hiredate=value;}
			get{return _hiredate;}
		}
		/// <summary>
		/// 生日
		/// </summary>
		public DateTime BirthDate
		{
			set{ _birthdate=value;}
			get{return _birthdate;}
		}
		/// <summary>
		/// 干部工人标识（关联字典表主键）
		/// </summary>
		public int WorkFlagId
		{
			set{ _workflagid=value;}
			get{return _workflagid;}
		}
		/// <summary>
		/// 工种（关联字典表主键）
		/// </summary>
		public int WorkTypeId
		{
			set{ _worktypeid=value;}
			get{return _worktypeid;}
		}
		/// <summary>
		/// 政治面貌（关联字典表主键）
		/// </summary>
		public int PoliticalStatusId
		{
			set{ _politicalstatusid=value;}
			get{return _politicalstatusid;}
		}
		/// <summary>
		/// 职务（关联字典表主键）
		/// </summary>
		public int PositionId
		{
			set{ _positionid=value;}
			get{return _positionid;}
		}
		/// <summary>
		/// 所在单位Id（关联铁路单位表主键）
		/// </summary>
		public int DepartmentId
		{
			set{ _departmentid=value;}
			get{return _departmentid;}
		}
		/// <summary>
		/// 系统登录密码
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

