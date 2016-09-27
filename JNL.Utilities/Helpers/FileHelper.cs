using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace JNL.Utilities.Helpers
{
    /// <summary>
    /// 文件操作帮助类
    /// </summary>
    /// <since>2016-09-18</since>
    public static class FileHelper
    {
        /// <summary>
        /// 将指定目录压缩为zip包
        /// </summary>
        /// <param name="zipFilePath">zip文件绝对路径</param>
        /// <param name="targetDir">待压缩的目录</param>
        public static void Zip(string zipFilePath, string targetDir)
        {
            if (!Directory.Exists(targetDir))
            {
                throw new DirectoryNotFoundException();
            }

            var zip = new FastZip();
            zip.CreateZip(zipFilePath, targetDir, true, string.Empty);
        }

        /// <summary>
        /// 将指定zip压缩包解压到指定目录下
        /// </summary>
        /// <param name="zipFilePath">压缩包文件绝对路径</param>
        /// <param name="extractPath">解压路径</param>
        public static void ExtractZip(string zipFilePath, string extractPath)
        {
            var zip = new FastZip();
            zip.ExtractZip(zipFilePath, extractPath, string.Empty);
        }

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="stream">The text file stream to analyze.</param>
        /// <returns>The detected encoding.</returns>
        public static Encoding GetEncoding(FileStream stream)
        {
            // Read the BOM
            var bom = new byte[4];
            stream.Read(bom, 0, 4);
            stream.Seek(0, SeekOrigin.Begin);

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.Default;
        }
    }
}
