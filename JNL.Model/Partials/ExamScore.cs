/**  版本信息模板在安装目录下，可自行修改。
* ExamScore.cs
*
* 功 能： N/A
* 类 名： ExamScore
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/22 13:26:35   N/A    初版
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
	/// 考试成绩
	/// </summary>
	[Serializable]
	public partial class ExamScore
	{
		public ExamScore()
		{}
		#region Model
		private int _id;
		private int _staffid=0;
		private string _staffname;
		private string _workno;
		private string _position;
		private string _workplace;
		private string _examtheme;
		private string _examsubject;
		private decimal _score;
		private DateTime _examtime;
		private DateTime _addtime= DateTime.Now;
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
		/// 人员Id
		/// </summary>
		public int StaffId
		{
			set{ _staffid=value;}
			get{return _staffid;}
		}
		/// <summary>
		/// 姓名
		/// </summary>
		public string StaffName
		{
			set{ _staffname=value;}
			get{return _staffname;}
		}
		/// <summary>
		/// 工号
		/// </summary>
		public string WorkNo
		{
			set{ _workno=value;}
			get{return _workno;}
		}
		/// <summary>
		/// 职务
		/// </summary>
		public string Position
		{
			set{ _position=value;}
			get{return _position;}
		}
		/// <summary>
		/// 车间
		/// </summary>
		public string WorkPlace
		{
			set{ _workplace=value;}
			get{return _workplace;}
		}
		/// <summary>
		/// 考试主题
		/// </summary>
		public string ExamTheme
		{
			set{ _examtheme=value;}
			get{return _examtheme;}
		}
		/// <summary>
		/// 考试科目
		/// </summary>
		public string ExamSubject
		{
			set{ _examsubject=value;}
			get{return _examsubject;}
		}
		/// <summary>
		/// 成绩
		/// </summary>
		public decimal Score
		{
			set{ _score=value;}
			get{return _score;}
		}
		/// <summary>
		/// 考试时间
		/// </summary>
		public DateTime ExamTime
		{
			set{ _examtime=value;}
			get{return _examtime;}
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

