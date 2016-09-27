using System;
using System.Reflection;
using JNL.Dal;

namespace JNL.Bll
{
    /// <summary>
    /// 数据访问层对象工厂类
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-07-14</since>
    public class DalFactory
    {
        /// <summary>
        /// 根据指定Model层对象的类型，创建与其相对应的Dal层对象
        /// </summary>
        /// <typeparam name="T">Model层对象类型</typeparam>
        /// <returns>与指定类型对应的Dal层实例</returns>
        public static BaseDal<T> CreateInstance<T>() where T : class,new()
        {
            var model = new T();
            var type = model.GetType();
            var dalFullName = $"JNL.Dal.{type.Name}Dal";

            var assembly = Assembly.Load("JNL.Dal");
            if (assembly == null)
            {
                throw new Exception("未能加载程序集JNL.Dal");
            }

            var dalInstance = (BaseDal<T>)assembly.CreateInstance(dalFullName);
            if (dalInstance == null)
            {
                var msg = $"创建{dalFullName}的实例失败";
                throw new Exception(msg);
            }

            return dalInstance;
        }
    }
}
