/**  版本信息模板在安装目录下，可自行修改。
* Line.cs
*
* 功 能： N/A
* 类 名： Line
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
	/// 线路信息
	/// </summary>
	[Serializable]
	public partial class Line
	{
		public Line()
		{}
		#region Model
		private int _id;
		private string _name;
		private string _firststation = string.Empty;
		private string _laststation = string.Empty;
		private DateTime _addtime = DateTime.Now;
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
		/// 线路名称
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 起始站名称
		/// </summary>
		public string FirstStation
		{
			set{ _firststation=value;}
			get{return _firststation;}
		}
		/// <summary>
		/// 终点站名称
		/// </summary>
		public string LastStation
		{
			set{ _laststation=value;}
			get{return _laststation;}
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

