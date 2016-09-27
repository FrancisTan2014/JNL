using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace JNL.DbProvider
{
    /// <summary>
    /// 数据库命令帮助类，提供创建DbAdpter对象的方法
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-06-29</since>
    public static class DbAdpterHelper
    {
        /// <summary>
        /// 创建与指定数据库类型想对应的DbDataAdapter对象
        /// </summary>
        /// <param name="dbType">指定的数据库类型</param>
        /// <param name="command">数据库命令对象</param>
        /// <returns>一个有效的DbDataAdapter的派生类对象</returns>
        public static IDbDataAdapter CreateAdapter(DatabaseType dbType, IDbCommand command)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    return new SqlDataAdapter(command as SqlCommand);
                case DatabaseType.Oracle:
                    throw new ArgumentOutOfRangeException(nameof(dbType), dbType, "Oracle is not supported.");
                case DatabaseType.MySql:
                    return new MySqlDataAdapter(command as MySqlCommand);
                default:
                    throw new ArgumentOutOfRangeException(nameof(dbType), dbType, "Unsupported database type.");

            }
        }
    }
}
