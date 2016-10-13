/**  版本信息模板在安装目录下，可自行修改。
* TraceInfo.cs
*
* 功 能： N/A
* 类 名： TraceInfo
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/22 13:26:38   N/A    初版
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
	/// 信息追踪
	/// </summary>
	[Serializable]
	public partial class TraceInfo
	{
		public TraceInfo()
		{}
		#region Model
		private int _id;
		private int _tracetype=2;
		private string _responsedepartmentids;
		private string _tracecontent;
	    private string _filename = "";
		private string _filepath="";
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
		/// 追踪类型（1 局追； 2 段追；）
		/// </summary>
		public int TraceType
		{
			set{ _tracetype=value;}
			get{return _tracetype;}
		}
		/// <summary>
		/// 责任部门Id
		/// </summary>
		public string ResponseDepartmentIds
		{
			set{ _responsedepartmentids=value;}
			get{return _responsedepartmentids;}
		}
		/// <summary>
		/// 追踪内容
		/// </summary>
		public string TraceContent
		{
			set{ _tracecontent=value;}
			get{return _tracecontent;}
		}
        /// <summary>
        /// 附件文件名称
        /// </summary>
	    public string FileName
	    {
            set { _filename = value; }
            get { return _filename; }
	    }
		/// <summary>
		/// 附件文件路径
		/// </summary>
		public string FilePath
		{
			set{ _filepath=value;}
			get{return _filepath;}
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

