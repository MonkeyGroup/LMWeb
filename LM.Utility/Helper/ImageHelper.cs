using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace LM.Utility.Helper
{
    public class ImageHelper
    {
        /// <summary>
        /// 切图 filepath, model.x, model.y, model.width, model.height, 75L, path180, 180
        /// </summary>
        /// <param name="imgSrc">图片原始路径</param>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        /// <param name="width">选择图片宽度</param>
        /// <param name="height">选择图片高度</param>
        /// <param name="quality">质量参数</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="t">输出图片的高度/宽度</param>
        public static void CutAvatar(string imgSrc, int x, int y, int width, int height, long quality, string savePath, int t)
        {
            var original = Image.FromFile(imgSrc);
            var img = new Bitmap(t, t, PixelFormat.Format24bppRgb);
            img.MakeTransparent(img.GetPixel(0, 0));
            img.SetResolution(72, 72);
            using (var gr = Graphics.FromImage(img))
            {
                if (original.RawFormat.Equals(ImageFormat.Jpeg) || original.RawFormat.Equals(ImageFormat.Png) || original.RawFormat.Equals(ImageFormat.Bmp))
                {
                    gr.Clear(Color.Transparent);
                }
                if (original.RawFormat.Equals(ImageFormat.Gif))
                {
                    gr.Clear(Color.White);
                }


                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                using (var attribute = new ImageAttributes())
                {
                    attribute.SetWrapMode(WrapMode.TileFlipXY);
                    gr.DrawImage(original, new Rectangle(0, 0, t, t), x, y, width, height, GraphicsUnit.Pixel, attribute);
                }
            }

            var myImageCodecInfo = GetEncoderInfo("image/jpeg");
            if (original.RawFormat.Equals(ImageFormat.Jpeg))
            {
                myImageCodecInfo = GetEncoderInfo("image/jpeg");
            }
            else if (original.RawFormat.Equals(ImageFormat.Png))
            {
                myImageCodecInfo = GetEncoderInfo("image/png");
            }
            else if (original.RawFormat.Equals(ImageFormat.Gif))
            {
                myImageCodecInfo = GetEncoderInfo("image/gif");
            }
            else if (original.RawFormat.Equals(ImageFormat.Bmp))
            {
                myImageCodecInfo = GetEncoderInfo("image/bmp");
            }

            Encoder myEncoder = Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            img.Save(savePath, myImageCodecInfo, myEncoderParameters);
        }

        /// <summary>
        /// 获取图像编码器的相关信息
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            var encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}