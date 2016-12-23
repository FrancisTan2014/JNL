/**  版本信息模板在安装目录下，可自行修改。
* Quota.cs
*
* 功 能： N/A
* 类 名： Quota
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/12/23 10:48:22   N/A    初版
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
	/// 指标管理
	/// </summary>
	[Serializable]
	public partial class Quota
	{
		public Quota()
		{}
		#region Model
		private int _id;
		private int _quotatype=1;
		private int _staffid;
		private decimal _quotaammount;
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
		/// 指标类型（1 风险信息录入条数指标）
		/// </summary>
		public int QuotaType
		{
			set{ _quotatype=value;}
			get{return _quotatype;}
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
		/// 指标数
		/// </summary>
		public decimal QuotaAmmount
		{
			set{ _quotaammount=value;}
			get{return _quotaammount;}
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

