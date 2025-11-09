using Paftax.Pafta.Drawings.Visuals;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawings.Elements
{
    public class ElevationMarker : DrawingElement
    {
        public override GeometryVisual Visual { get; }
        public ElevationMarker()
        {
            Visual = new ElevationMarkerVisual()
            {
                Angle = 0,
                TriangleVisible = [true, false, false, false],
                SheetNumber = "A101",
                DetailNumber = "1",
                Scale = Scale,
                Stroke = Brushes.Black,
                Center = Origin
            };
            IsAnnotation = true;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Visual.Draw(drawingContext);
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            Point pt = hitTestParameters.HitPoint;
            Geometry geometry = Visual.GetGeometry();
            PathGeometry widened = geometry.GetWidenedPathGeometry(new Pen(Visual.Stroke, 500.0));

            if (widened.FillContains(pt))
            {
                Visual.Stroke = Brushes.Red;
                InvalidateVisual();
                return new PointHitTestResult(this, pt);
            }
            else
            {
                Visual.Stroke = Brushes.Black;
                InvalidateVisual();
            }

            return base.HitTestCore(hitTestParameters);
        }
    }
}