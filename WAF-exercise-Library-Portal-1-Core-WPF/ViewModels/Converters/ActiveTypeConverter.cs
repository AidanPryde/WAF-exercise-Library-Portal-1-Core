using System;
using System.Globalization;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Linq;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels.Converters
{
    public class ActiveTypeConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if ((value is Byte) == false)
            {
                return Binding.DoNothing;
            }

            if (parameter == null || !(parameter is IEnumerable<String>))
            {
                return Binding.DoNothing;
            }

            List<String> activeNames = (parameter as IEnumerable<String>).ToList();
            Byte index = (Byte)value;

            if (index < 0 || index > activeNames.Count())
            {
                return Binding.DoNothing;
            }

            return activeNames[index];
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value == null || !(value is String))
            {
                return DependencyProperty.UnsetValue;
            }

            if (parameter == null || !(parameter is IEnumerable<String>))
            {
                return Binding.DoNothing;
            }

            List<String> actionNames = (parameter as IEnumerable<String>).ToList();
            String action = (String)value;

            if (!actionNames.Contains(action))
            {
                return DependencyProperty.UnsetValue;
            }

            return (Byte)actionNames.IndexOf(action);
        }
    }
}
