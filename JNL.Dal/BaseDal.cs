using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Transactions;
using JNL.DbProvider;

namespace JNL.Dal
{
    public abstract class BaseDal<T> where T : class,new()
    {
        /// <summary>
        /// 在派生类中重写时，表示当前操作的表名称
        /// </summary>
        protected abstract string TableName { get; }

        /// <summary>
        /// 在派生类中重写时，表示当前表的主键名称
        /// </summary>
        protected abstract string PrimaryKeyName { get; }

        /// <summary>
        /// 基本条件（若实例中有IsDelete字段，则基本条件为IsDelete=0，否则为1=1）
        /// </summary>
        protected string BaseCondition { get; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected virtual string ConnectionString
        {
            get
            {
                var conStr = ConfigurationManager.ConnectionStrings["MainDb"];
                if (conStr == null)
                {
                    throw new ConfigurationErrorsException("未能获取到主数据库连接字符串MainDb");
                }

                return conStr.ToString();
            }
        }

        /// <summary>
        /// 数据库操作对象
        /// </summary>
        protected IDbHelper DbHelper { get; set; }

        /// <summary>
        /// 默认构造方法
        /// </summary>
        protected BaseDal()
        {
            DbHelper = DbHelperFactory.GetInstance(DatabaseType.SqlServer);

            try
            {
                var modelType = typeof(T);
                var isDelete = modelType.GetProperty("IsDelete");
                BaseCondition = isDelete == null ? "1=1" : "IsDelete=0";
            }
            catch (Exception)
            {
                BaseCondition = "1=1";
            }
        }

        /// <summary>
        /// 执行sql语句，返回DataTable
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string cmdText)
        {
            if (string.IsNullOrEmpty(cmdText))
            {
                throw new ArgumentNullException(nameof(cmdText));
            }

            return DbHelper.ExecuteDataTable(ConnectionString, CommandType.Text, cmdText);
        }

        /// <summary>
        /// 执行sql语句，返回指定类型的模型集合
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public IEnumerable<TSource> ExecuteModel<TSource>(string cmdText) where TSource: class, new()
        {
            if (string.IsNullOrEmpty(cmdText))
            {
                throw new ArgumentNullException(nameof(cmdText));
            }

            var table = DbHelper.ExecuteDataTable(ConnectionString, CommandType.Text, cmdText);

            return EntityHelper.MapEntity<TSource>(table);
        }

        /// <summary>
        /// 判断指定类型是否是自定义类型
        /// </summary>
        /// <param name="type">指定的类型</param>
        /// <returns>是自定义类型返回<c>true</c>否则返回<c>false</c></returns>
        protected bool IsReferenceTypeButNotString(Type type)
        {
            return !type.IsValueType && type != typeof(string);
        }

        /// <summary>
        /// 在插入数据库之前处理属性的默认值问题（如DateTime类型及string类型的默认值），这些类型以默认插入数据库是会报错
        /// </summary>
        /// <param name="propertyInfo">待处理的属性信息对象</param>
        /// <param name="value">属性值</param>
        /// <returns>返回处理过后的属性值</returns>
        protected object DealDefaultValue(PropertyInfo propertyInfo, object value)
        {
            if (propertyInfo.PropertyType == typeof(DateTime) && value.Equals(default(DateTime)))
            {
                value = new DateTime(1900, 1, 1);
            }
            else if (propertyInfo.PropertyType == typeof(string) && value == null)
            {
                value = string.Empty;
            }

            return value;
        }

        /// <summary>
        /// 向数据库中插入新的数据，插入成功会将新的主键id记录在实体中返回
        /// </summary>
        /// <param name="model">待插入的实体对象</param>
        /// <param name="fields"></param>
        /// <returns>带有插入数据的主键id的实体对象</returns>
        public virtual T Insert(T model, string[] fields = null)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var propNameList = new List<string>();
            var paraNameList = new List<string>();
            var sqlParamList = new List<SqlParameter>();

