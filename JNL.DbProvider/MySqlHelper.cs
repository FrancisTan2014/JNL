using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace JNL.DbProvider
{
    public class MySqlHelper : DbHelper
    {
        public MySqlHelper(DatabaseType dbType) : base(dbType)
        {
        }

        public override void BulkInsert(string connectionString, string tableName, DataTable table)
        {
            
        }
    }
}
