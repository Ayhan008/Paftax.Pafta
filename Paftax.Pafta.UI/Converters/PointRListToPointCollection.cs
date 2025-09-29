using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Paftax.Pafta.UI.Converters
{
    public class PointDListToPointCollection : IValueConverter
    {
        public double TotalMinX { get; set; }
        public double TotalMaxY { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PointCollection pointCollection = [];

            if (value is IEnumerable<Point> pointRs)
            {
                foreach (Point pointD in pointRs)
                {
                    Point point = new(pointD.X - TotalMinX, TotalMaxY - pointD.Y);

                    pointCollection.Add(point);
                }
                return pointCollection;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
