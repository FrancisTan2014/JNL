/**  版本信息模板在安装目录下，可自行修改。
* ViewArticle.cs
*
* 功 能： N/A
* 类 名： ViewArticle
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/10/7 16:16:34   N/A    初版
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
	/// ViewArticle:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ViewArticle
	{
		public ViewArticle()
		{}
		#region Model
		private int _id;
		private int _categoryid;
		private string _title;
		private string _content;
		private int _createstaffid;
		private DateTime _addtime;
		private bool _isdelete;
		private string _categoryname;
		private string _creator;
		private int? _departmentid;
		private string _department;
		private string _salaryid;
		private string _workid;
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
		public int CategoryId
		{
			set{ _categoryid=value;}
			get{return _categoryid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int CreateStaffId
		{
			set{ _createstaffid=value;}
			get{return _createstaffid;}
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
		public string CategoryName
		{
			set{ _categoryname=value;}
			get{return _categoryname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Creator
		{
			set{ _creator=value;}
			get{return _creator;}
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
		#endregion Model

	}
}

