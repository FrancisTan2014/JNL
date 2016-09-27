using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace JNL.DbProvider
{
    /// <summary>
    /// 数据库命令帮助类，提供创建数据库命令对象的方法
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-06-29</since>
    public static class DbCommandHelper
    {
        /// <summary>
        /// 根据数据库类型，创建不同的Command对象
        /// </summary>
        public static IDbCommand CreateCommand(DatabaseType dbType)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    return new SqlCommand();
                case DatabaseType.Oracle:
                    throw new ArgumentOutOfRangeException(nameof(dbType), dbType, "Oracle is not supported.");
                case DatabaseType.MySql:
                    return new MySqlCommand();
                default:
                    throw new ArgumentOutOfRangeException(nameof(dbType), dbType, "Unsupported database type");
            }
        }

        /// <summary>
        /// 创建一个SqlCommand命令对象
        /// </summary>
        /// <param name="dbType">所要创建的数据库连接对象的数据库类型</param>
        /// <param name="connection">一个有效的数据库连接对象</param>
        /// <param name="transaction">一个有效的数据事务对象</param>
        /// <param name="cmdType">数据库命令类型</param>
        /// <param name="cmdText">存储过程名称或者T-SQL语句</param>
        /// <param name="parameters">数据库参数数组</param>
        /// <returns>返回有效的SqlCommand对象</returns>
        public static IDbCommand CreateCommand(DatabaseType dbType, IDbConnection connection, IDbTransaction transaction, CommandType cmdType, string cmdText,
            IDataParameter[] parameters)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            IDbCommand command = CreateCommand(dbType);
            command.Connection = connection;
            command.CommandType = cmdType;
            command.CommandText = cmdText;

            if (transaction != null)
            {
                if (transaction.Connection == null)
                {
                    throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                }
                command.Transaction = transaction;
            }

            if (parameters != null)
            {
                foreach (var sqlParameter in parameters)
                {
                    command.Parameters.Add(sqlParameter);
                }
            }

            return command;

        }
    }
}
