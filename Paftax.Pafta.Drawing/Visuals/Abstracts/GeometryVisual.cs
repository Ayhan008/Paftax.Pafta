using Paftax.Pafta.Drawing.Structs;
using Paftax.Pafta.Drawing.Utilities;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Visuals.Abstracts
{
    public abstract class GeometryVisual : DrawingVisual
    {
        public Brush Stroke { get; set; } = Brushes.Black;
        public double StrokeThickness { get; set; } = UnitConverter.MmToPoint(0.1);
        public double Scale { get; set; } = 1.0;
        public Point2 Center { get; set; }
        public Bounding2 Bounding { get; set; }
        public abstract void Draw(DrawingContext drawingContext);
    }
}
