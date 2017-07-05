using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JNL.Model
{
    [Serializable]
    public partial class ViewAdminConfig
    {
        public ViewAdminConfig()
        { }
        #region Model
        private int _id;
        private int _configtype;
        private int _targetid;
        private DateTime? _addtime;
        private string _description;
        private bool _isbottom;
        private bool _isdelete;
        private int? _parentid;
        private int? _secondlevelid;
        private string _secondlevelname;
        private string _topestname;
        private int? _topesttypeid;
        private DateTime? _updatetime;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ConfigType
        {
            set { _configtype = value; }
            get { return _configtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TargetId
        {
            set { _targetid = value; }
            get { return _targetid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsBottom
        {
            set { _isbottom = value; }
            get { return _isbottom; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete
        {
            set { _isdelete = value; }
            get { return _isdelete; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ParentId
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SecondLevelId
        {
            set { _secondlevelid = value; }
            get { return _secondlevelid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SecondLevelName
        {
            set { _secondlevelname = value; }
            get { return _secondlevelname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TopestName
        {
            set { _topestname = value; }
            get { return _topestname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? TopestTypeId
        {
            set { _topesttypeid = value; }
            get { return _topesttypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        #endregion Model
    }
}
