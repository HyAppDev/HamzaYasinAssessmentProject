using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace HamzaYasinAssessmentProject.Maui.Converters
{
    public class BoolToStatusConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is true ? "Disable" : "Enable";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
