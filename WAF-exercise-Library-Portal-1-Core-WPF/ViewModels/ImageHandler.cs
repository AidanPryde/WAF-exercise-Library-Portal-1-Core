using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels
{
    public static class ImageHandler
    {
        public static Byte[] OpenAndResize(String path, Int32 width)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path);
            image.DecodePixelWidth = width;
            image.EndInit();

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }
    }
}
