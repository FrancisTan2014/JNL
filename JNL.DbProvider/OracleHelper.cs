using System;
using System.Collections.Generic;

namespace JNL.DbProvider
{
    public class OracleHelper : DbHelper
    {
        public override void BulkInsert(string connectionString, string tableName, System.Data.DataTable table)
        {
            throw new NotImplementedException();
        }

        public override void BulkInsert<T>(string connectionString, string tableName, IEnumerable<T> data)
        {
            throw new NotImplementedException();
        }

        public OracleHelper(DatabaseType dbType) : base(dbType)
        {

        }
    }
}
