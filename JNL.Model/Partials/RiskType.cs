﻿/**  版本信息模板在安装目录下，可自行修改。
* RiskType.cs
*
* 功 能： N/A
* 类 名： RiskType
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/22 13:26:37   N/A    初版
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
	/// 风险信息分类
	/// </summary>
	[Serializable]
	public partial class RiskType
	{
		public RiskType()
		{}
		#region Model
		private int _id;
		private string _name;
		private int _parentid;
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
		/// 分类名称
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ParentId
		{
			set{ _parentid=value;}
			get{return _parentid;}
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

