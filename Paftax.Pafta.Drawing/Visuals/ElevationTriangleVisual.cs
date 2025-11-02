using Paftax.Pafta.Drawing.Utilities;
using Paftax.Pafta.Drawing.Visuals.Abstracts;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Visuals
{
    internal class ElevationTriangleVisual : GeometryVisual
    {
        public double Radius { get; set; } = UnitConverter.MmToPoint(6.25);

        public override void Draw(DrawingContext dc)
        {
            double scaledRadius = Radius * Scale;

            Pen pen = new(Stroke, StrokeThickness * Scale);

            StreamGeometry elevationTriangle = new();
            using (StreamGeometryContext ctx = elevationTriangle.Open())
            {
                Point p1 = new(Center.X - scaledRadius / Math.Sqrt(2), Center.Y + scaledRadius / Math.Sqrt(2));
                Point p2 = new(Center.X, Center.Y + scaledRadius * Math.Sqrt(2));
                Point p3 = new(Center.X + scaledRadius / Math.Sqrt(2), Center.Y + scaledRadius / Math.Sqrt(2));

                ctx.BeginFigure(p1, true, true);
                ctx.LineTo(p2, true, false);
                ctx.LineTo(p3, true, true);

                ctx.ArcTo(p1, new Size(scaledRadius, scaledRadius), 0, false, SweepDirection.Clockwise, true, true);
            }

            elevationTriangle.Freeze();
            dc.DrawGeometry(Stroke, pen, elevationTriangle);
        }
    }
}
