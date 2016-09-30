using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using JNL.Bll;
using JNL.Model;
using JNL.Utilities.Helpers;
using JNL.Web.Models;
using JNL.Web.Utils;

namespace JNL.Web.Controllers
{
    public class CommonController : Controller
    {
        /// <summary>
        /// 公共接口，获取数据集合
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetList(GetListParams parameters)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var bllInstance = BllFactory.GetBllInstance(parameters.TableName);
                    if (bllInstance == null)
                    {
                        return Json(ErrorModel.InputError);
                    }

                    var condition = string.IsNullOrEmpty(parameters.Conditions)
                        ? string.Empty
                        : parameters.Conditions.Replace("###", " AND ");

                    object data;
                    var type = bllInstance.GetType();
                    if (parameters.PageIndex <= 0 || parameters.PageSize <= 0)
                    {
                        data = type.InvokeMember("QueryList", BindingFlags.InvokeMethod, null, bllInstance,
                            new object[] { condition, null, null, parameters.OrderField, parameters.Desending });
                    }
                    else
                    {
                        data = type.InvokeMember("QueryPageList", BindingFlags.InvokeMethod, null, bllInstance,
                            new object[] { parameters.PageIndex, parameters.PageSize, condition, null, null, parameters.OrderField, parameters.Desending });
                    }

                    return Json(ErrorModel.GetDataSuccess(data));
                }
                catch (Exception ex)
                {
                    ExceptionLogBll.ExceptionPersistence(nameof(CommonController), nameof(CommonController), ex);

                    return Json(ErrorModel.InputError);
                }
            }

            return Json(ErrorModel.InputError);
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <param name="json"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertData(string json, string target)
        {
            return InsertOrUpdateData(json, target, "INSERT");
        }

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="json">数据实体json对象</param>
        /// <param name="target">目录表名</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateData(string json, string target)
        {
            return InsertOrUpdateData(json, target, "UPDATE");
        }

        /// <summary>
        /// 反射获取指定表的Model类型
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        private Type GetModelType(string tableName)
        {
            var assembly = Assembly.Load("JNL.Model");

            return assembly.GetType($"JNL.Model.{tableName}");
        }

        private JsonResult InsertOrUpdateData(string json, string target, string operate)
        {
            var modelType = GetModelType(target);
            if (modelType == null)
            {
                return Json(ErrorModel.InputError);
            }

            dynamic bllInstance = BllFactory.GetBllInstance(target);
            if (bllInstance == null)
            {
                return Json(ErrorModel.InputError);
            }
            
            dynamic model = typeof(JsonHelper).GetMethod("Deserialize")
                .MakeGenericMethod(modelType)
                .Invoke(null, BindingFlags.InvokeMethod, null, new object[] { json }, CultureInfo.CurrentCulture);

            bool success;
            if (operate == "INSERT")
            {
                bllInstance.Insert(model);

                success = model.Id > 0;
            }
            else if (operate == "UPDATE")
            {
                success = bllInstance.Update(model);
            }
            else
            {
                return Json(ErrorModel.InputError);
            }

            if (success)
            {
                return Json(ErrorModel.OperateSuccess);
            }

            return Json(ErrorModel.OperateFailed);
        }

        /// <summary>
        /// 接收客户端上传的文件并保存到本地，将保存的路径返回给客户端
        /// </summary>
        [HttpPost]
        public JsonResult FileUpload()
        {
            var uploadRes = UploadHelper.FileUpload();
            return Json(uploadRes);
        }
    }
}
