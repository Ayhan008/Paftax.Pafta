using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Paftax.Pafta.UI.Converters
{
    public class RevitPointsToWindowsPointCollectionConverter : IValueConverter
    {
        // Sabit canvas boyutu
        public double CanvasWidth { get; set; } = 800;
        public double CanvasHeight { get; set; } = 450;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var points = (value as IEnumerable<System.Drawing.PointF>)?.Select(p => new System.Windows.Point(p.X, p.Y))
                        ?? (value as IEnumerable<System.Drawing.Point>)?.Select(p => new System.Windows.Point(p.X, p.Y));

            if (points == null || !points.Any()) return null;

            // BBox
            double minX = points.Min(p => p.X);
            double maxX = points.Max(p => p.X);
            double minY = points.Min(p => p.Y);
            double maxY = points.Max(p => p.Y);

            double roomWidth = maxX - minX;
            double roomHeight = maxY - minY;

            // Ölçekleme (aspect ratio koru)
            double scale = Math.Min(CanvasWidth / roomWidth, CanvasHeight / roomHeight);

            // Offset ile ortalama
            double offsetX = (CanvasWidth - roomWidth * scale) / 2 - minX * scale;
            double offsetY = (CanvasHeight - roomHeight * scale) / 2 - minY * scale;

            return new PointCollection(points.Select(p =>
                new System.Windows.Point(
                    p.X * scale + offsetX,
                    CanvasHeight - (p.Y * scale + offsetY) // Y eksenini ters çevir
                )
            ));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
