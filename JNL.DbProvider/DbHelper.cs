using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace JNL.DbProvider
{
    /// <summary>
    /// 提供一组方法，实现对数据库的各类操作
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-06-29</since>
    public abstract class DbHelper : IDbHelper
    {
        #region 私有字段，属性
        /// <summary>
        /// 标识当前所操作的数据库类型
        /// </summary>
        private readonly DatabaseType _dbType;

        /// <summary>
        /// 清除command中的参数（若检测到参数列表中有输出参数将不会执行清除操作）
        /// </summary>
        /// <param name="command">待清除的command对象</param>
        private void ClearCommandParameters(IDbCommand command)
        {
            var canClear = command.Parameters.Cast<IDataParameter>().All(commandParameter => commandParameter.Direction == ParameterDirection.Input);

            if (canClear)
            {
                command.Parameters.Clear();
            }
        } 
        #endregion

        #region 构造方法
        /// <summary>
        /// 根据指定的数据库类型创建本类的实例对象
        /// </summary>
        /// <param name="dbType">指定的数据库类型</param>
        protected DbHelper(DatabaseType dbType)
        {
            _dbType = dbType;
        }
        #endregion

        #region int ExecuteNonQuery 非查询命令

        /// <summary>
        /// 执行指定类型的数据库命令
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <returns>返回命令所影响的行数</returns>
        public int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText)
        {
            return ExecuteNonQuery(connectionString, cmdType, cmdText, null);
        }

        /// <summary>
        /// 执行指定类型的数据库命令
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的参数数组</param>
        /// <returns>返回命令所影响的行数</returns>
        public int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters)
        {
            using (var connection = DbConnectionHelper.CreateConnection(_dbType, connectionString))
            {
                return ExecuteNonQuery(connection, cmdType, cmdText, dbParameters);
            }
        }

        /// <summary>
        /// 执行指定数据库连接的命令
        /// </summary>
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <returns>返回命令所影响的行数</returns>
        public int ExecuteNonQuery(IDbConnection dbConnection, CommandType cmdType, string cmdText)
        {
            return ExecuteNonQuery(dbConnection, cmdType, cmdText, null);
        }

        /// <summary>
        /// 执行指定数据库连接的命令
        /// </summary>
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的参数数组</param>
        /// <returns>返回命令所影响的行数</returns>
        public int ExecuteNonQuery(IDbConnection dbConnection, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters)
        {
            if (dbConnection == null)
            {
                throw new ArgumentException("dbConnection");
            }

            var command = DbCommandHelper.CreateCommand(_dbType, dbConnection, null, cmdType, cmdText,
                dbParameters);
            var returnVal = command.ExecuteNonQuery();

            command.Parameters.Clear();
            return returnVal;
        }

        /// <summary>
        /// 执行带事务的数据库命令
        /// </summary>
        /// <param name="transaction">一个有效的数据库事务对象</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <returns>返回命令所影响的行数</returns>
        public int ExecuteNonQuery(IDbTransaction transaction, CommandType cmdType, string cmdText)
        {
            return ExecuteNonQuery(transaction, cmdType, cmdText, null);
        }

        /// <summary>
        /// 执行带事务的数据库命令
        /// </summary>
        /// <param name="transaction">一个有效的数据库事务对象</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的参数数组</param>
        /// <returns>返回命令所影响的行数</returns>
        public int ExecuteNonQuery(IDbTransaction transaction, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));

            var command = DbCommandHelper.CreateCommand(_dbType, transaction.Connection, transaction, cmdType, cmdText,
                dbParameters);
            var returnVal = command.ExecuteNonQuery();

            command.Parameters.Clear();
            return returnVal;

        }
        #endregion

        #region IDataReader ExecuteReader 数据阅读器

        /// <summary>
        /// 执行指定数据库连接字符串的数据阅读器
        /// </summary>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <returns>返回包含结果集的IDataReader对象</returns>
        public IDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText)
        {
            return ExecuteReader(connectionString, cmdType, cmdText, null);
        }

        /// <summary>
        /// 执行指定数据库连接字符串的数据阅读器
        /// </summary>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的数据库参数</param>
        /// <returns>返回包含结果集的IDataReader对象</returns>
        public IDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters)
        {
            using (var connection = DbConnectionHelper.CreateConnection(_dbType, connectionString))
            {
                return ExecuteReader(connection, cmdType, cmdText, dbParameters);
            }
        }

        /// <summary>
        /// 执行指定数据库连接对象的数据阅读器
        /// </summary>
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <returns>返回包含结果集的IDataReader对象</returns>
        public IDataReader ExecuteReader(IDbConnection dbConnection, CommandType cmdType, string cmdText)
        {
            return ExecuteReader(dbConnection, cmdType, cmdText, null);
        }

        /// <summary>
        /// 执行指定数据库连接字符串的数据阅读器
        /// </summary>
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的数据库参数</param>
        /// <returns>返回包含结果集的IDataReader对象</returns>
        public IDataReader ExecuteReader(IDbConnection dbConnection, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters)
        {
            if (dbConnection == null)
            {
                throw new ArgumentNullException(nameof(dbConnection));
            }

            var command = DbCommandHelper.CreateCommand(_dbType, dbConnection, null, cmdType, cmdText, dbParameters);
            var dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);

            ClearCommandParameters(command);

            return dataReader;
        }

        /// <summary>
        /// [调用者方式]执行指定数据库事务的数据阅读器,指定参数值.
        /// </summary>
        /// <param name="transaction">一个有效的数据库事务对象</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <returns>返回命令所影响的行数</returns>
        public IDataReader ExecuteReader(IDbTransaction transaction, CommandType cmdType, string cmdText)
        {
            return ExecuteReader(transaction, cmdType, cmdText, null);
        }

        /// <summary>
        /// [调用者方式]执行指定数据库事务的数据阅读器,指定参数.
        /// </summary>
        /// <param name="transaction">一个有效的数据库事务对象</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的参数数组</param>
        /// <returns>返回命令所影响的行数</returns>
        public IDataReader ExecuteReader(IDbTransaction transaction, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));
            }

            var command = DbCommandHelper.CreateCommand(_dbType, transaction.Connection, transaction, cmdType, cmdText, dbParameters);
            var dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);

            ClearCommandParameters(command);

            return dataReader;
        }
        #endregion

        #region object ExecuteScalar 返回结果集中的第一行第一列

        /// <summary> 
        /// 执行指定数据库连接字符串的命令,返回结果集中的第一行第一列. 
        /// </summary> 
        /// <param name="connectionString">一个有效的数据库连接字符串</param> 
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText)
        {
            return ExecuteScalar(connectionString, cmdType, cmdText, null);
        }

        /// <summary> 
        /// 执行指定数据库连接字符串的命令,返回结果集中的第一行第一列. 
        /// </summary> 
        /// <param name="connectionString">一个有效的数据库连接字符串</param> 
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的数据库参数数组</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters)
        {
            using (var connection = DbConnectionHelper.CreateConnection(_dbType, connectionString))
            {
                return ExecuteScalar(connection, cmdType, cmdText, dbParameters);
            }
        }

        /// <summary> 
        /// 执行指定数据库连接对象的命令,返回结果集中的第一行第一列. 
        /// </summary> 
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public object ExecuteScalar(IDbConnection dbConnection, CommandType cmdType, string cmdText)
        {
            return ExecuteScalar(dbConnection, cmdType, cmdText, null);
        }

        /// <summary> 
        /// 执行指定数据库连接对象的命令,返回结果集中的第一行第一列. 
        /// </summary> 
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的数据库参数数组</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public object ExecuteScalar(IDbConnection dbConnection, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters)
        {
            if (dbConnection == null)
            {
                throw new ArgumentNullException(nameof(dbConnection));
            }

            var command = DbCommandHelper.CreateCommand(_dbType, dbConnection, null, cmdType, cmdText, dbParameters);
            var result = command.ExecuteScalar();

            command.Parameters.Clear();

            return result;
        }

        /// <summary>
        /// 执行指定数据库事务的命令,返回结果集中的第一行第一列. 
        /// </summary>
        /// <param name="transaction">一个有效的连接事务</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public object ExecuteScalar(IDbTransaction transaction, CommandType cmdType, string cmdText)
        {
            return ExecuteScalar(transaction, cmdType, cmdText, null);
        }

        /// <summary>
        /// 执行指定数据库事务的命令,返回结果集中的第一行第一列. 
        /// </summary>
        /// <param name="transaction">一个有效的连接事务</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的数据库参数数组</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public object ExecuteScalar(IDbTransaction transaction, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));
            }

            var command = DbCommandHelper.CreateCommand(_dbType, transaction.Connection, transaction, cmdType, cmdText,
                dbParameters);
            var result = command.ExecuteScalar();

            command.Parameters.Clear();

            return result;
        }
        #endregion

        #region DataTable ExecuteDataTable 查询数据表格

        /// <summary>
        /// 根据指定的数据连接字符串查询数据表格
        /// </summary>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本，以及其它）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL语句</param>
        /// <returns>返回查询到的数据表格（若无数据，此表格不会为null）</returns>
        public DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText)
        {
            return ExecuteDataTable(connectionString, cmdType, cmdText, null);
        }

        /// <summary>
        /// 根据指定的数据连接字符串查询数据表格
        /// </summary>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本，以及其它）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需的数据库参数数组</param>
        /// <returns>返回查询到的数据表格（若无数据，此表格不会为null）</returns>
        public DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters)
        {
            using (var connection = DbConnectionHelper.CreateConnection(_dbType, connectionString))
            {
                return ExecuteDataTable(connection, cmdType, cmdText, dbParameters);
            }
        }

        /// <summary> 
        /// 根据指定的数据连接对象查询数据表格 
        /// </summary> 
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <returns>返回查询到的数据表格（若无数据，此表格不会为null）</returns>
        public DataTable ExecuteDataTable(IDbConnection dbConnection, CommandType cmdType, string cmdText)
        {
            return ExecuteDataTable(dbConnection, cmdType, cmdText, null);
        }

        /// <summary> 
        /// 根据指定的数据连接对象查询数据表格 
        /// </summary> 
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的数据库参数数组</param>
        /// <returns>返回查询到的数据表格（若无数据，此表格不会为null）</returns>
        public DataTable ExecuteDataTable(IDbConnection dbConnection, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters)
        {
            if (dbConnection == null)
            {
                throw new ArgumentNullException(nameof(dbConnection));
            }

            var command = DbCommandHelper.CreateCommand(_dbType, dbConnection, null, cmdType, cmdText, dbParameters);
            var dbAdpter = DbAdpterHelper.CreateAdapter(_dbType, command);
            
            var dataSet = new DataSet();
            dbAdpter.Fill(dataSet);

            return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
        }

        /// <summary>
        /// 执行指定数据库事务的命令,返回查询到的数据表格   
        /// </summary>
        /// <param name="transaction">一个有效的连接事务</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <returns>返回查询到的数据表格（若无数据，此表格不会为null）</returns>
        public DataTable ExecuteDataTable(IDbTransaction transaction, CommandType cmdType, string cmdText)
        {
            return ExecuteDataTable(transaction, cmdType, cmdText, null);
        }

        /// <summary>
        /// 执行指定数据库事务的命令,返回查询到的数据表格  
        /// </summary>
        /// <param name="transaction">一个有效的连接事务</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的数据库参数数组</param>
        /// <returns>返回查询到的数据表格（若无数据，此表格不会为null）</returns>
        public DataTable ExecuteDataTable(IDbTransaction transaction, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));
            }

            var command = DbCommandHelper.CreateCommand(_dbType, transaction.Connection, transaction, cmdType, cmdText,
                dbParameters);
            var dbAdpter = DbAdpterHelper.CreateAdapter(_dbType, command);

            var dataSet = new DataSet();
            dbAdpter.Fill(dataSet);

            return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
        }
        #endregion

        #region 查询实体模型

        /// <summary>
        /// 执行指定的命令，并将结果转换为指定的数据模型返回
        /// </summary>
        /// <typeparam name="T">要转换成的实体模型类型</typeparam>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="cmdType">命令类型（文本或存储过程）</param>
        /// <param name="cmdText">T-SQL语句或者存储过程名称</param>
        /// <returns>转换为指定数据模型后的结果集</returns>
        public IEnumerable<T> QueryModelFromDb<T>(string connectionString, CommandType cmdType, string cmdText)
            where T : class, new()
        {
            return QueryModelFromDb<T>(connectionString, cmdType, cmdText, null);
        }

        /// <summary>
        /// 执行指定的命令，并将结果转换为指定的数据模型返回
        /// </summary>
        /// <typeparam name="T">要转换成的实体模型类型</typeparam>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="cmdType">命令类型（文本或存储过程）</param>
        /// <param name="cmdText">T-SQL语句或者存储过程名称</param>
        /// <param name="dbParameters">执行命令所需的参数</param>
        /// <returns>转换为指定数据模型后的结果集</returns>
        public IEnumerable<T> QueryModelFromDb<T>(string connectionString, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters) where T : class, new()
        {
            var dataTable = ExecuteDataTable(connectionString, cmdType, cmdText, dbParameters);

            return EntityHelper.MapEntity<T>(dataTable);
        }
        #endregion

        #region BulkInsert 批量插入
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="tableName">待插入的表格名称</param>
        /// <param name="table">待插入的数据集合</param>
        public abstract void BulkInsert(string connectionString, string tableName, DataTable table);

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="T">待插入的数据实体类型</typeparam>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="tableName">待插入的表格名称</param>
        /// <param name="data">待插入的数据实体集合</param>
        public virtual void BulkInsert<T>(string connectionString, string tableName, IEnumerable<T> data)
            where T : class, new()
        {
            var table = data.ToDataTable();
            BulkInsert(connectionString, tableName, table);
        }
        #endregion
    }
}
