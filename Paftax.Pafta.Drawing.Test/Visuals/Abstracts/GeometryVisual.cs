using Paftax.Pafta.Drawing.Utilities;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Test.Visuals.Abstracts
{
    public abstract class GeometryVisual : DrawingVisual
    {
        public Brush Stroke { get; set; } = Brushes.Black;
        public double StrokeThickness { get; set; } = UnitConverter.MmToPoint(0.1);
        public double Scale { get; set; } = 1.0;
        public Point Center { get; set; } = new(0, 0);
        public Geometry Geometry { get; set; } = new GeometryGroup();
        public abstract void Draw(DrawingContext drawingContext);
    }
}