            PropertyInfo primaryKeyProp = null;
            foreach (var propertyInfo in properties)
            {
                var propName = propertyInfo.Name;
                if (propName == PrimaryKeyName)
                {
                    primaryKeyProp = propertyInfo;
                    continue;
                }

                if (fields != null && !fields.Contains(propName))
                {
                    continue;
                }

                var propType = propertyInfo.PropertyType;
                if (!IsReferenceTypeButNotString(propType) && propType.Name != PrimaryKeyName)
                {
                    var propValue = propertyInfo.GetValue(model, null);
                    propValue = DealDefaultValue(propertyInfo, propValue);

                    sqlParamList.Add(new SqlParameter
                    {
                        ParameterName = "@" + propName,
                        Value = propValue
                    });
                    propNameList.Add($"[{propName}]");
                    paraNameList.Add("@" + propName);
                }
            }

            if (primaryKeyProp == null)
            {
                return model;
            }

            var outParamName = "@__identity";
            var outParamSqlType = SqlDbType.Int;
            if (primaryKeyProp.PropertyType == typeof(long))
            {
                outParamSqlType = SqlDbType.BigInt;
            }
            else if (primaryKeyProp.PropertyType == typeof(string))
            {
                outParamSqlType = SqlDbType.NVarChar;
            }

            var outIdentity = new SqlParameter
            {
                ParameterName = outParamName,
                Direction = ParameterDirection.Output,
                SqlDbType = outParamSqlType
            };
            sqlParamList.Add(outIdentity);

            var propNames = string.Join(",", propNameList);
            var paramNames = string.Join(",", paraNameList);
            var cmdText =
                $"INSERT INTO {TableName}({propNames}) VALUES({paramNames}) SELECT {outParamName}=SCOPE_IDENTITY()";

            var count = DbHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText, sqlParamList.ToArray());

            if (count > 0)
            {
                type.InvokeMember(primaryKeyProp.Name, BindingFlags.SetProperty, null, model,
                    new[] { outIdentity.Value });
            }

            return model;
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="list">待插入的数据集合</param>
        public virtual void BulkInsert(IEnumerable<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException();
            }

