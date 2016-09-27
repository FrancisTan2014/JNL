using System;
using System.Collections.Generic;
using System.Web;
using JNL.Utilities.Extensions;

namespace JNL.Utilities.Helpers
{
    /// <summary>
    /// 为js插件jquery.datatable向后台请求数据的相关操作封装的帮助类
    /// </summary>
    /// <remarks>@谭光洪 2016-04-07</remarks>
    public class JqueryDataTableAjaxHelper
    {
        /// <summary>
        /// 此方法适用于处理jquery.datatable向后台请求列表数据的情景
        /// 旨在将对jquery.datatable的参数接收与返回封装在一个方法里
        /// 在处理请求的地方只需要关心如何查询所需要的数据的逻辑
        /// </summary>
        /// <param name="request">封装了当前请求信息的HttpRequest对象</param>
        /// <param name="queryPageListFunc">指定当方法解析出pageIndex,pageSize后获取数据的方法，此方法必须返回一个键值对，其中键为：数据总条件,值为：获取到的分布数据集合</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>将结果序列化后的json对象</returns>
        public static object GetPageListJson(HttpRequestBase request, Func<int, int, KeyValuePair<int, object>> queryPageListFunc)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (queryPageListFunc == null)
            {
                throw new ArgumentNullException(nameof(queryPageListFunc));
            }

            // 以下参数为jquery.datatable插件的传递的参数，名称不可更改
            var sEcho = request["sEcho"].ToInt32(); // 操作次数，默认对此数据进行+1操作
            var iDisplayStart = request["iDisplayStart"].ToInt32(); // 分页条件，本次分页取数据的开始位置
            var iDisplayLength = request["iDisplayLength"].ToInt32(); // 分布条件，本次分页取数据的长度

            var pageIndex = (iDisplayStart - 1) / iDisplayLength + 1;
            var pageSize = iDisplayLength;
            var keyPairValue = queryPageListFunc(pageIndex, pageSize);

            // 下面的对象符合插件要求回传的数据格式
            var jsonObj = new
            {
                sEcho = sEcho + 1,
                iTotalRecords = keyPairValue.Key,
                iTotalDisplayRecords = keyPairValue.Key,
                aaData = keyPairValue.Value
            };

            return jsonObj;
        }

        /// <summary>
        /// 此方法适用于处理jquery.datatable向后台请求列表数据的情景
        /// 旨在将对jquery.datatable的参数接收与返回封装在一个方法里
        /// 在处理请求的地方只需要关心如何查询所需要的数据的逻辑
        /// </summary>
        /// <param name="request">封装了当前请求信息的HttpRequest对象</param>
        /// <param name="queryPageListFunc">指定当方法解析出pageIndex,pageSize后获取数据的方法，此方法必须返回一个键值对，其中键为：数据总条件,值为：获取到的分布数据集合</param>
        /// <param name="jsonSerializer">指定当取到数据后使用的序列化方法</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>将结果序列化后的json字符串</returns>
        public static string GetPageListJson(HttpRequestBase request, Func<int, int, KeyValuePair<int, object>> queryPageListFunc, Func<object, string> jsonSerializer)
        {
            var jsonObj = GetPageListJson(request, queryPageListFunc);

            return jsonSerializer(jsonObj);
        }
    }
}
