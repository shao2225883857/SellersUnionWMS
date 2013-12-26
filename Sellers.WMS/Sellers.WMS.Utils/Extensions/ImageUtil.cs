using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Sellers.WMS.Utils.Extensions
{
    public class ImageUtil
    {
        public static byte[] GetPictureData(string imagepath)
        {
            /**/
            ////根据图片文件的路径使用文件流打开，并保存为byte[] 
            FileStream fs = new FileStream(imagepath, FileMode.Open);//可以是其他重载方法 
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
            fs.Close();
            return byData;
        }
        //或者使用
        public static byte[] PhotoImageInsert(System.Drawing.Image imgPhoto)
        {
            //将Image转换成流数据，并保存为byte[] 
            MemoryStream mstream = new MemoryStream();
            imgPhoto.Save(mstream, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] byData = new Byte[mstream.Length];
            mstream.Position = 0;
            mstream.Read(byData, 0, byData.Length);
            mstream.Close();
            return byData;
        }

        public static System.Drawing.Image ReturnPhoto(byte[] streamByte)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }

        public static void DrawImageRectRect(Image imageFrom, string newImgPath, int width, int height)
        {
            // 源图宽度及高度 
            int imageFromWidth = imageFrom.Width;
            int imageFromHeight = imageFrom.Height;
            //在原画布中的位置
            int X, Y;
            //在原画布中取得的长宽
            int bitmapWidth, bitmapHeight;
            //// 根据源图及欲生成的缩略图尺寸,计算缩略图的实际尺寸及其在"画布"上的位置 
            if (imageFromWidth / width > imageFromHeight / height)
            {
                bitmapWidth = (width * imageFromHeight) / height;
                bitmapHeight = imageFromHeight;
                X = (imageFromWidth - bitmapWidth) / 2;
                Y = 0;
            }
            else
            {
                bitmapWidth = imageFromWidth;
                bitmapHeight = (height * imageFromWidth) / width;
                X = 0;
                Y = (imageFromHeight - bitmapHeight) / 2;

            }
            // 创建画布 
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            // 用白色清空 
            g.Clear(Color.White);
            // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。 
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // 指定高质量、低速度呈现。 
            g.SmoothingMode = SmoothingMode.HighQuality;
            // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。 
            g.DrawImage(imageFrom, new Rectangle(0, 0, width, height), new Rectangle(X, Y, bitmapWidth, bitmapHeight), GraphicsUnit.Pixel);
            try
            {
                string path = Path.GetDirectoryName(newImgPath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (File.Exists(newImgPath))
                {
                    File.Delete(newImgPath);
                }

                //经测试 .jpg 格式缩略图大小与质量等最优 
                bmp.Save(newImgPath, ImageFormat.Jpeg);
            }
            catch
            {

            }
            finally
            {
                //显示释放资源 
                imageFrom.Dispose();
                bmp.Dispose();
                g.Dispose();
            }
        }

        public static void DrawImageRectRect(string rawImgPath, string newImgPath, int width, int height)
        {
            try
            {
                System.Drawing.Image imageFrom = System.Drawing.Image.FromFile(rawImgPath);
                DrawImageRectRect(imageFrom, newImgPath, width, height);
            }
            catch (Exception)
            {

            }

        }
    }
}
