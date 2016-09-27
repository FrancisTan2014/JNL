using System;

namespace JNL.Utilities.Extensions
{
    /// <summary>
    /// 对DateTime类型的扩展
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// 将1900-01-01这一天做为时间的默认值
        /// </summary>
        public static readonly DateTime DefaultValue = new DateTime(1900, 1, 1);
    }
}
