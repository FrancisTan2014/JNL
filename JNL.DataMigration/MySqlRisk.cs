using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JNL.DataMigration
{
    class MySqlRisk
    {
        public MySqlRisk()
        { }
        #region Model
        private int _id;
        private string _report_user_id;
        private int _from_dept_id;
        private DateTime _event_date_time;
        private int _event_time_year;
        private int _event_time_month;
        private int _event_time_day;
        private int _event_time_hour;
        private int _event_time_min;
        private int _event_place_type = 1;
        private int _event_place_line = 0;
        private int? _event_place_station = 0;
        private int? _event_place_start = 0;
        private int? _event_place_end = 0;
        private int? _train_type = 0;
        private string _weather_type;
        private string _resp_user_id;
        private int? _resp_dept_id = 0;
        private int? _risk_type = 0;
        private int? _risk_level = 0;
        private int? _risk_outline_id = 0;
        private string _risk_detail;
        private int _risk_store = 2;
        private int _menu_cat = 1;
        private int _menu_type = 1;
        private int _status = 1;
        private int _audit_status = 1;
        private int _audit_time;
        private int _room_sign = 2;
        private int _leader_sign = 2;
        private int _need_fix_text = 2;
        private int _is_stress = 2;
        private int _stress_trace = 1;
        private int _deal_status = 1;
        private string _fix_user_id;
        private string _risk_reason;
        private string _risk_fix;
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
        public string report_user_id
        {
            set { _report_user_id = value; }
            get { return _report_user_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int from_dept_id
        {
            set { _from_dept_id = value; }
            get { return _from_dept_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime event_date_time
        {
            set { _event_date_time = value; }
            get { return _event_date_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int event_time_year
        {
            set { _event_time_year = value; }
            get { return _event_time_year; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int event_time_month
        {
            set { _event_time_month = value; }
            get { return _event_time_month; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int event_time_day
        {
            set { _event_time_day = value; }
            get { return _event_time_day; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int event_time_hour
        {
            set { _event_time_hour = value; }
            get { return _event_time_hour; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int event_time_min
        {
            set { _event_time_min = value; }
            get { return _event_time_min; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int event_place_type
        {
            set { _event_place_type = value; }
            get { return _event_place_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int event_place_line
        {
            set { _event_place_line = value; }
            get { return _event_place_line; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? event_place_station
        {
            set { _event_place_station = value; }
            get { return _event_place_station; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? event_place_start
        {
            set { _event_place_start = value; }
            get { return _event_place_start; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? event_place_end
        {
            set { _event_place_end = value; }
            get { return _event_place_end; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? train_type
        {
            set { _train_type = value; }
            get { return _train_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string weather_type
        {
            set { _weather_type = value; }
            get { return _weather_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string resp_user_id
        {
            set { _resp_user_id = value; }
            get { return _resp_user_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? resp_dept_id
        {
            set { _resp_dept_id = value; }
            get { return _resp_dept_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? risk_type
        {
            set { _risk_type = value; }
            get { return _risk_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? risk_level
        {
            set { _risk_level = value; }
            get { return _risk_level; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? risk_outline_id
        {
            set { _risk_outline_id = value; }
            get { return _risk_outline_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string risk_detail
        {
            set { _risk_detail = value; }
            get { return _risk_detail; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int risk_store
        {
            set { _risk_store = value; }
            get { return _risk_store; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int menu_cat
        {
            set { _menu_cat = value; }
            get { return _menu_cat; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int menu_type
        {
            set { _menu_type = value; }
            get { return _menu_type; }
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
        public int audit_status
        {
            set { _audit_status = value; }
            get { return _audit_status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int audit_time
        {
            set { _audit_time = value; }
            get { return _audit_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int room_sign
        {
            set { _room_sign = value; }
            get { return _room_sign; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int leader_sign
        {
            set { _leader_sign = value; }
            get { return _leader_sign; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int need_fix_text
        {
            set { _need_fix_text = value; }
            get { return _need_fix_text; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int is_stress
        {
            set { _is_stress = value; }
            get { return _is_stress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int stress_trace
        {
            set { _stress_trace = value; }
            get { return _stress_trace; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int deal_status
        {
            set { _deal_status = value; }
            get { return _deal_status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string fix_user_id
        {
            set { _fix_user_id = value; }
            get { return _fix_user_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string risk_reason
        {
            set { _risk_reason = value; }
            get { return _risk_reason; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string risk_fix
        {
            set { _risk_fix = value; }
            get { return _risk_fix; }
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
