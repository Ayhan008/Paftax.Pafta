using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawings.Visuals
{
    internal class SectionTriangleVisual : GeometryVisual
    {
        public double Radius { get; set; } = UnitConverter.MmToPoint(6.25);

        public override void Draw(DrawingContext dc)
        {
            double scaledRadius = Radius * Scale;

            Pen pen = new(Stroke, StrokeThickness * Scale);

            StreamGeometry sectionTriangle = new();
            using (StreamGeometryContext ctx = sectionTriangle.Open())
            {
                Point p1 = new(Center.X - scaledRadius, Center.Y);
                Point p2 = new(Center.X - scaledRadius * Math.Sqrt(2), Center.Y);
                Point p3 = new(Center.X, Center.Y + scaledRadius * Math.Sqrt(2));
                Point p4 = new(Center.X + scaledRadius * Math.Sqrt(2), Center.Y);
                Point p5 = new(Center.X + scaledRadius, Center.Y);

                ctx.BeginFigure(p1, true, true);
                ctx.LineTo(p2, true, false);
                ctx.LineTo(p3, true, true);
                ctx.LineTo(p4, true, false);
                ctx.LineTo(p5, true, true);

                ctx.ArcTo(p1, new Size(scaledRadius, scaledRadius), 0, false, SweepDirection.Clockwise, true, true);
            }

            sectionTriangle.Freeze();

            dc.DrawGeometry(Stroke, pen, sectionTriangle);
        }

        protected override Geometry CreateGeometry()
        {
            throw new NotImplementedException();
        }
    }
}
