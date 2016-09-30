using System;
using System.Web;
using JNL.Bll;
using JNL.Utilities.Extensions;
using JNL.Utilities.FileUpload;
using JNL.Web.Models;

namespace JNL.Web.Utils
{
    /// <summary>
    /// 提供处理客户端文件上传的方法
    /// </summary>
    public static class UploadHelper
    {
        /// <summary>
        /// 处理文件上传
        /// </summary>
        /// <returns>上传成功则返回文件在服务器上的存储信息，否则返回上传失败的提示信息</returns>
        public static object FileUpload()
        {
           var request = HttpContext.Current.Request;

            try
            {
                if (request.Files.Count > 0)
                {
                    var receptFile = request.Files[0];
                    var fileName = receptFile.FileName;
                    var fileSize = receptFile.ContentLength / 1024;

                    var fileType = request["fileType"].ToInt32();
                    var savePath = AppSettings.GetFileSavePath(fileType);
                    if (string.IsNullOrEmpty(savePath))
                    {
                        return ErrorModel.UnknownUploadFileType;
                    }

                    var filePathInfo = new FilePathInfo(fileName, fileSize, savePath);

                    filePathInfo.CreateDirectory();
                    receptFile.SaveAs(filePathInfo.FileAbsolutePath);

                    return filePathInfo;
                }

                return ErrorModel.FileUploadFailed;
            }
            catch (Exception ex)
            {
                ExceptionLogBll.ExceptionPersistence("UploadHelper.cs", "UploadHelper", ex);

                return ErrorModel.FileUploadFailed;
            }
        }
    }
}