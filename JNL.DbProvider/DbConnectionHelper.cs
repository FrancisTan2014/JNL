using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace JNL.DbProvider
{
    /// <summary>
    /// 数据库连接帮助类，提供根据指定数据库类型创建数据库连接对象的方法
    /// </summary>
    public static class DbConnectionHelper
    {
        /// <summary>
        /// 所要创建数据库连接对象的数据库类型
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="conStr">一个有效的数据库连接字符串</param>
        /// <returns>一个有效的数据库连接对象</returns>
        public static IDbConnection CreateConnection(DatabaseType dbType, string conStr)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    return new SqlConnection(conStr);
                case DatabaseType.Oracle:
                    throw new ArgumentOutOfRangeException(nameof(dbType), dbType, "Oracle is not supported.");
                case DatabaseType.MySql:
                    return new MySqlConnection(conStr);
                default:
                    throw new ArgumentOutOfRangeException(nameof(dbType), dbType, "Unsupported database type.");

            }
        }
    }
}
