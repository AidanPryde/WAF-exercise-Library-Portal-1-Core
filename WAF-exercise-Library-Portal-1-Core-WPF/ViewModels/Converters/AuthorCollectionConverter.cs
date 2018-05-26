using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels.Converters
{
    public class AuthorCollectionConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is IEnumerable<AuthorData>))
            {
                return Binding.DoNothing;
            }

            return (value as IEnumerable<AuthorData>).Select(ad => ad.Name);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
