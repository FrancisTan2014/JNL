namespace JNL.Web.Models
{
    /// <summary>
    /// 封装针对异步请求返回的错误消息
    /// </summary>
    /// <author>FrancisTan</author>
    /// <since>2016-07-14</since>
    public class ErrorModel
    {
        /// <summary>
        /// 操作成功（100）
        /// </summary>
        public static object OperateSuccess => new
        {
            code = 100,
            msg = "操作成功"
        };

        /// <summary>
        /// 输入有误（101）
        /// </summary>
        public static object InputError => new
        {
            code = 101,
            msg = "输入参数有误"
        };

        /// <summary>
        /// 登录成功（102）
        /// </summary>
        public static object LoginSuccess => new
        {
            code = 102,
            msg = "登录成功"
        };

        /// <summary>
        /// 登录失败（103）
        /// </summary>
        public static object LoginFailed => new
        {
            code = 103,
            msg = "用户名或者密码错误"
        };

        /// <summary>（105）
        /// 已退出登录
        /// </summary>
        public static object Logout => new
        {
            code = 105,
            msg = "已退出登录"
        };

        /// <summary>
        /// 获取封装了通知客户端需要跳转到登录页的消息（104）
        /// </summary>
        /// <param name="backUrl">在登录页面登录成功后跳转至的页面地址</param>
        /// <returns>返回给客户端的json数据</returns>
        public static object NeedLoginFirst(string backUrl)
        {
            return new
            {
                code = 104,
                msg = "未登录或者登录已失效",
                backUrl
            };
        }

        /// <summary>
        /// 非法的app签名（106）
        /// </summary>
        public static object SignatureError => new
        {
            code = 106,
            msg = "非法的app签名"
        };

        /// <summary>
        /// 批量插入数据成功（107）
        /// </summary>
        public static object BulkInsertSuccess => new
        {
            code = 107, 
            msg = "数据插入成功"
        };

        /// <summary>
        /// app请求数据成功后返回的消息（带有请求到的数据）（108）
        /// </summary>
        /// <param name="data">请求到的数据结果</param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static object GetDataSuccess(object data, string tableName = "")
        {
            return new
            {
                code = 108,
                msg = "数据请求成功",
                data = data,
                tableName = tableName
            };
        }

        /// <summary>
        /// 未查询到指定信息（109）
        /// </summary>
        public static object GetDataFailed => new
        {
            code = 109, 
            msg = "未能查询到指定信息"
        };

        /// <summary>
        /// 文件上传失败（110）
        /// </summary>
        public static object FileUploadFailed => new
        {
            code = 110,
            msg = "文件上传失败"
        };

        /// <summary>
        /// 操作失败（111）
        /// </summary>
        public static object OperateFailed => new
        {
            code = 111,
            msg = "操作失败，请稍后重试"
        };

        /// <summary>
        /// 结果为真（针对某些需要从服务器获取一段逻辑的真假的返回结果）（112）
        /// </summary>
        public static object TrueResult => new
        {
            code = 112,
            msg = "结果为是"
        };

        /// <summary>
        /// 结果为假（针对某些需要从服务器获取一段逻辑的真假的返回结果）（113）
        /// </summary>
        public static object FalseResult => new
        {
            code = 113,
            msg = "结果为否"
        };

        /// <summary>
        /// 存在依赖项（114）
        /// </summary>
        public static object DeleteForbidden => new
        {
            code = 114,
            msg = "存在依赖项，无法删除"
        };

        /// <summary>
        /// 添加数据成功（116）
        /// </summary>
        /// <param name="id">新添加的数据id</param>
        /// <returns></returns>
        public static object AddDataSuccess(int id)
        {
            return new
            {
                code = 116,
                msg = "添加成功",
                id
            };
        }

        /// <summary>
        /// 未指定上传的文件类型（117）
        /// </summary>
        public static object UnknownUploadFileType => new
        {
            code = 117,
            msg = "未指定上传的文件类型"
        };

        /// <summary>
        /// 已存在相同项（118）
        /// </summary>
        public static object ExistSameItem => new
        {
            code = 118,
            msg = "已存在相同顶"
        };

        /// <summary>
        /// 目录已存在（119）
        /// </summary>
        public static object DirectoryExists => new
        {
            code = 119,
            msg = "目录已存在，无法创建相同名称的目录"
        };

        /// <summary>
        /// 文件已存在（120）
        /// </summary>
        public static object FileExists => new
        {
            code = 120,
            msg = "当前目录下已存在同名文件，无法上传"
        };
    }
}