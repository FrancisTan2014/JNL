/**  版本信息模板在安装目录下，可自行修改。
* ViewLocomotive.cs
*
* 功 能： N/A
* 类 名： ViewLocomotive
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/10/8 23:41:53   N/A    初版
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
	/// ViewLocomotive:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ViewLocomotive
	{
		public ViewLocomotive()
		{}
		#region Model
		private int _id;
		private string _loconumber;
		private int? _locomodelid;
		private string _locomodel;
		private int? _locotypeid;
		private string _locotype;
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
		public string LocoNumber
		{
			set{ _loconumber=value;}
			get{return _loconumber;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LocoModelId
		{
			set{ _locomodelid=value;}
			get{return _locomodelid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LocoModel
		{
			set{ _locomodel=value;}
			get{return _locomodel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LocoTypeId
		{
			set{ _locotypeid=value;}
			get{return _locotypeid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LocoType
		{
			set{ _locotype=value;}
			get{return _locotype;}
		}
		#endregion Model

	}
}

