/**  版本信息模板在安装目录下，可自行修改。
* ViewRiskResponseStaff.cs
*
* 功 能： N/A
* 类 名： ViewRiskResponseStaff
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/10/3 19:36:57   N/A    初版
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
	/// ViewRiskResponseStaff:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ViewRiskResponseStaff
	{
		public ViewRiskResponseStaff()
		{}
		#region Model
		private int _id;
		private int _riskid;
		private int _responsestaffid;
		private DateTime _addtime;
		private bool _isdelete;
		private string _name;
		private int? _departmentid;
		private string _department;
	    private string _salaryid;
	    private string _workid;
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
		public int RiskId
		{
			set{ _riskid=value;}
			get{return _riskid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ResponseStaffId
		{
			set{ _responsestaffid=value;}
			get{return _responsestaffid;}
		}
		/// <summary>
		/// 
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
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? DepartmentId
		{
			set{ _departmentid=value;}
			get{return _departmentid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Department
		{
			set{ _department=value;}
			get{return _department;}
		}

	    public string SalaryId
	    {
	        set { _salaryid = value; }
            get { return _salaryid; }
	    }

	    public string WorkId
	    {
	        set { _workid = value; }
            get {  return _workid; }
	    }
        #endregion Model

    }
}

