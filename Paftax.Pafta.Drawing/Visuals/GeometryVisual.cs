using Paftax.Pafta.Shared.Geometries;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawings.Visuals
{
    public abstract class GeometryVisual : DrawingVisual
    {
        private Geometry? _geometry;

        // Genel çizim özellikleri
        public Brush Stroke { get; set; } = Brushes.Black;
        public virtual double StrokeThickness { get; set; } = UnitConverter.MmToPoint(0.1);
        public double Scale { get; set; } = 1.0;
        public Point2 Center { get; set; }
        public Geometry Geometry
        {
            get => _geometry ?? Geometry.Empty;
            protected set => _geometry = value;
        }
        public virtual Bounding2 Bounding
        {
            get
            {
                Rect bounds = Geometry.Bounds;
                return new Bounding2(
                    new Point2(bounds.Left, bounds.Top),
                    new Point2(bounds.Right, bounds.Bottom)
                );
            }
        }
        public virtual bool HitTest(Point2 point, double tolerance)
        {
            if (Geometry == null)
                return false;

            var widened = Geometry.GetWidenedPathGeometry(new Pen(Brushes.Black, tolerance));
            return widened.FillContains(point);
        }
        public virtual void Draw(DrawingContext dc)
        {
            if (Geometry == null) return;

            Pen pen = new(Stroke, StrokeThickness * Scale);
            dc.DrawGeometry(null, pen, Geometry);
        }
        protected abstract Geometry CreateGeometry();
        
        public void UpdateGeometry()
        {
            Geometry = CreateGeometry();
        }
    }
}
