using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Paftax.Pafta.UI.Converters
{
    public class PointRToPoint : IValueConverter
    {
        public double CanvasHeight { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Point pointR)
            {
                Point point = new(pointR.X, CanvasHeight / 2 - pointR.Y);
                return point;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
