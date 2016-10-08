/**  版本信息模板在安装目录下，可自行修改。
* ViewWarningImplement.cs
*
* 功 能： N/A
* 类 名： ViewWarningImplement
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/10/7 20:36:04   N/A    初版
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
	/// ViewWarningImplement:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ViewWarningImplement
	{
		public ViewWarningImplement()
		{}
		#region Model
		private int _id;
		private int _warningid;
		private int _implementdepartmentid;
		private bool _hasimplemented;
		private bool _hasresponded;
		private string _implementdetail;
		private DateTime _respondtime;
		private int _responseverifystatus;
		private int _verifystaffid;
		private DateTime _verifytime;
		private DateTime _addtime;
		private DateTime _updatetime;
		private bool _isdelete;
		private string _implementdepart;
		private string _verifystaff;
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
		public int WarningId
		{
			set{ _warningid=value;}
			get{return _warningid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ImplementDepartmentId
		{
			set{ _implementdepartmentid=value;}
			get{return _implementdepartmentid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool HasImplemented
		{
			set{ _hasimplemented=value;}
			get{return _hasimplemented;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool HasResponded
		{
			set{ _hasresponded=value;}
			get{return _hasresponded;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ImplementDetail
		{
			set{ _implementdetail=value;}
			get{return _implementdetail;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime RespondTime
		{
			set{ _respondtime=value;}
			get{return _respondtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ResponseVerifyStatus
		{
			set{ _responseverifystatus=value;}
			get{return _responseverifystatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int VerifyStaffId
		{
			set{ _verifystaffid=value;}
			get{return _verifystaffid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime VerifyTime
		{
			set{ _verifytime=value;}
			get{return _verifytime;}
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
		public string ImplementDepart
		{
			set{ _implementdepart=value;}
			get{return _implementdepart;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string VerifyStaff
		{
			set{ _verifystaff=value;}
			get{return _verifystaff;}
		}
		#endregion Model

	}
}

