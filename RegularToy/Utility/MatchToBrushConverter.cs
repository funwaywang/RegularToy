using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace RegularToy
{
    [ValueConversion(typeof(string), typeof(Brush))]
    public class MatchToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                switch ((string)value)
                {
                    case "M":
                        return new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50));
                    case "G":
                        return new SolidColorBrush(Color.FromRgb(0xFF, 0x98, 0x00));
                    case "C":
                        return new SolidColorBrush(Color.FromRgb(0x9C, 0x27, 0xB0));
                }
            }

            return new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
