/**  版本信息模板在安装目录下，可自行修改。
* Warning.cs
*
* 功 能： N/A
* 类 名： Warning
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/22 13:26:38   N/A    初版
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
	/// 预警信息
	/// </summary>
	[Serializable]
	public partial class Warning
	{
		public Warning()
		{}
		#region Model
		private int _id;
		private DateTime _warningtime;
		private DateTime _timelimit;
		private int _warningdepartmentid;
		private string _warningtitle;
		private string _warningcontent;
		private string _changerequirement;
		private bool _hasimplementedall= false;
		private bool _visible= true;
		private int _createstaffid;
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
		/// 预警时间
		/// </summary>
		public DateTime WarningTime
		{
			set{ _warningtime=value;}
			get{return _warningtime;}
		}
		/// <summary>
		/// 落实时限
		/// </summary>
		public DateTime TimeLimit
		{
			set{ _timelimit=value;}
			get{return _timelimit;}
		}
		/// <summary>
		/// 预警部门Id
		/// </summary>
		public int WarningDepartmentId
		{
			set{ _warningdepartmentid=value;}
			get{return _warningdepartmentid;}
		}
		/// <summary>
		/// 预警主题
		/// </summary>
		public string WarningTitle
		{
			set{ _warningtitle=value;}
			get{return _warningtitle;}
		}
		/// <summary>
		/// 预警内容
		/// </summary>
		public string WarningContent
		{
			set{ _warningcontent=value;}
			get{return _warningcontent;}
		}
		/// <summary>
		/// 整改要求
		/// </summary>
		public string ChangeRequirement
		{
			set{ _changerequirement=value;}
			get{return _changerequirement;}
		}
		/// <summary>
		/// 是否全部响应
		/// </summary>
		public bool HasImplementedAll
		{
			set{ _hasimplementedall=value;}
			get{return _hasimplementedall;}
		}
		/// <summary>
		/// 是否对外可见
		/// </summary>
		public bool Visible
		{
			set{ _visible=value;}
			get{return _visible;}
		}
		/// <summary>
		/// 创建人Id
		/// </summary>
		public int CreateStaffId
		{
			set{ _createstaffid=value;}
			get{return _createstaffid;}
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

