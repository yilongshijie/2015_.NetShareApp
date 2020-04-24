using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{

    public static class ImageManager
    {
        private static object objectLock = new object();
        private static object objectLock2 = new object();
        public static void CompressImage(string originImage)
        {
            try
            {
                lock (objectLock)
                {
                    Path.Combine(Path.GetDirectoryName(originImage), string.Format("{0}_{1}", Path.GetFileNameWithoutExtension(originImage), Path.GetExtension(originImage)));
                    Image original = Image.FromFile(originImage);
                    Bitmap bitmap = new Bitmap(original);
                    original.Dispose();
                    int num = 95;
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)num);
                    MemoryStream stream = new MemoryStream();
                    bitmap.Save(stream, ImageCodecInfo.GetImageEncoders().FirstOrDefault<ImageCodecInfo>(o => o.MimeType == "image/jpeg"), encoderParams);
                    Image image2 = Image.FromStream(stream);
                    bitmap.Dispose();
                    encoderParams.Dispose();
                    int num2 = 0;
                    Label_00B5:;
                    try
                    {
                        image2.Save(originImage);
                        image2.Dispose();
                        stream.Dispose();
                    }
                    catch (Exception exception)
                    {
                        if ((num2 >= 10) || (exception.Message.IndexOf("GDI+ 中发生一般性错误。") <= -1))
                        {
                            image2.Dispose();
                            throw;
                        }
                        num2++;
                        goto Label_00B5;
                    }
                }
            }
            catch
            {

            }
        }

        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            Bitmap bitmap2 = null;
            try
            {
                if (newW == 0)
                {
                    newW = 1;
                }
                if (newH == 0)
                {
                    newH = 1;
                }
                Bitmap image = new Bitmap(newW, newH);
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                    bitmap2 = image;
                }
            }
            catch
            {

            }
            return bitmap2;
        }

        public static string ResizeImage(string originImage, ImageResizeOptions options2)
        {

            string filename = string.Format("{0}_{1}x{2}{3}", new object[] { Path.GetFileNameWithoutExtension(originImage),
                options2.Width, options2.Height, Path.GetExtension(originImage) });
            string str = Path.Combine(Path.GetDirectoryName(originImage), filename);
            ResizeImage(originImage, str, options2);
            CompressImage(str);
            File.Delete(originImage);
            return filename;
        }

        private static void ResizeImage(string originImage, string tatget, ImageResizeOptions opt)
        {
            try
            {
                lock (objectLock)
                {
                    using (Image image = Image.FromFile(originImage))
                    {
                        if (!opt.cut)
                        {
                            if (opt.Width > image.Width)
                            {
                                opt.Width = image.Width;
                                opt.Height = image.Height;
                            }
                            else
                            {
                                opt.Height = (int)Math.Round(opt.Width * (Convert.ToDecimal(image.Height) / Convert.ToDecimal(image.Width)));
                            }

                        }
                        else
                        {
                            decimal num51 = Convert.ToDecimal(opt.Height) / opt.Width;
                            if (opt.Width > image.Width)
                            {
                                opt.Width = image.Width;
                                opt.Height = Convert.ToInt32(opt.Width * num51);
                            }

                        }

                        int x = 0;
                        int y = 0;
                        int width = 0;
                        int height = 0;
                        decimal num5 = opt.Width / Convert.ToDecimal(opt.Height);
                        decimal num6 = image.Width / Convert.ToDecimal(image.Height);
                        if (num6 > num5)
                        {
                            x = (int)Math.Round((decimal)((Convert.ToDecimal(image.Width) - (num5 * image.Height)) / 2M));
                            width = (int)Math.Round((decimal)(num5 * image.Height));
                            height = image.Height;
                        }
                        else if (num6 < num5)
                        {
                            y = (int)Math.Round((decimal)((Convert.ToDecimal(image.Height) - Convert.ToDecimal((decimal)(image.Width / num5))) / 2M));
                            width = image.Width;
                            height = (int)Math.Round((decimal)(image.Width / num5));
                        }
                        else if (num6 == num5)
                        {
                            width = image.Width;
                            height = image.Height;
                        }
                        if (image.Width < (x + width))
                        {
                            width = image.Width - x;
                        }
                        if (image.Height < (y + height))
                        {
                            height = image.Height - y;
                        }
                        RectangleF cloneRect = new RectangleF(x, y, width, height);
                        Rectangle rect = new Rectangle(x, y, width, height);
                        using (Bitmap bitmap = new Bitmap(image))
                        {
                            
                            using (Bitmap bitmap2 = bitmap.Clone(cloneRect, bitmap.PixelFormat))
                            {

                                using (Bitmap bitmap3 = KiResizeImage(bitmap2, opt.Width, opt.Height))
                                {
                                    int num7 = 0;
                                    Label_01C0:;
                                    try
                                    {
                                        bitmap3.Save(tatget);
                                    }
                                    catch (Exception exception)
                                    {
                                        if ((num7 >= 10) || (exception.Message.IndexOf("GDI+ 中发生一般性错误。") <= -1))
                                        {
                                            throw;
                                        }
                                        num7++;
                                        goto Label_01C0;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch
            {

            }
        }
    }
}
