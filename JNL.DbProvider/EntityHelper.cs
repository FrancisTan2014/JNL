using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace JNL.DbProvider
{
    /// <summary>
    /// 将ado.net获取到的数据映射成实体的帮助类
    /// @谭光洪 2016-03-17
    /// </summary>
    public static class EntityHelper
    {
        #region 方法: 万能扩展( 数据类型转换 ) 但是需保证双方类型转换无误,否则会报错

        /// <summary>
        /// 万能扩展( 数据类型转换 ) 但是需保证双方类型转换无误,否则会报错
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="convertibleValue"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this IConvertible convertibleValue)
        {
            if (string.IsNullOrEmpty(convertibleValue.ToString()))
            {
                return default(T);
            }
            if (!typeof(T).IsGenericType)
            {
                return (T)Convert.ChangeType(convertibleValue, typeof(T));
            }
            else
            {
                Type genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return (T)Convert.ChangeType(convertibleValue, Nullable.GetUnderlyingType(typeof(T)));
                }
            }
            throw new InvalidCastException(string.Format("Invalid cast from type \"{0}\" to type \"{1}\".", convertibleValue.GetType().FullName, typeof(T).FullName));
        }
        #endregion

        #region 将一个SqlDataReader对象转换成一个实体类对象 +static TEntity MapEntity<TEntity>(SqlDataReader reader) where TEntity : class,new()
        /// <summary>
        /// 将一个SqlDataReader对象转换成一个实体类对象
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="reader">当前指向的reader</param>
        /// <returns>实体对象</returns>
        public static TEntity MapEntity<TEntity>(SqlDataReader reader) where TEntity : class,new()
        {
            try
            {
                var props = typeof(TEntity).GetProperties();
                var entity = new TEntity();
                foreach (var prop in props)
                {
                    if (prop.CanWrite)
                    {
                        try
                        {
                            //reader 中包含实体类中此属性的列 
                            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName='" + prop.Name + "'";

                            if (reader.GetSchemaTable().DefaultView.Count > 0)
                            {
                                var index = reader.GetOrdinal(prop.Name);
                                var data = reader.GetValue(index);
                                if (data != DBNull.Value)
                                {
                                    if (prop.PropertyType == typeof(int?))
                                    {
                                        int? temp = (int?)data;
                                        prop.SetValue(entity, temp.ConvertTo<int?>(), null);
                                    }
                                    else if (prop.PropertyType == typeof(decimal?))
                                    {
                                        decimal? temp = (decimal?)data;
                                        prop.SetValue(entity, temp.ConvertTo<decimal?>(), null);
                                    }
                                    else if (prop.PropertyType == typeof(DateTime?))
                                    {
                                        DateTime? temp = (DateTime?)data;
                                        prop.SetValue(entity, temp.ConvertTo<DateTime?>(), null);
                                    }
                                    else
                                    {
                                        if (data != DBNull.Value)
                                        {
                                            prop.SetValue(entity, Convert.ChangeType(data, prop.PropertyType), null);
                                        }
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            int i = 0;
                            continue;
                        }
                    }
                }
                return entity;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 将一个DataRow对象转换成一个实体类对象 +static TEntity MapEntity<TEntity>(DataRow dataRow) where TEntity : class,new()
        /// <summary>
        /// 将一个SqlDataReader对象转换成一个实体类对象
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="dataRow">当前指向的dataRow</param>
        /// <returns>实体对象</returns>
        public static TEntity MapEntity<TEntity>(DataRow dataRow) where TEntity : class,new()
        {
            try
            {

                if (dataRow == null)
                {
                    return null;
                }



                var entity = new TEntity();
                var props = typeof(TEntity).GetProperties();

                foreach (var prop in props)
                {
                    if (prop.CanWrite)
                    {
                        try
                        {
                            if (dataRow.Table.Columns.Contains(prop.Name))
                            {
                                var data = dataRow[prop.Name];
                                if (data != DBNull.Value)
                                {
                                    var newValue = SD_ChanageType(data, prop.PropertyType);
                                    prop.SetValue(entity, newValue, null);
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }

                return entity;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 将一个DataTable对象转换成一个实体类集合对象 +static List<TEntity> MapEntity<TEntity>(DataTable dataTable) where TEntity : class,new()

        /// <summary>
        /// 将一个DataTable对象转换成一个实体类集合对象
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="dataTable">当前指向的DataTable</param>
        /// <returns>实体对象</returns>
        public static IEnumerable<TEntity> MapEntity<TEntity>(DataTable dataTable) where TEntity : class,new()
        {
            List<TEntity> list = new List<TEntity>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (dataRow != null)
                    {
                        list.Add(MapEntity<TEntity>(dataRow));
                    }
                }
            }
            return list;
        }
        #endregion


        #region 方法: 万能类型转换

        /// <summary>
        /// 万能类型转换
        /// </summary>
        /// <param name="data"></param>
        /// <param name="convertsionType"></param>
        /// <returns></returns>
        public static object SD_ChanageType(object data, Type convertsionType)
        {
            //判断convertsionType类型是否为泛型，因为nullable是泛型类, //判断convertsionType是否为nullable泛型类
            if (convertsionType.IsGenericType && convertsionType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (data == null || data.ToString().Length == 0)
                {
                    return null;
                }
                //如果convertsionType为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                NullableConverter nullableConverter = new NullableConverter(convertsionType);
                //将convertsionType转换为nullable对的基础基元类型
                convertsionType = nullableConverter.UnderlyingType;
            }

            return Convert.ChangeType(data, convertsionType);

        }
        #endregion


        #region 将数据集合转换为DataTable +static DataTable ToDataTable<T>(this IEnumerable<T> data)
        /// <summary>
        /// 将数据集合转换为DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            var type = typeof (T);
            var properties = type.GetProperties();
            var table = new DataTable();
            var columnNames = new List<string>();

            foreach (var propertyInfo in properties)
            {
                columnNames.Add(propertyInfo.Name);
                table.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
            }

            foreach (var data in list)
            {
                var row = table.NewRow();
                columnNames.ForEach(c =>
                {
                    try
                    {
                        row[c] = type.InvokeMember(c, BindingFlags.GetProperty, null, data, null);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                });

                table.Rows.Add(row);
            }

            return table;
        }
        #endregion

    }
}
