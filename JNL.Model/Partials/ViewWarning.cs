/**  版本信息模板在安装目录下，可自行修改。
* ViewWarning.cs
*
* 功 能： N/A
* 类 名： ViewWarning
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/10/7 20:47:34   N/A    初版
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
	/// ViewWarning:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ViewWarning
	{
		public ViewWarning()
		{}
		#region Model
		private int _id;
		private string _warningcontent;
		private int _warningsource;
		private int _warningstaffid;
		private DateTime _warningtime;
		private string _warningtitle;
		private string _implementdeparts;
		private string _changerequirement;
		private bool _hasbeganimplement;
		private bool _hasimplementedall;
		private bool _visible;
		private int _createstaffid;
		private DateTime _addtime;
		private DateTime _updatetime;
		private bool _isdelete;
		private string _warningstaff;
		private int? _warningdepartid;
		private string _warningdepart;
		private string _warningsourcename;
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
		public string WarningContent
		{
			set{ _warningcontent=value;}
			get{return _warningcontent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int WarningSource
		{
			set{ _warningsource=value;}
			get{return _warningsource;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int WarningStaffId
		{
			set{ _warningstaffid=value;}
			get{return _warningstaffid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime WarningTime
		{
			set{ _warningtime=value;}
			get{return _warningtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WarningTitle
		{
			set{ _warningtitle=value;}
			get{return _warningtitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ImplementDeparts
		{
			set{ _implementdeparts=value;}
			get{return _implementdeparts;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ChangeRequirement
		{
			set{ _changerequirement=value;}
			get{return _changerequirement;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool HasBeganImplement
		{
			set{ _hasbeganimplement=value;}
			get{return _hasbeganimplement;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool HasImplementedAll
		{
			set{ _hasimplementedall=value;}
			get{return _hasimplementedall;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Visible
		{
			set{ _visible=value;}
			get{return _visible;}
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
		public string WarningStaff
		{
			set{ _warningstaff=value;}
			get{return _warningstaff;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? WarningDepartId
		{
			set{ _warningdepartid=value;}
			get{return _warningdepartid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WarningDepart
		{
			set{ _warningdepart=value;}
			get{return _warningdepart;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WarningSourceName
		{
			set{ _warningsourcename=value;}
			get{return _warningsourcename;}
		}

	    public DateTime TimeLimit { get; set; }
		#endregion Model

	}
}

