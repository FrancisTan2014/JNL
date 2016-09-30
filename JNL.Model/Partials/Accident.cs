/**  版本信息模板在安装目录下，可自行修改。
* Accident.cs
*
* 功 能： N/A
* 类 名： Accident
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/30 21:58:21   N/A    初版
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
		private string _responsebureau="";
		private string _responsedepot="";
		private string _accidenttype="";
		private string _locotype="";
		private string _weatherlike="";
		private string _keywords="";
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
		/// 责任铁路局
		/// </summary>
		public string ResponseBureau
		{
			set{ _responsebureau=value;}
			get{return _responsebureau;}
		}
		/// <summary>
		/// 责任机务段
		/// </summary>
		public string ResponseDepot
		{
			set{ _responsedepot=value;}
			get{return _responsedepot;}
		}
		/// <summary>
		/// 事故类别
		/// </summary>
		public string AccidentType
		{
			set{ _accidenttype=value;}
			get{return _accidenttype;}
		}
		/// <summary>
		/// 列车分类
		/// </summary>
		public string LocoType
		{
			set{ _locotype=value;}
			get{return _locotype;}
		}
		/// <summary>
		/// 天气情况
		/// </summary>
		public string WeatherLike
		{
			set{ _weatherlike=value;}
			get{return _weatherlike;}
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

