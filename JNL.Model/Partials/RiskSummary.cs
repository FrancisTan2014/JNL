/**  版本信息模板在安装目录下，可自行修改。
* RiskSummary.cs
*
* 功 能： N/A
* 类 名： RiskSummary
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
	/// 风险分类
	/// </summary>
	[Serializable]
	public partial class RiskSummary
	{
		public RiskSummary()
		{}
		#region Model
		private int _id;
		private string _description;
		private int _parentid;
	    private int _topesttypeid;
	    private string _topestname = string.Empty;
	    private int _secondlevelid;
	    private string _secondlevelname = string.Empty;
		private bool _isbottom;
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
		/// 分类概述
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 父级Id
		/// </summary>
		public int ParentId
		{
			set{ _parentid=value;}
			get{return _parentid;}
		}
        /// <summary>
        /// 是否最底层，若为真，则表示此项为风险概述内容，否则它表示一个风险概述的分类（如红线、甲、乙等）
        /// </summary>
        public bool IsBottom
		{
			set{ _isbottom=value;}
			get{return _isbottom;}
		}
        /// <summary>
        /// 顶级分类Id
        /// </summary>
	    public int TopestTypeId
	    {
            set { _topesttypeid = value; }
            get { return _topesttypeid; }
	    }

	    public string TopestName
	    {
            set { _topestname = value; }
            get { return _topestname; }
	    }

	    public int SecondLevelId
	    {
	        set { _secondlevelid = value; }
            get { return _secondlevelid; }
	    }

	    public string SecondLevelName
	    {
	        set { _secondlevelname = value; }
            get { return _secondlevelname; }
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

