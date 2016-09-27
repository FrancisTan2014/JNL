/**  版本信息模板在安装目录下，可自行修改。
* WarningImplement.cs
*
* 功 能： N/A
* 类 名： WarningImplement
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/22 13:26:39   N/A    初版
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
	/// 预警落实信息
	/// </summary>
	[Serializable]
	public partial class WarningImplement
	{
		public WarningImplement()
		{}
		#region Model
		private int _id;
		private int _warningid;
		private int _implementdepartmentid;
		private bool _hasimplemented= false;
		private bool _hasresponded= false;
		private string _implementdetail="";
		private DateTime _respondtime;
		private int _responseverifystatus=1;
		private int _verifystaffid;
		private DateTime _verifytime;
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
		/// 预警信息Id
		/// </summary>
		public int WarningId
		{
			set{ _warningid=value;}
			get{return _warningid;}
		}
		/// <summary>
		/// 落实部门Id
		/// </summary>
		public int ImplementDepartmentId
		{
			set{ _implementdepartmentid=value;}
			get{return _implementdepartmentid;}
		}
		/// <summary>
		/// 是否已落实
		/// </summary>
		public bool HasImplemented
		{
			set{ _hasimplemented=value;}
			get{return _hasimplemented;}
		}
		/// <summary>
		/// 是否已响应
		/// </summary>
		public bool HasResponded
		{
			set{ _hasresponded=value;}
			get{return _hasresponded;}
		}
		/// <summary>
		/// 预警响应
		/// </summary>
		public string ImplementDetail
		{
			set{ _implementdetail=value;}
			get{return _implementdetail;}
		}
		/// <summary>
		/// 响应时间
		/// </summary>
		public DateTime RespondTime
		{
			set{ _respondtime=value;}
			get{return _respondtime;}
		}
		/// <summary>
		/// 响应审核状态（1 未审核； 2 审核通过； 3 响应被否决）
		/// </summary>
		public int ResponseVerifyStatus
		{
			set{ _responseverifystatus=value;}
			get{return _responseverifystatus;}
		}
		/// <summary>
		/// 审核人Id
		/// </summary>
		public int VerifyStaffId
		{
			set{ _verifystaffid=value;}
			get{return _verifystaffid;}
		}
		/// <summary>
		/// 审核时间
		/// </summary>
		public DateTime VerifyTime
		{
			set{ _verifytime=value;}
			get{return _verifytime;}
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

