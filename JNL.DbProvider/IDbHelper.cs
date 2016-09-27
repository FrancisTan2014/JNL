using System.Collections.Generic;
using System.Data;

namespace JNL.DbProvider
{
    /// <summary>
    /// 定义一组通用数据操作规范
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-06-29</since>
    public interface IDbHelper
    {
        #region BulkInsert 批量插入

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="tableName">待插入的表格名称</param>
        /// <param name="table">待插入的数据集合</param>
        void BulkInsert(string connectionString, string tableName, DataTable table);

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="T">待插入的数据实体类型</typeparam>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="tableName">待插入的表格名称</param>
        /// <param name="data">待插入的数据实体集合</param>
        void BulkInsert<T>(string connectionString, string tableName, IEnumerable<T> data) where T : class,new();
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
        IEnumerable<T> QueryModelFromDb<T>(string connectionString, CommandType cmdType, string cmdText) where T : class,new(); 

        /// <summary>
        /// 执行指定的命令，并将结果转换为指定的数据模型返回
        /// </summary>
        /// <typeparam name="T">要转换成的实体模型类型</typeparam>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="cmdType">命令类型（文本或存储过程）</param>
        /// <param name="cmdText">T-SQL语句或者存储过程名称</param>
        /// <param name="dbParameters">执行命令所需的参数</param>
        /// <returns>转换为指定数据模型后的结果集</returns>
        IEnumerable<T> QueryModelFromDb<T>(string connectionString, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters) where T: class,new(); 
        #endregion

        #region int ExecuteNonQuery 非查询命令
        /// <summary>
        /// 执行指定类型的数据库命令
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <returns>返回命令所影响的行数</returns>
        int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText);

