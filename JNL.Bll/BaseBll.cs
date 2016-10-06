using System;
using System.Collections.Generic;
using JNL.Dal;
using JNL.Utilities.Helpers;

namespace JNL.Bll
{
    /// <summary>
    /// 业务逻辑层基类，提供对数据库基本的CRUD操作
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-07-14</since>
    public abstract class BaseBll<T> where T : class,new()
    {
        /// <summary>
        /// 当前操作表的Dal层实例
        /// </summary>
        public BaseDal<T> DalInstance { get; }

        /// <summary>
        /// 默认构造方法，初始化dal层实例等
        /// </summary>
        protected BaseBll()
        {
            DalInstance = DalFactory.CreateInstance<T>();
        }

        /// <summary>
        /// 判断指定条件的数据是否存在
        /// </summary>
        /// <param name="condition">判断条件（当条件为null或者''时，可判断此表是否有数据）</param>
        /// <returns>表示是否存在的布尔值</returns>
        public bool Exists(string condition = null)
        {
            return DalInstance.Exists(condition);
        }

        /// <summary>
        /// 获取当前表格的最大主键值
        /// </summary>
        /// <returns></returns>
        public object GetMaxId()
        {
            return DalInstance.GetMaxId();
        }

        /// <summary>
        /// 向数据库中插入新的数据，插入成功会将新的主键id记录在实体中返回
        /// </summary>
        /// <param name="json">待插入的数据的json字符串表示</param>
        /// <param name="fields"></param>
        /// <returns>带有插入数据的主键id的实体对象</returns>
        public T Insert(string json, string[] fields = null)
        {
            var model = JsonHelper.Deserialize<T>(json);
            if (model == null)
            {
                return null;
            }

            return Insert(model, fields);
        }

