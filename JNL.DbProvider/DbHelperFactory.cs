using System;

namespace JNL.DbProvider
{
    /// <summary>
    /// 工厂类，提供根据数据库类型返回不同的数据库操作对象的方法
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-06-30</since>
    public static class DbHelperFactory
    {
        /// <summary>
        /// 根据指定的数据库类型返回不同
        /// </summary>
        /// <param name="dbType">目标数据库类型</param>
        /// <returns>与指定数据库类型相对应的数据库操作实体</returns>
        public static IDbHelper GetInstance(DatabaseType dbType)
        {
            // 此方法提供给调用者，目的是提供一种统一的获取dbHelper的方式
            // 即使DbHelper实现方式发生变化，对调用者来说是不用关心的
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    return new SqlHelper(dbType);
                case DatabaseType.Oracle:
                    return new OracleHelper(dbType);
                case DatabaseType.MySql:
                    return new MySqlHelper(dbType);
                default:
                    throw new ArgumentOutOfRangeException(nameof(dbType), dbType, "Unsupported database type.");
            }
        }
    }
}
