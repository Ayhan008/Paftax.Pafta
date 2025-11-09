using Paftax.Pafta.Shared.Geometries;
using System.Windows.Media;

namespace Paftax.Pafta.Drawings.Visuals
{
    public abstract class GeometryVisual : DrawingVisual
    {
        public Brush Stroke { get; set; } = Brushes.Black;
        public double StrokeThickness { get; set; } = UnitConverter.MmToPoint(0.1);
        public double Scale { get; set; } = 1.0;
        public Point2 Center { get; set; }
        public Bounding2 Bounding { get; set; }
        public Geometry Geometry => GetGeometry();
        public abstract Geometry GetGeometry();
        public abstract void Draw(DrawingContext drawingContext);
        public virtual bool HitTest(Point2 point, double tolerance)
        {
            var geometry = GetGeometry();
            if (geometry == null)
                return false;

            var widened = geometry.GetWidenedPathGeometry(new Pen(Brushes.Black, tolerance));
            return widened.FillContains(point);
        }
    }
}
