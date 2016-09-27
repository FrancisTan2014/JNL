/**  版本信息模板在安装目录下，可自行修改。
* Accident.cs
*
* 功 能： N/A
* 类 名： Accident
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/22 13:26:34   N/A    初版
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
	/// 典型事故
	/// </summary>
	[Serializable]
	public partial class Accident
	{
		public Accident()
		{}
		#region Model
		private int _id;
		private DateTime _occurrencetime;
		private string _place;
		private int _depotid;
		private int _accidenttypeid;
		private int _locoservicetypeid;
		private int _weatherid;
		private string _keywords;
		private string _summary="";
		private string _help="";
		private string _responsibility="";
		private string _lesson="";
		private string _reason="";
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
		/// 事故发生时间
		/// </summary>
		public DateTime OccurrenceTime
		{
			set{ _occurrencetime=value;}
			get{return _occurrencetime;}
		}
		/// <summary>
		/// 事故发生地点
		/// </summary>
		public string Place
		{
			set{ _place=value;}
			get{return _place;}
		}
		/// <summary>
		/// 责任机务/电务/车辆段Id（关联Department表主键）
		/// </summary>
		public int DepotId
		{
			set{ _depotid=value;}
			get{return _depotid;}
		}
		/// <summary>
		/// 事故类别Id（关联字典表主键）
		/// </summary>
		public int AccidentTypeId
		{
			set{ _accidenttypeid=value;}
			get{return _accidenttypeid;}
		}
		/// <summary>
		/// 列车类别（如：货车、客车等）Id
		/// </summary>
		public int LocoServiceTypeId
		{
			set{ _locoservicetypeid=value;}
			get{return _locoservicetypeid;}
		}
		/// <summary>
		/// 天气情况Id（关联字典表主键）
		/// </summary>
		public int WeatherId
		{
			set{ _weatherid=value;}
			get{return _weatherid;}
		}
		/// <summary>
		/// 关键词
		/// </summary>
		public string Keywords
		{
			set{ _keywords=value;}
			get{return _keywords;}
		}
		/// <summary>
		/// 事故概述
		/// </summary>
		public string Summary
		{
			set{ _summary=value;}
			get{return _summary;}
		}
		/// <summary>
		/// 补救措施
		/// </summary>
		public string Help
		{
			set{ _help=value;}
			get{return _help;}
		}
		/// <summary>
		/// 责任追究
		/// </summary>
		public string Responsibility
		{
			set{ _responsibility=value;}
			get{return _responsibility;}
		}
		/// <summary>
		/// 吸取到的教训
		/// </summary>
		public string Lesson
		{
			set{ _lesson=value;}
			get{return _lesson;}
		}
		/// <summary>
		/// 事故原因
		/// </summary>
		public string Reason
		{
			set{ _reason=value;}
			get{return _reason;}
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

