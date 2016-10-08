using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JNL.DataMigration
{
    public class Live6
    {
        public Live6()
        { }
        #region Model
        private int _id;
        private int _type = 0;
        private string _model;
        private string _number;
        private DateTime? _report_time;
        private string _repair_user_id;
        private int _live_own;
        private string _damage_palce;
        private string _repair_method;
        private DateTime? _repair_start_time;
        private DateTime? _repair_end_time;
        private string _repair_detail;
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
        public string repair_user_id
        {
            set { _repair_user_id = value; }
            get { return _repair_user_id; }
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
        public string damage_palce
        {
            set { _damage_palce = value; }
            get { return _damage_palce; }
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
        public DateTime? repair_start_time
        {
            set { _repair_start_time = value; }
            get { return _repair_start_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? repair_end_time
        {
            set { _repair_end_time = value; }
            get { return _repair_end_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string repair_detail
        {
            set { _repair_detail = value; }
            get { return _repair_detail; }
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