        /// <summary>
        /// 向数据库中插入新的数据，插入成功会将新的主键id记录在实体中返回
        /// </summary>
        /// <param name="model">待插入的实体对象</param>
        /// <param name="fields"></param>
        /// <returns>带有插入数据的主键id的实体对象</returns>
        public T Insert(T model, string[] fields = null)
        {
            return DalInstance.Insert(model, fields);
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="json">待插入的数据集的json表示</param>
        public void BulkInsert(string json)
        {
            var list = JsonHelper.Deserialize<IEnumerable<T>>(json);
            BulkInsert(list);
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="list">待插入的数据集合</param>
        public void BulkInsert(IEnumerable<T> list)
        {
            DalInstance.BulkInsert(list);
        }

        /// <summary>
        /// 更新指定数据
        /// </summary>
        /// <param name="json">待更新的数据的json字符串表示</param>
        /// <param name="fields">指定需要更新的字段，若为null则默认更新所有字段</param>
        /// <returns>操作成功返回<c>true</c>，否则返回<c>false</c></returns>
        public bool Update(string json, string[] fields = null)
        {
            var model = JsonHelper.Deserialize<T>(json);
            if (model == null)
            {
                return false;
            }

            return Update(model, fields);
        }

        /// <summary>
        /// 更新指定数据
        /// </summary>
        /// <param name="model">待更新的数据实体</param>
        /// <param name="fields">指定需要更新的字段，若为null则默认更新所有字段</param>
        /// <returns>操作成功返回<c>true</c>，否则返回<c>false</c></returns>
        public bool Update(T model, string[] fields = null)
        {
            return DalInstance.Update(model, fields);
        }

        /// <summary>
        /// 根据主键id删除指定数据
        /// </summary>
        /// <param name="id">主键id值</param>
        /// <returns>操作成功返回<c>true</c>，否则返回<c>false</c></returns>
        public bool Delete(object id)
        {
            return DalInstance.Delete(id);
        }

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="condition">删除条件，它不能为空</param>
        /// <returns>表示删除是否成功的布尔值</returns>
        public bool Delete(string condition)
        {
            return DalInstance.Delete(condition);
        }

        /// <summary>
        /// 根据主键id将指定数据软删除（将记录删除状态的字段值置为1）
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="softDeleteFieldName">记录删除状态的字段名称，默认为IsDelete</param>
        /// <returns></returns>
        public bool DeleteSoftly(object id, string softDeleteFieldName = "IsDelete")
        {
            return DalInstance.DeleteSoftly(id, softDeleteFieldName);
        }

        /// <summary>
        /// 根据条件将指定的数据软删除
        /// </summary>
        /// <param name="condition">删除条件，不能为空</param>
        /// <param name="softDeleteFieldName">记录删除状态的字段名称，默认为IsDelete</param>
        /// <returns>操作成功与否的布尔值</returns>
        public bool DeleteSoftly(string condition, string softDeleteFieldName = "IsDelete")
        {
            return DalInstance.DeleteSoftly(condition, softDeleteFieldName);
        }

        /// <summary>
        /// 根据条件查询表中的指定列，并返回这一列的数据集合，若查询不到数据则返回空集合(而非null)
        /// </summary>
        /// <param name="whereStr">查询条件</param>
        /// <param name="fieldName">要查询的列名</param>
        /// <returns>查询到的数据集合或者空集合（非null）</returns>
        public IEnumerable<object> QuerySingleColumn(string whereStr, string fieldName)
        {
            return DalInstance.QuerySingleColumn(whereStr, fieldName);
        }

        /// <summary>
        /// 根据主键id查询单个实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="queryFields">指定要查询的部分列名称，此参数为空则默认查询所有列</param>
        /// <returns>查询到数据则返回实体对象，否则返回null</returns>
        public T QuerySingle(object id, string[] queryFields = null)
        {
            return DalInstance.QuerySingle(id, queryFields);
        }

        /// <summary>
        /// 根据条件查询单个实体
        /// </summary>
        /// <param name="whereStr">查询数据的where条件，不需要带有where关键字，只需要写出条件即可，如：name='test'，此参数为空时，默认查询所有数据</param>
        /// <param name="queryFields">指定要查询的部分列名称，此参数为空则默认查询所有列</param>
        /// <param name="paramDic">动态参数字典，键为字段名称，值为参数值，若有动态参数则参数名需要带有@字符</param>
        /// <returns>查询到数据则返回实体对象，否则返回null</returns>
        public T QuerySingle(string whereStr, string[] queryFields = null,
            IDictionary<string, object> paramDic = null)
        {
            return DalInstance.QuerySingle(whereStr, queryFields, paramDic);
        }

        #region QueryList
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
        public IEnumerable<T> QueryList(string whereStr, string[] fields,
            IDictionary<string, object> paramDic, string orderField,
            bool isDescending = false)
        {
            return DalInstance.QueryList(whereStr, fields, paramDic, orderField, isDescending);
        }

        /// <summary>
        /// 查询本表所有数据
        /// </summary>
        public IEnumerable<T> QueryAll()
        {
            return QueryList(string.Empty, null, null, null);
        }

        /// <summary>
        /// 根据条件查询数据集合
        /// </summary>
        /// <param name="whereStr">查询数据的where条件，不需要带有where关键字，只需要写出条件即可，如：name='test'，此参数为空时，默认查询所有数据</param>
        /// <returns>返回查询到的实体集合</returns>
        public IEnumerable<T> QueryList(string whereStr)
        {
            return QueryList(whereStr, null, null, null);
        }

        /// <summary>
        /// 根据条件查询数据集合
        /// </summary>
        /// <param name="whereStr">查询数据的where条件，不需要带有where关键字，只需要写出条件即可，如：name='test'，此参数为空时，默认查询所有数据</param>
        /// <param name="fields">指示查询的数据列的名称，带有此参数可避免select * 语句的低效查询，若此参数为空则默认查询所有列</param>
        /// <returns>返回查询到的实体集合</returns>
        public IEnumerable<T> QueryList(string whereStr, string[] fields)
        {
            return QueryList(whereStr, fields, null, null);
        }

        #endregion

        #region QueryPageList
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
        public IEnumerable<T> QueryPageList(int pageIndex, int pageSize, string whereStr, string[] fields,
            IDictionary<string, object> paramDic, string orderField,
            bool isDescending)
        {
            return DalInstance.QueryPageList(pageIndex, pageSize, whereStr, fields, paramDic, orderField, isDescending);
        }

        /// <summary>
        /// 分页查询数据（按Id升序排列）
        /// </summary>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">每页数据个数</param>
        /// <returns>返回查询到的实体集合</returns>
        public IEnumerable<T> QueryPageList(int pageIndex, int pageSize)
        {
            return QueryPageList(pageIndex, pageSize, string.Empty, null, null, null, false);
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">每页数据个数</param>
        /// <param name="orderField">查询数据的排序字段名称</param>
        /// <param name="isDescending">指示是否对查询结果降序排序</param>
        /// <returns>返回查询到的实体集合</returns>
        public IEnumerable<T> QueryPageList(int pageIndex, int pageSize, string orderField, bool isDescending)
        {
            return QueryPageList(pageIndex, pageSize, string.Empty, null, null, orderField, isDescending);
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">每页数据个数</param>
        /// <param name="orderField">查询数据的排序字段名称</param>
        /// <param name="isDescending">指示是否对查询结果降序排序</param>
        /// <param name="totalCount">本表总记录数量</param>
        /// <returns>返回查询到的实体集合</returns>
        public IEnumerable<T> QueryPageList(int pageIndex, int pageSize, string orderField, bool isDescending, out int totalCount)
        {
            totalCount = DalInstance.GetTotalCount();

            return QueryPageList(pageIndex, pageSize, string.Empty, null, null, orderField, isDescending);
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">每页数据个数</param>
        /// <param name="condition">查询条件</param>
        /// <param name="orderField">查询数据的排序字段名称</param>
        /// <param name="isDescending">指示是否对查询结果降序排序</param>
        /// <param name="totalCount">本表总记录数量</param>
        /// <returns>返回查询到的实体集合</returns>
        public IEnumerable<T> QueryPageList(int pageIndex, int pageSize, string condition, string orderField, bool isDescending, out int totalCount)
        {
            totalCount = DalInstance.GetTotalCount();

            return QueryPageList(pageIndex, pageSize, condition, null, null, orderField, isDescending);
        }

        /// <summary>
        /// 分页查询数据（按Id升序排列）
        /// </summary>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">每页数据个数</param>
        /// <param name="whereStr">查询数据的where条件，不需要带有where关键字，只需要写出条件即可，如：name='test'，此参数为空时，默认查询所有数据</param>
        /// <returns>返回查询到的实体集合</returns>
        public IEnumerable<T> QueryPageList(int pageIndex, int pageSize, string whereStr)
        {
            return QueryPageList(pageIndex, pageSize, whereStr, null, null, null, false);
        }

        /// <summary>
        /// 分页查询数据（按Id升序排列）
        /// </summary>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">每页数据个数</param>
        /// <param name="fields">指示查询的数据列的名称，带有此参数可避免select * 语句的低效查询，若此参数为空则默认查询所有列</param>
        /// <returns>返回查询到的实体集合</returns>
        public IEnumerable<T> QueryPageList(int pageIndex, int pageSize, string[] fields)
        {
            return QueryPageList(pageIndex, pageSize, string.Empty, fields, null, null, false);
        }
        #endregion

        /// <summary>
        /// 将多个执行数据库操作的委托封装到一个事务环境中，确保所有的委托都被正确执行，否则将执行回滚操作
        /// </summary>
        /// <param name="delegates">待执行的委托集合</param>
        public virtual bool ExecuteTransation(params Func<bool>[] delegates)
        {
            return DalInstance.ExecuteTransation(delegates);
        }
    }
}
