using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Paftax.Pafta.UI.Converters
{
    public sealed class DrawingPointsToPointCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<System.Drawing.Point> points)
            {
                foreach (var point in points)
                {
                    PointCollection pointCollection =
                    [
                        new System.Windows.Point(point.X, point.Y)
                    ];
                    return pointCollection;
                }
            }
            if (value is IEnumerable<System.Drawing.PointF> pointFs)
            {
                foreach (var point in pointFs)
                {
                    PointCollection pointCollection =
                    [
                        new System.Windows.Point(point.X, point.Y)
                    ];
                    return pointCollection;
                }
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}