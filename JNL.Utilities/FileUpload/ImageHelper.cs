using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;

namespace Guoli.Utilities.FileUpload
{
    public class ImageHelper
    {
        #region Methods:图片路径/类型等

        public static ImageFormat GetImageFormat(string imgExtension)
        {
            if (string.IsNullOrEmpty(imgExtension))
            {
                throw new ArgumentNullException("imgExtension");
            }

            var type = imgExtension.Replace(".", "").ToLower();
            switch (type)
            {
                case "png":
                    return ImageFormat.Png;
                case "jpg":
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }
        #endregion

        /// <summary>
        /// 将指定图片流处理成给定质量的标清图片
        /// </summary>
        /// <param name="imgStream"></param>
        /// <param name="quality"></param>
        /// <param name="imgExtension"></param>
        /// <returns></returns>
        public static void GetStandardImage(Stream imgStream, string savePath, int quality, string imgExtension)
        {
            using (var bitmap = new Bitmap(imgStream))
            {
                var encoder = System.Drawing.Imaging.Encoder.Quality;
                using (var ept = new EncoderParameter(encoder, quality))
                {
                    using (var encoderParameters = new EncoderParameters(1))
                    {
                        var mimeType = GetMiMeType(imgExtension);
                        var ici = GetImageCoderInfo(mimeType);

                        encoderParameters.Param[0] = ept;

                        bitmap.Save(savePath, ici, encoderParameters);
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定后缀名的MIME类型
        /// </summary>
        public static string GetMiMeType(string extension)
        {
            var ext = extension.ToLower();
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 获取图片编码类型信息
        /// </summary>
        /// <param name="coderType">编码类型</param>
        /// <returns>ImageCodecInfo</returns>
        public static ImageCodecInfo GetImageCoderInfo(string coderType)
        {
            ImageCodecInfo[] iciS = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo retIci = null;
            foreach (ImageCodecInfo ici in iciS)
            {
                if (ici.MimeType.Equals(coderType))
                    retIci = ici;
            }
            return retIci;
        }

        #region Methods：图片处理
        /// <summary>
        /// 获取指定图片的缩略（指定大小）
        /// </summary>
        /// <remarks>@FrancisTan 2016-05-17</remarks>
        /// <param name="imgStream">图片二进制流</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图宽度</param>
        /// <param name="quality">图片质量百分比，它必须是0~100之间的整数</param>
        /// <exception cref="ArgumentNullException">图片文件流为空</exception>
        /// <returns>包含缩略图信息的Image对象</returns>
        public static Image GetThumbnail(Stream imgStream, int width, int height, int quality)
        {
            using (Stream thumbNailStream = GetThumbnailStream(imgStream, width, height, quality))
            {
                return Image.FromStream(thumbNailStream);
            }
        }

        /// <summary>
        /// 获取指定图片的缩略（指定大小）
        /// </summary>
        /// <remarks>@FrancisTan 2016-05-17</remarks>
        /// <param name="imgStream">图片二进制流</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图宽度</param>
        /// <param name="quality">图片质量百分比，它必须是0~100之间的整数</param>
        /// <exception cref="ArgumentNullException">图片文件流为空</exception>
        /// <returns>缩略图二进制流</returns>
        public static Stream GetThumbnailStream(Stream imgStream, int width, int height, int quality)
        {
            if (imgStream == null || imgStream.Length == 0)
            {
                throw new ArgumentNullException("imgStream");
            }
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException("width", "图片宽度必须大于0");
            }
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException("height", "图片高度必须大于0");
            }
            if (quality <= 0 || quality > 100)
            {
                throw new ArgumentOutOfRangeException("quality", "图片质量百分比须在0~100之间");
            }

            // Format is automatically detected though can be changed.
            ISupportedImageFormat format = new PngFormat();
            Size size = new Size(width, height);

            using (ImageFactory factory = new ImageFactory(preserveExifData: true))
            {
                factory.Load(imgStream)
                    .Format(format);

                var scaling = GetScaling(factory.Image.Size, size);
                var scaleSize = GetScaleSize(factory.Image.Size, scaling);
                factory.Constrain(scaleSize);

                return CropAroundCenterPoint(factory, size);
            }
        }

        /// <summary>
        /// 将给定图片按指定比例压缩，并返回新的图片
        /// </summary>
        /// <param name="imgStream">待压缩的图片二进制流</param>
        /// <param name="quality">压缩比例，它是一个1到100之间的整数</param>
        /// <returns>压缩后的图片文件对象</returns>
        public static Image GetZipImage(Stream imgStream, int quality)
        {
            using (var zipStream = GetZipImageStream(imgStream, quality))
            {
                return Image.FromStream(zipStream);
            }
        }

        /// <summary>
        /// 将给定图片按指定比例压缩，并返回新的图片
        /// </summary>
        /// <param name="imgStream">待压缩的图片二进制流</param>
        /// <param name="quality">压缩比例，它是一个1到100之间的整数</param>
        /// <returns>压缩后的图片二进制流</returns>
        public static Stream GetZipImageStream(Stream imgStream, int quality)
        {
            MemoryStream outStream = new MemoryStream();
            using (ImageFactory factory = new ImageFactory(preserveExifData: true))
            {
                factory.Load(imgStream)
                    .Quality(quality)
                    .Save(outStream);
            }

            return outStream;
        }

        /// <summary>
        /// 将给定图片从中心点周围裁剪出指定大小的图片
        /// </summary>
        /// <param name="imgStream">给定图片二进制流</param>
        /// <param name="width">需要裁剪的宽度</param>
        /// <param name="height">需要裁剪的高度</param>
        /// <returns>从原图片中裁剪出来的新图片二进制流</returns>
        public static Stream CropAroundCenterPoint(Stream imgStream, int width, int height)
        {
            return CropAroundCenterPoint(imgStream, new Size(width, height));
        }

        /// <summary>
        /// 将给定图片从中心点周围裁剪出指定大小的图片
        /// </summary>
        /// <param name="imgStream">给定图片二进制流</param>
        /// <param name="size">需要裁剪的尺寸</param>
        /// <returns>从原图片中裁剪出来的新图片二进制流</returns>
        public static Stream CropAroundCenterPoint(Stream imgStream, Size size)
        {
            using (ImageFactory factory = new ImageFactory(preserveExifData: true))
            {
                factory.Load(imgStream);

                return CropAroundCenterPoint(factory, size);
            }
        }

        /// <summary>
        /// 将给定图片从中心点周围裁剪出指定大小的图片
        /// </summary>
        /// <param name="factory">包含给定图片的工厂类对象</param>
        /// <param name="size">需要裁剪的尺寸</param>
        /// <returns>从原图片中裁剪出来的新图片二进制流</returns>
        private static Stream CropAroundCenterPoint(ImageFactory factory, Size size)
        {
            MemoryStream outStream = new MemoryStream();

            var cropArea = GetCropAreaAroundCenterPoint(factory.Image.Size, size);
            factory.Crop(cropArea)
                   .Save(outStream);

            return outStream;
        }

        /// <summary>
        /// 根据原始尺寸以及给定尺寸求出一个以原图中心点为中心点的矩形
        /// </summary>
        /// <param name="originalSize">原图尺寸</param>
        /// <param name="newSize">需要裁剪的尺寸</param>
        /// <returns>返回一个以原图中心点为中心点，大小为给定尺寸的矩形</returns>
        public static Rectangle GetCropAreaAroundCenterPoint(Size originalSize, Size newSize)
        {
            // 顶点Y坐标：1/2原图高 - 1/2新图高
            // 顶点X从标：1/2原图宽 - 1/2新图宽
            var originPointX = originalSize.Width / 2 - newSize.Width / 2;
            if (originPointX < 0)
            {
                originPointX = 0;
            }

            var originPointY = originalSize.Height / 2 - newSize.Height / 2;
            if (originPointY < 0)
            {
                originPointY = 0;
            }

            var originPoint = new Point(originPointX, originPointY);

            return new Rectangle(originPoint, newSize);
        }

        /// <summary>
        /// 根据原始尺寸以及给定尺寸，求出相对给定尺寸的原始图片的缩放比例
        /// </summary>
        /// <param name="originalSize">原图尺寸</param>
        /// <param name="newSize">给定尺寸</param>
        /// <returns>返回相对给定尺寸的原始图片的缩放比例</returns>
        public static double GetScaling(Size originalSize, Size newSize)
        {
            // 原图宽高均比给定尺寸小
            if (originalSize.Width <= newSize.Width && originalSize.Height <= newSize.Height)
            {
                return 0;
            }

            // 原图宽度小于给定宽度，高度大于给定高度
            if (originalSize.Width <= newSize.Width && originalSize.Height >= newSize.Height)
            {
                return 0;
            }

            // 原图宽度大于给定宽度，高度小于给定高度
            if (originalSize.Width >= newSize.Width && originalSize.Height <= newSize.Height)
            {
                return 0;
            }

            // 原图宽高均大于给定宽高
            if (originalSize.Width > newSize.Width && originalSize.Height > newSize.Height)
            {
                var minusWidth = originalSize.Width - newSize.Width;
                var minusHeight = originalSize.Height - newSize.Height;

                return minusWidth <= minusHeight
                    ? newSize.Width * 1.0 / originalSize.Width
                    : newSize.Height * 1.0 / originalSize.Height;
            }

            return 0;
        }

        /// <summary>
        /// 根据指定的缩放比例，创建新的图片尺寸对象
        /// </summary>
        /// <param name="originalSize">图片原始大小</param>
        /// <param name="scaling">缩放比例</param>
        /// <returns>缩放后的尺寸</returns>
        public static Size GetScaleSize(Size originalSize, double scaling)
        {
            var scaleWidth = scaling * originalSize.Width;
            var scaleHeight = scaling * originalSize.Height;
            return new Size((int)scaleWidth, (int)scaleHeight);
        }

        #endregion
    }
}
