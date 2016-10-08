using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JNL.DataMigration
{
    public class Live28
    {
        public Live28()
        { }
        #region Model
        private int _id;
        private int _type = 0;
        private string _model;
        private string _number;
        private DateTime? _report_time;
        private string _repair_team;
        private int _live_own;
        private string _repair_method;
        private int _repair_process;
        private string _live_item;
        private int _status = 1;
        private int _create_time;
        private int _update_time;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string model
        {
            set { _model = value; }
            get { return _model; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string number
        {
            set { _number = value; }
            get { return _number; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? report_time
        {
            set { _report_time = value; }
            get { return _report_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string repair_team
        {
            set { _repair_team = value; }
            get { return _repair_team; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int live_own
        {
            set { _live_own = value; }
            get { return _live_own; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string repair_method
        {
            set { _repair_method = value; }
            get { return _repair_method; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int repair_process
        {
            set { _repair_process = value; }
            get { return _repair_process; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string live_item
        {
            set { _live_item = value; }
            get { return _live_item; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int create_time
        {
            set { _create_time = value; }
            get { return _create_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int update_time
        {
            set { _update_time = value; }
            get { return _update_time; }
        }
        #endregion Model
    }
}
