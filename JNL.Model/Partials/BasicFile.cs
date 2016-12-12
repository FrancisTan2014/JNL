/**  版本信息模板在安装目录下，可自行修改。
* BasicFile.cs
*
* 功 能： N/A
* 类 名： BasicFile
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
	/// 基础文件
	/// </summary>
	[Serializable]
	public partial class BasicFile
	{
		public BasicFile()
		{}
		#region Model
		private int _id;
		private string _filename;
		private string _filenumber;
		private string _filepath;
		private int _filetype;
		private int _publishlevel;
	    private DateTime _publishtime=DateTime.Now;
		private DateTime _addtime= DateTime.Now;
		private bool _isdelete;
		/// <summary>
		/// 主键
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 文件名称
		/// </summary>
		public string FileName
		{
			set{ _filename=value;}
			get{return _filename;}
		}
		/// <summary>
		/// 文件编号（如铁运[2012]111号）
		/// </summary>
		public string FileNumber
		{
			set{ _filenumber=value;}
			get{return _filenumber;}
		}
		/// <summary>
		/// 文件在服务器存储的相对地址，以/开头
		/// </summary>
		public string FilePath
		{
			set{ _filepath=value;}
			get{return _filepath;}
		}
		/// <summary>
		/// 文件类型（1 技术规章； 2 企业标准； 3 制度措施）
		/// </summary>
		public int FileType
		{
			set{ _filetype=value;}
			get{return _filetype;}
		}
		/// <summary>
		/// 文件发布等级（1 总公司； 2 铁路局； 3 机务段；）
		/// </summary>
		public int PublishLevel
		{
			set{ _publishlevel=value;}
			get{return _publishlevel;}
		}
        /// <summary>
        /// 发文日期
        /// </summary>
	    public DateTime PublishTime
	    {
	        set { _publishtime = value; }
            get { return _publishtime; }
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
		/// 
		/// </summary>
		public bool IsDelete
		{
			set{ _isdelete=value;}
			get{return _isdelete;}
		}
		#endregion Model

	}
}

