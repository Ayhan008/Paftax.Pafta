using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.UI.Test.Visuals
{
    internal class ElevationTriangleVisual : GeometryVisual
    {
        public Point Center { get; set; } = new Point(0, 0);
        public double Radius { get; set; } = 100;
        public Brush Stroke { get; set; } = Brushes.Black;
        public double Thickness { get; set; } = 6;

        public override void DrawGeometry(DrawingContext dc)
        {
            Pen pen = new(Stroke, Thickness);

            StreamGeometry elevationTriangle = new();
            using StreamGeometryContext ctx = elevationTriangle.Open();

            ctx.BeginFigure(new Point(Center.X - Radius/Math.Sqrt(2), Center.Y + Radius/Math.Sqrt(2)), true, true);
            ctx.LineTo(new Point(Center.X, Center.Y + Radius*Math.Sqrt(2)), true, false);
            ctx.LineTo(new Point(Center.X + Radius / Math.Sqrt(2), Center.Y + Radius / Math.Sqrt(2)), true, true);
            ctx.ArcTo(new Point(Center.X - Radius / Math.Sqrt(2), Center.Y + Radius / Math.Sqrt(2)), new Size(Radius,Radius), 0, false, SweepDirection.Clockwise, true, true);

            elevationTriangle.Freeze();
            dc.DrawGeometry(Stroke, pen, elevationTriangle);
        }
    }
}
