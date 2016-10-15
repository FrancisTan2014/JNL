using System;
using JNL.Model;

namespace JNL.Bll
{
    public partial class ExceptionLogBll
    {
        private static readonly ExceptionLogBll Persistence = new ExceptionLogBll();

        /// <summary>
        /// 异常信息持久化
        /// </summary>
        /// <param name="fileName">异常发生时所在的文件名称</param>
        /// <param name="className">异常发生时所在的类名称</param>
        /// <param name="exception">异常信息对象</param>
        public static void ExceptionPersistence(string fileName, string className, Exception exception)
        {
            var message = exception.Message + exception.InnerException?.Message;

            var exceptionLog = new ExceptionLog
            {
                FileName = fileName,
                ClassName = className,
                Message = message,
                Source = 1,
                MethodName = exception.TargetSite.ToString(),
                Instance = exception.Source,
                StackTrace = exception.StackTrace,
                HappenTime = DateTime.Now
            };

            Persistence.Insert(exceptionLog);
        }
    }
}
