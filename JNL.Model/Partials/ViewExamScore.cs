/**  版本信息模板在安装目录下，可自行修改。
* ViewExamScore.cs
*
* 功 能： N/A
* 类 名： ViewExamScore
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/10/10 17:26:35   N/A    初版
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
	/// ViewExamScore:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ViewExamScore
	{
		public ViewExamScore()
		{}
		#region Model
		private int _id;
		private int _staffid;
		private string _staffname;
		private string _workno;
		private string _position;
		private string _workplace;
		private string _examtheme;
		private string _examsubject;
		private decimal _score;
		private DateTime _examtime;
		private DateTime _addtime;
		private bool _isdelete;
		private int? _departmentid;
		private string _department;
		private int? _positionid;
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
		public int StaffId
		{
			set{ _staffid=value;}
			get{return _staffid;}
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
		public string WorkNo
		{
			set{ _workno=value;}
			get{return _workno;}
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
		public string WorkPlace
		{
			set{ _workplace=value;}
			get{return _workplace;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ExamTheme
		{
			set{ _examtheme=value;}
			get{return _examtheme;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ExamSubject
		{
			set{ _examsubject=value;}
			get{return _examsubject;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal Score
		{
			set{ _score=value;}
			get{return _score;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ExamTime
		{
			set{ _examtime=value;}
			get{return _examtime;}
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
		#endregion Model

	}
}

