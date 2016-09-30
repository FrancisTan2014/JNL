﻿

/*
 * 本文件使用代码生成器生成，随时有可能被更改
 * 若要添加新逻辑请在其他地方新建partial类
 */

using JNL.Model;

namespace JNL.Dal  

{
	/// <summary>
    /// Accident表的数据访问层实现
    /// </summary>
    /// <remarks>动软生成于2016-09-22</remarks>
	public partial class AccidentDal : BaseDal<Accident>
	{
		/// <summary>
        /// 在派生类中重写时，表示当前操作的表名称
        /// </summary>
		protected override string TableName { get { return "Accident"; } }
		
		/// <summary>
        /// 在派生类中重写时，表示当前表的主键名称
        /// </summary>
		protected override string PrimaryKeyName { get { return "Id"; } }		
	}
}