        /// <summary>
        /// 执行指定类型的数据库命令
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的参数数组</param>
        /// <returns>返回命令所影响的行数</returns>
        int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params IDataParameter[] dbParameters);

        /// <summary>
        /// 执行指定数据库连接的命令
        /// </summary>
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <returns>返回命令所影响的行数</returns>
        int ExecuteNonQuery(IDbConnection dbConnection, CommandType cmdType, string cmdText);

        /// <summary>
        /// 执行指定数据库连接的命令
        /// </summary>
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的参数数组</param>
        /// <returns>返回命令所影响的行数</returns>
        int ExecuteNonQuery(IDbConnection dbConnection, CommandType cmdType, string cmdText, params IDataParameter[] dbParameters);

        /// <summary>
        /// 执行带事务的数据库命令
        /// </summary>
        /// <param name="transaction">一个有效的数据库事务对象</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <returns>返回命令所影响的行数</returns>
        int ExecuteNonQuery(IDbTransaction transaction, CommandType cmdType, string cmdText);

        /// <summary>
        /// 执行带事务的数据库命令
        /// </summary>
        /// <param name="transaction">一个有效的数据库事务对象</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的参数数组</param>
        /// <returns>返回命令所影响的行数</returns>
        int ExecuteNonQuery(IDbTransaction transaction, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters); 
        #endregion

        #region IDataReader ExecuteReader 数据阅读器
        /// <summary>
        /// 执行指定数据库连接字符串的数据阅读器
        /// </summary>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <returns>返回包含结果集的IDataReader对象</returns>
        IDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText); 

        /// <summary>
        /// 执行指定数据库连接字符串的数据阅读器
        /// </summary>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的数据库参数</param>
        /// <returns>返回包含结果集的IDataReader对象</returns>
        IDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters);

        /// <summary>
        /// 执行指定数据库连接对象的数据阅读器
        /// </summary>
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <returns>返回包含结果集的IDataReader对象</returns>
        IDataReader ExecuteReader(IDbConnection dbConnection, CommandType cmdType, string cmdText);

        /// <summary>
        /// 执行指定数据库连接字符串的数据阅读器
        /// </summary>
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的数据库参数</param>
        /// <returns>返回包含结果集的IDataReader对象</returns>
        IDataReader ExecuteReader(IDbConnection dbConnection, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters);

        /// <summary>
        /// [调用者方式]执行指定数据库事务的数据阅读器,指定参数值.
        /// </summary>
        /// <param name="transaction">一个有效的数据库事务对象</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <returns>返回命令所影响的行数</returns>
        IDataReader ExecuteReader(IDbTransaction transaction, CommandType cmdType, string cmdText);

        /// <summary>
        /// [调用者方式]执行指定数据库事务的数据阅读器,指定参数.
        /// </summary>
        /// <param name="transaction">一个有效的数据库事务对象</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本以及其它）</param>
        /// <param name="cmdText">存储过程名称或者SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的参数数组</param>
        /// <returns>返回命令所影响的行数</returns>
        IDataReader ExecuteReader(IDbTransaction transaction, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters); 
        #endregion
        
        #region object ExecuteScalar 返回结果集中的第一行第一列
        /// <summary> 
        /// 执行指定数据库连接字符串的命令,返回结果集中的第一行第一列. 
        /// </summary> 
        /// <param name="connectionString">一个有效的数据库连接字符串</param> 
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <returns>返回结果集中的第一行第一列</returns> 
        object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText);

        /// <summary> 
        /// 执行指定数据库连接字符串的命令,返回结果集中的第一行第一列. 
        /// </summary> 
        /// <param name="connectionString">一个有效的数据库连接字符串</param> 
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的数据库参数数组</param>
        /// <returns>返回结果集中的第一行第一列</returns> 
        object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters);

        /// <summary> 
        /// 执行指定数据库连接对象的命令,返回结果集中的第一行第一列. 
        /// </summary> 
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <returns>返回结果集中的第一行第一列</returns> 
        object ExecuteScalar(IDbConnection dbConnection, CommandType cmdType, string cmdText);

        /// <summary> 
        /// 执行指定数据库连接对象的命令,返回结果集中的第一行第一列. 
        /// </summary> 
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的数据库参数数组</param>
        /// <returns>返回结果集中的第一行第一列</returns> 
        object ExecuteScalar(IDbConnection dbConnection, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters);

        /// <summary>
        /// 执行指定数据库事务的命令,返回结果集中的第一行第一列. 
        /// </summary>
        /// <param name="transaction">一个有效的连接事务</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        object ExecuteScalar(IDbTransaction transaction, CommandType cmdType, string cmdText); 

        /// <summary>
        /// 执行指定数据库事务的命令,返回结果集中的第一行第一列. 
        /// </summary>
        /// <param name="transaction">一个有效的连接事务</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的数据库参数数组</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        object ExecuteScalar(IDbTransaction transaction, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters); 
        #endregion

        #region DataTable ExecuteDataTable 查询数据表格
        /// <summary>
        /// 根据指定的数据连接字符串查询数据表格
        /// </summary>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本，以及其它）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL语句</param>
        /// <returns>返回查询到的数据表格（若无数据，此表格不会为null）</returns>
        DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText);

        /// <summary>
        /// 根据指定的数据连接字符串查询数据表格
        /// </summary>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="cmdType">数据库命令类型（存储过程、文本，以及其它）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需的数据库参数数组</param>
        /// <returns>返回查询到的数据表格（若无数据，此表格不会为null）</returns>
        DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText, params IDataParameter[] dbParameters);

        /// <summary> 
        /// 根据指定的数据连接对象查询数据表格 
        /// </summary> 
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <returns>返回查询到的数据表格（若无数据，此表格不会为null）</returns> 
        DataTable ExecuteDataTable(IDbConnection dbConnection, CommandType cmdType, string cmdText);

        /// <summary> 
        /// 根据指定的数据连接对象查询数据表格 
        /// </summary> 
        /// <param name="dbConnection">一个有效的数据库连接对象</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的数据库参数数组</param>
        /// <returns>返回查询到的数据表格（若无数据，此表格不会为null）</returns> 
        DataTable ExecuteDataTable(IDbConnection dbConnection, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters);

        /// <summary>
        /// 执行指定数据库事务的命令,返回查询到的数据表格   
        /// </summary>
        /// <param name="transaction">一个有效的连接事务</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <returns>返回查询到的数据表格（若无数据，此表格不会为null）</returns>
        DataTable ExecuteDataTable(IDbTransaction transaction, CommandType cmdType, string cmdText);

        /// <summary>
        /// 执行指定数据库事务的命令,返回查询到的数据表格  
        /// </summary>
        /// <param name="transaction">一个有效的连接事务</param>
        /// <param name="cmdType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <param name="dbParameters">执行命令所需要的数据库参数数组</param>
        /// <returns>返回查询到的数据表格（若无数据，此表格不会为null）</returns>
        DataTable ExecuteDataTable(IDbTransaction transaction, CommandType cmdType, string cmdText,
            params IDataParameter[] dbParameters); 
        #endregion
    }
}
