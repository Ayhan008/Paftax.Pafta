using System.Globalization;
using System.Windows.Data;

namespace Paftax.Pafta.UI.Converters
{
    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "Collapsed" : "Visible";
            }
            return "Visible";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue)
            {
                return strValue.Equals("Visible", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}
