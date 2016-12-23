/**  版本信息模板在安装目录下，可自行修改。
* QuotaAchievement.cs
*
* 功 能： N/A
* 类 名： QuotaAchievement
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/12/23 10:48:23   N/A    初版
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
	/// 指标完成情况
	/// </summary>
	[Serializable]
	public partial class QuotaAchievement
	{
		public QuotaAchievement()
		{}
		#region Model
		private int _id;
		private int _quotaid;
		private decimal _achieveammount;
		private int _year;
		private int _month;
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
		/// 指标Id
		/// </summary>
		public int QuotaId
		{
			set{ _quotaid=value;}
			get{return _quotaid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal AchieveAmmount
		{
			set{ _achieveammount=value;}
			get{return _achieveammount;}
		}
		/// <summary>
		/// 年份
		/// </summary>
		public int Year
		{
			set{ _year=value;}
			get{return _year;}
		}
		/// <summary>
		/// 月份
		/// </summary>
		public int Month
		{
			set{ _month=value;}
			get{return _month;}
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

