using System.Globalization;
using System.Windows.Data;

namespace Paftax.Pafta.UI.Converters
{
    public sealed class RevitPointToWindowsPointConverter : IValueConverter
    {
        public double CanvasWidth { get; set; } = 600;
        public double CanvasHeight { get; set; } = 450;

        public double Scale { get; set; } = 1.0;
        public double OffsetX { get; set; } = 0.0;
        public double OffsetY { get; set; } = 0.0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Drawing.Point p)
            {
                return new System.Windows.Point(
                    p.X * Scale + OffsetX,
                    CanvasHeight - (p.Y * Scale + OffsetY)
                );
            }
            if (value is System.Drawing.PointF pf)
            {
                return new System.Windows.Point(
                    pf.X * Scale + OffsetX,
                    CanvasHeight - (pf.Y * Scale + OffsetY)
                );
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
