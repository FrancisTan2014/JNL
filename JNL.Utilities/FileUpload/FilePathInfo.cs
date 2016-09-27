using System;
using System.IO;
using System.Web;

namespace JNL.Utilities.FileUpload
{
    /// <summary>
    /// 对上传文件路径信息的封装，主要包含文件在服务器上存储的地址等信息
    /// </summary>
    /// <remarks>@FrancisTan 2016-05-17</remarks>
    public class FilePathInfo
    {
        #region Properties
        /// <summary>
        /// 原始文件名称
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        /// 新的图片名称
        /// </summary>
        public string NewFileName { get; set; }

        /// <summary>
        /// 文件类型（后缀名，*.jpg）
        /// </summary>
        public string FileExtension => Path.GetExtension(OriginalFileName);

        /// <summary>
        /// 文件大小（单位：KB）
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 文件存储在服务器上根目录
        /// </summary>
        public string RootRelativeDir { get; set; }

        /// <summary>
        /// 文件存储在服务器磁盘上的目录的绝对路径
        /// </summary>
        public string RootAbsolutePath { get; set; }

        /// <summary>
        /// 文件存储在服务器上的相对路径
        /// </summary>
        public string FileRelativePath => Path.Combine(RootRelativeDir, NewFileName).Replace("\\", "/");

        /// <summary>
        /// 文件存储在服务器磁盘上的绝对路径
        /// </summary>
        public string FileAbsolutePath => Path.Combine(RootAbsolutePath, NewFileName).Replace("/", "\\");

        /// <summary>
        /// 用于指示在创建文件路径时是否保留原文件名称，或者采用guid作为新名称
        /// </summary>
        public bool KeepOriginalName { get; set; }

        #endregion

        /// <summary>
        /// 通过指定参数创建实例对象
        /// </summary>
        /// <param name="fileName">图片文件名称</param>
        /// <param name="relativeDirPath">文件存储于服务器的目录的相对路径</param>
        public FilePathInfo(string fileName, string relativeDirPath)
        {
            Init(fileName, 0, relativeDirPath, false);
        }

        /// <summary>
        /// 通过指定参数创建实例对象
        /// </summary>
        /// <param name="fileName">图片文件名称</param>
        /// <param name="fileSize">文件大小，以KB为单位</param>
        /// <param name="relativeDirPath">文件存储于服务器的目录的相对路径</param>
        public FilePathInfo(string fileName, long fileSize, string relativeDirPath)
        {
            Init(fileName, fileSize, relativeDirPath, false);
        }

        /// <summary>
        /// 通过指定参数创建实例对象
        /// </summary>
        /// <param name="fileName">图片文件名称</param>
        /// <param name="fileSize">文件大小，以KB为单位</param>
        /// <param name="relativeDirPath">文件存储于服务器的目录的相对路径</param>
        /// <param name="keepOriginalName">是否保留原文件名（若为false，则使用guid命名）</param>
        public FilePathInfo(string fileName, long fileSize, string relativeDirPath, bool keepOriginalName)
        {
            Init(fileName, fileSize, relativeDirPath, keepOriginalName);
        }

        private void Init(string fileName, long fileSize, string relativeDirPath, bool keepOriginalName)
        {
            OriginalFileName = fileName;

            var dir = relativeDirPath.Replace("~/", "/");
            var rootAbPath = HttpContext.Current.Server.MapPath(dir);
            var dateDirName = DateTime.Now.ToString("yyyy-MM-dd");
            RootAbsolutePath = Path.Combine(rootAbPath, dateDirName);
            RootRelativeDir = Path.Combine(dir, dateDirName);

            NewFileName = fileName;
            if (!keepOriginalName)
            {
                var guid = Guid.NewGuid();
                NewFileName = guid + FileExtension;
            }

            FileSize = fileSize;
        }

        /// <summary>
        /// 判断指定的目录是否存在，若不存在则创建
        /// </summary>
        public void CreateDirectory()
        {
            if (!Directory.Exists(RootAbsolutePath))
            {
                Directory.CreateDirectory(RootAbsolutePath);
            }
        }

        /// <summary>
        /// 此方法是为了兼容以前的配置文件中目录路径开头不带/的情况
        /// </summary>
        /// <param name="path">待处理的路径</param>
        /// <returns>确保目录路径自根目录开始的新路径</returns>
        public static string ProcessBeginOfThePath(string path)
        {
            if (!path.StartsWith("/") && !path.StartsWith("\\") &&
                !path.StartsWith("~/"))
            {
                return "/" + path;
            }

            return path;
        }
    }
}