            DbHelper.BulkInsert(ConnectionString, TableName, list);
        }

        /// <summary>
        /// 更新指定数据
        /// </summary>
        /// <param name="model">待更新的数据实体</param>
        /// <param name="fields">指定需要更新的字段，若为null则默认更新所有字段</param>
        /// <returns>操作成功返回<c>true</c>，否则返回<c>false</c></returns>
        public virtual bool Update(T model, string[] fields = null)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var paraNameList = new List<string>();
            var sqlParamList = new List<SqlParameter>();

            object primaryKeyValue = null;
            foreach (var propertyInfo in properties)
            {
                var propName = propertyInfo.Name;
                if (propName != PrimaryKeyName && null != fields && !fields.Contains(propName))
                {
                    continue;
                }

                var propType = propertyInfo.PropertyType;
                if (!IsReferenceTypeButNotString(propType))
                {
                    var propValue = propertyInfo.GetValue(model, null);
                    if (propName == PrimaryKeyName)
                    {
                        primaryKeyValue = propValue;
                    }
                    else
                    {
                        propValue = DealDefaultValue(propertyInfo, propValue);

                        sqlParamList.Add(new SqlParameter
                        {
                            ParameterName = "@" + propName,
                            Value = propValue
                        });
                        paraNameList.Add(string.Format("[{0}]=@{0}", propName));
                    }
                }
            }

            if (null == primaryKeyValue)
            {
                return false;
            }

            var paramNames = string.Join(",", paraNameList);
            var cmdText = $"UPDATE {TableName} SET {paramNames} WHERE {PrimaryKeyName}={primaryKeyValue}";

            return DbHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText, sqlParamList.ToArray()) > 0;
        }

        /// <summary>
        /// 根据主键id删除指定数据
        /// </summary>
        /// <param name="id">主键id值</param>
        /// <returns>操作成功返回<c>true</c>，否则返回<c>false</c></returns>
        public virtual bool Delete(object id)
        {
            var cmdText = $"DELETE {TableName} WHERE {PrimaryKeyName}={id}";

            return DbHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText) > 0;
        }

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="condition">删除条件，它不能为空</param>
        /// <returns>表示删除是否成功的布尔值</returns>
        public virtual bool Delete(string condition)
        {
            if (string.IsNullOrEmpty(condition) || Regex.Replace(condition, "\\s+", "") == "1=1")
            {
                throw new ArgumentException("条件为空或为1=1，删除操作被阻止");
            }

            var cmdText = $"DELETE {TableName} WHERE {condition}";

            return DbHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText) > 0;
        }

        /// <summary>
        /// 根据主键id将指定数据软删除（将记录删除状态的字段值置为1）
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="softDeleteFieldName">记录删除状态的字段名称，默认为IsDelete</param>
        /// <returns></returns>
        public virtual bool DeleteSoftly(object id, string softDeleteFieldName = "IsDelete")
        {
            var cmdText = $"UPDATE {TableName} SET {softDeleteFieldName}=1 WHERE {PrimaryKeyName}={id}";

            return DbHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText) > 0;
        }

        /// <summary>
        /// 根据条件将指定的数据软删除
        /// </summary>
        /// <param name="condition">删除条件，不能为空</param>
        /// <param name="softDeleteFieldName">记录删除状态的字段名称，默认为IsDelete</param>
        /// <returns>操作成功与否的布尔值</returns>
        public virtual bool DeleteSoftly(string condition, string softDeleteFieldName = "IsDelete")
        {
            var cmdText = $"UPDATE {TableName} SET {softDeleteFieldName}=1 WHERE {condition}";

            return DbHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText) > 0;
        }

        /// <summary>
        /// 根据主键id查询单个实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="queryFields">指定要查询的部分列名称，此参数为空则默认查询所有列</param>
        /// <returns>查询到数据则返回实体对象，否则返回null</returns>
        public virtual T QuerySingle(object id, string[] queryFields = null)
        {
            var fieldStr = queryFields == null ? "*" : string.Join(",", queryFields);
            var cmdText = $"SELECT {fieldStr} FROM {TableName} WHERE {PrimaryKeyName}={id} AND {BaseCondition}";
            var modelList = DbHelper.QueryModelFromDb<T>(ConnectionString, CommandType.Text, cmdText);

            return modelList.FirstOrDefault();
        }

        /// <summary>
        /// 根据条件查询单个实体
        /// </summary>
        /// <param name="whereStr">查询数据的where条件，不需要带有where关键字，只需要写出条件即可，如：name='test'，此参数为空时，默认查询所有数据</param>
        /// <param name="queryFields">指定要查询的部分列名称，此参数为空则默认查询所有列</param>
        /// <param name="paramDic">动态参数字典，键为字段名称，值为参数值，若有动态参数则参数名需要带有@字符</param>
        /// <returns>查询到数据则返回实体对象，否则返回null</returns>
        public virtual T QuerySingle(string whereStr, string[] queryFields = null, IDictionary<string, object> paramDic = null)
        {
            if (string.IsNullOrEmpty(whereStr))
            {
                throw new ArgumentNullException(nameof(whereStr));
            }

            return QueryList(whereStr, queryFields, paramDic).FirstOrDefault();
        }

        /// <summary>
        /// 根据条件查询数据集合，请注意在调用此方法的时候，如果条件带有动态参数，请确保whereStr中的参数名与参数字段中的名称一致，如：
        /// whereStr="name=@name"，则paramDic中相应参数的键应该为@name
        ///     调用示例：
        ///         查询所有数据：QueryList();
        ///         查询部分数据：QueryList("name=test", new [] {"Id", "Name"});
        ///         查询部分数据（带参数：
        ///             QueryList("name=@name", 
        ///                        new [] {"Id", "Name"}, 
        ///                        new Dictionary string, object {{"@name", "test"}},
        ///                        "Id", 
        ///                        true);
        /// </summary>
        /// <param name="whereStr">查询数据的where条件，不需要带有where关键字，只需要写出条件即可，如：name='test'，此参数为空时，默认查询所有数据</param>
        /// <param name="fields">指示查询的数据列的名称，带有此参数可避免select * 语句的低效查询，若此参数为空则默认查询所有列</param>
        /// <param name="paramDic">动态参数字典，键为字段名称，值为参数值，若有动态参数则参数名需要带有@字符</param>
        /// <param name="orderField">查询数据的排序字段名称</param>
        /// <param name="isDescending">指示是否对查询结果降序排序</param>
        /// <returns>返回查询到的实体集合</returns>
        public virtual IEnumerable<T> QueryList(string whereStr = null, string[] fields = null, IDictionary<string, object> paramDic = null, string orderField = null,
            bool isDescending = false)
        {
            var cmdText = PrepareSql(whereStr, fields, orderField, isDescending);

            var sqlParams = new List<SqlParameter>();
            if (paramDic != null)
            {
                sqlParams.AddRange(paramDic.Select(
                    keyValuePair => new SqlParameter(
                        keyValuePair.Key.StartsWith("@") ? keyValuePair.Key : "@" + keyValuePair.Key,
                        keyValuePair.Value)
                    ));
            }

            return DbHelper.QueryModelFromDb<T>(ConnectionString, CommandType.Text, cmdText, sqlParams.ToArray());
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">每页数据个数</param>
        /// <param name="whereStr">查询数据的where条件，不需要带有where关键字，只需要写出条件即可，如：name='test'，此参数为空时，默认查询所有数据</param>
        /// <param name="fields">指示查询的数据列的名称，带有此参数可避免select * 语句的低效查询，若此参数为空则默认查询所有列</param>
        /// <param name="paramDic">查询数据的参数字典，键为字段名称，值为参数值，若有动态参数则参数名需要带有@字符</param>
        /// <param name="orderField">查询数据的排序字段名称</param>
        /// <param name="isDescending">指示是否对查询结果降序排序</param>
        /// <returns>返回查询到的实体集合</returns>
        public virtual IEnumerable<T> QueryPageList(int pageIndex, int pageSize, string whereStr, string[] fields = null,
            IDictionary<string, object> paramDic = null, string orderField = null,
            bool isDescending = false)
        {
            if (pageIndex <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            var cmdText = PrepareSql(whereStr, fields, orderField, isDescending, pageIndex, pageSize);
            var sqlParams = new List<SqlParameter>();
            if (paramDic != null)
            {
                sqlParams.AddRange(paramDic.Select(keyValuePair => new SqlParameter(keyValuePair.Key, keyValuePair.Value)));
            }

            return DbHelper.QueryModelFromDb<T>(ConnectionString, CommandType.Text, cmdText, sqlParams.ToArray());
        }

        /// <summary>
        /// 获取根据指定条件查询本表将得到的结果集数量
        /// </summary>
        /// <param name="whereStr">指定条件</param>
        /// <returns>总数量</returns>
        public virtual int GetTotalCount(string whereStr = null)
        {
            var condition = string.IsNullOrEmpty(whereStr) ? BaseCondition : $"{whereStr} AND {BaseCondition}";
            var cmdText = $"SELECT COUNT(1) FROM {TableName} WHERE {condition}";
            return (int) DbHelper.ExecuteScalar(ConnectionString, CommandType.Text, cmdText);
        }

        /// <summary>
        /// 获取当前表的最大主键值
        /// </summary>
        /// <returns>当前表的最大主键值</returns>
        public virtual object GetMaxId()
        {
            var cmdText = $"SELECT MAX(Id) FROM {TableName}";
            var maxId = DbHelper.ExecuteScalar(ConnectionString, CommandType.Text, cmdText);
            if (maxId.Equals(DBNull.Value))
            {
                return 0;
            }

            return maxId;
        }

        /// <summary>
        /// 判断指定条件的数据是否存在
        /// </summary>
        /// <param name="condition">判断条件</param>
        /// <returns>表示是否存在的布尔值</returns>
        public virtual bool Exists(string condition)
        {
            condition = string.IsNullOrEmpty(condition) ? BaseCondition : $"{condition} AND {BaseCondition}";
            var cmdText = $"SELECT COUNT(1) FROM {TableName} WHERE {condition}";

            var res = DbHelper.ExecuteScalar(ConnectionString, CommandType.Text, cmdText);

            return (int) res > 0;
        }

        /// <summary>
        /// 根据条件查询表中的指定列，并返回这一列的数据集合，若查询不到数据则返回空集合(而非null)
        /// </summary>
        /// <param name="whereStr">查询条件</param>
        /// <param name="fieldName">要查询的列名</param>
        /// <returns>查询到的数据集合或者空集合（非null）</returns>
        public virtual IEnumerable<object> QuerySingleColumn(string whereStr, string fieldName)
        {
            var list = QueryList(whereStr, new[] {fieldName});

            var enumerable = list as T[] ?? list.ToArray();
            if (!enumerable.Any())
            {
                return new List<object>();
            }

            try
            {
                return enumerable.Select(entity =>
                {
                    var type = entity.GetType();
                    return type.InvokeMember(fieldName, BindingFlags.GetProperty, null, entity, null);
                });
            }
            catch (Exception)
            {
                return new List<object>();
            }
        }

        /// <summary>
        /// 将多个执行数据库操作的委托封装到一个事务环境中，确保所有的委托都被正确执行，否则将执行回滚操作
        /// </summary>
        /// <param name="delegates">待执行的委托集合</param>
        public virtual bool ExecuteTransation(params Func<bool>[] delegates)
        {
            if (delegates == null)
            {
                throw new ArgumentNullException(nameof(delegates));
            }

            using (var scope = new TransactionScope())
            {
                try
                {
                    var result = false;
                    foreach (var func in delegates)
                    {
                        result = func();
                    }

                    if (result)
                    {
                        scope.Complete();
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }            
        }

        /// <summary>
        /// 为查询方法准备sql语句
        /// </summary>
        /// <param name="whereStr">查询的where条件（条件开头与结尾不需要带AND）</param>
        /// <param name="fields">查询的列名称</param>
        /// <param name="orderField">排序字段名称</param>
        /// <param name="isDescending">是否对结果进行降序排序</param>
        /// <param name="pageIndex">分布查询的页码</param>
        /// <param name="pageSize">分布查询每页长度</param>
        /// <returns>按照规则拼接好的sql语句</returns>
        private string PrepareSql(string whereStr, string[] fields, string orderField, bool isDescending, int pageIndex = 0, int pageSize = 0)
        {
            var selectFileds = fields == null ? "*" : string.Join(",", fields);
            var orderStr = string.IsNullOrEmpty(orderField)
                ? $"{PrimaryKeyName}"
                : $"{orderField} {(isDescending ? "DESC" : "ASC")}";

            string condition = string.IsNullOrEmpty(whereStr) 
                ? BaseCondition 
                : $"{whereStr} AND {BaseCondition}";

            string sql;
            if (pageIndex == 0 || pageSize == 0)
            {
                sql = $"SELECT {selectFileds} FROM {TableName} WHERE {condition} ORDER BY {orderStr}";
            }
            else
            {
                sql = $"SELECT {selectFileds} FROM (SELECT {selectFileds},ROW_NUMBER() OVER(ORDER BY {orderStr}) AS RowNumber FROM {TableName} WHERE {condition}) AS NEWTABLE WHERE RowNumber BETWEEN ({pageIndex}-1)*{pageSize}+1 AND {pageIndex}*{pageSize}";
            }

            return sql;
        }
    }
}
