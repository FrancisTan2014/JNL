/**  版本信息模板在安装目录下，可自行修改。
* ExceptionLog.cs
*
* 功 能： N/A
* 类 名： ExceptionLog
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/7/28 21:56:02   N/A    初版
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
	/// 程序异常日志表
	/// </summary>
	[Serializable]
	public partial class ExceptionLog
	{
		public ExceptionLog()
		{}
		#region Model
		private int _id;
		private string _message;
		private int _source;
		private string _filename;
		private string _classname;
		private string _methodname;
		private string _instance;
		private string _stacktrace;
		private DateTime _happentime= DateTime.Now;
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
		/// 异常消息
		/// </summary>
		public string Message
		{
			set{ _message=value;}
			get{return _message;}
		}
		/// <summary>
		/// 异常来源（1 后台；2 安卓app）
		/// </summary>
		public int Source
		{
			set{ _source=value;}
			get{return _source;}
		}
		/// <summary>
		/// 异常发生所在文件
		/// </summary>
		public string FileName
		{
			set{ _filename=value;}
			get{return _filename;}
		}
		/// <summary>
		/// 异常发生所在类名
		/// </summary>
		public string ClassName
		{
			set{ _classname=value;}
			get{return _classname;}
		}
		/// <summary>
		/// 异常发生所在方法名
		/// </summary>
		public string MethodName
		{
			set{ _methodname=value;}
			get{return _methodname;}
		}
		/// <summary>
		/// 引发异常的对象
		/// </summary>
		public string Instance
		{
			set{ _instance=value;}
			get{return _instance;}
		}
		/// <summary>
		/// 异常堆栈信息
		/// </summary>
		public string StackTrace
		{
			set{ _stacktrace=value;}
			get{return _stacktrace;}
		}
		/// <summary>
		/// 发生时间
		/// </summary>
		public DateTime HappenTime
		{
			set{ _happentime=value;}
			get{return _happentime;}
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

