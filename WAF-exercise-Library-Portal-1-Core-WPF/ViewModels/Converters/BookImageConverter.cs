using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels.Converters
{
    public class BookImageConverter : IValueConverter
    {
        public object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if ((value is Byte[]) == false)
            {
                return Binding.DoNothing;
            }

            try
            {
                using (MemoryStream stream = new MemoryStream(value as Byte[]))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                    return image;
                }
            }
            catch
            {
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
