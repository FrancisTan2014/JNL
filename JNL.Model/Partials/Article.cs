/**  版本信息模板在安装目录下，可自行修改。
* Article.cs
*
* 功 能： N/A
* 类 名： Article
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
	/// 文章
	/// </summary>
	[Serializable]
	public partial class Article
	{
		public Article()
		{}
		#region Model
		private int _id;
		private int _categoryid;
	    private int _publevel;
		private string _title;
		private string _content;
	    private string _filepath;
		private int _createstaffid;
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
		/// 文章分类Id
		/// </summary>
		public int CategoryId
		{
			set{ _categoryid=value;}
			get{return _categoryid;}
		}
        /// <summary>
        /// 发布级别（1 总公司； 2 铁路局； 3 机务段）
        /// </summary>
	    public int PubLevel
	    {
            set { _publevel = value; }
            get { return _publevel; }
	    }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 文章内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
        /// <summary>
        /// 文件地址
        /// </summary>
	    public string FilePath
	    {
            set { _filepath = value; }
            get { return _filepath; }
	    }
        /// <summary>
        /// 添加人
        /// </summary>
        public int CreateStaffId
		{
			set{ _createstaffid=value;}
			get{return _createstaffid;}
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

