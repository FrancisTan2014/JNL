namespace JNL.Utilities.Enums
{
    /// <summary>
    /// 描述数据库操作类型（CRUD）的枚举类型
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-07-28</since>
    public enum DbOperateType
    {
        /// <summary>
        /// 插入操作
        /// </summary>
        Insert = 1,

        /// <summary>
        /// 批量插入操作
        /// </summary>
        BulkInsert = 2,

        /// <summary>
        /// 更新操作
        /// </summary>
        Update = 3,

        /// <summary>
        /// 删除操作
        /// </summary>
        Delete = 4,

        /// <summary>
        /// 查询单条数据
        /// </summary>
        QuerySingle = 5,

        /// <summary>
        /// 查询数据集合
        /// </summary>
        QueryList = 6,

        /// <summary>
        /// 分页查询数据
        /// </summary>
        QueryPageList = 7
    }
}
