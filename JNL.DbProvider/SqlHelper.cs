using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace JNL.DbProvider
{
    /// <summary>
    /// MSSQLSERVER数据库操作类，提供对指定数据库进行操作的方法
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-07-01</since>
    public class SqlHelper : DbHelper
    {
        public override void BulkInsert(string connectionString, string tableName, DataTable table)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }
            if (table == null || table.Rows.Count == 0)
            {
                return;
            }

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlBulkCopy bulk = new SqlBulkCopy(connection))
                    {
                        foreach (DataColumn column in table.Columns)
                        {
                            bulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                        
                        bulk.DestinationTableName = tableName;
                        bulk.WriteToServer(table);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                finally
                {
                    connection.Close();
                }
            }
        }

        public SqlHelper(DatabaseType dbType) : base(dbType)
        {
            
        }
    }
}
