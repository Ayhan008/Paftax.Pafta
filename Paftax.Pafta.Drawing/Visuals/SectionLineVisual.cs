using Paftax.Pafta.Drawing.Utilities;
using Paftax.Pafta.Drawing.Visuals.Abstracts;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Visuals
{
    public class SectionLineVisual : GeometryVisual
    {
        public Point StartPoint { get; set; } = new Point(300, 500);
        public Point EndPoint { get; set; } = new Point(0, 200);
        public double Radius { get; set; } = UnitConverter.MmToPoint(6.25);
        public double TailLength { get; set; } = UnitConverter.MmToPoint(7);
        public double TailWidth { get; set; } = UnitConverter.MmToPoint(2);
        public string SheetNumber { get; set; } = "A101";
        public string DetailNumber { get; set; } = "1";
        public bool Flip { get; set; } = false;
        public double Depth { get; set; } = 200;

        public override void Draw(DrawingContext dc)
        {
            // Prevent drawing if line has zero length
            if (StartPoint == EndPoint) return;

            // Pen for drawing the line and annotation shapes
            Pen pen = new(Stroke, StrokeThickness * Scale);

            // Compute direction vector and normalize it
            Vector dir = EndPoint - StartPoint;
            double len = dir.Length;

            if (len == 0) return;
            dir /= len; // Normalized direction

            // Compute the angle for head rotation
            double angle = Math.Atan2(dir.Y, dir.X) * 180 / Math.PI;
            double headAngle = Flip ? angle + 180 : angle;

            // Compute the center of the section head (apply scale)
            Point sectionHeadCenter = StartPoint - dir * (Radius * Math.Sqrt(2) * Scale);

            // Tail center (scale applied for annotation, not the line)
            Point sectionTailCenter = EndPoint - dir;

            // This line represents the real model measurement and does NOT scale.
            dc.DrawLine(pen, StartPoint, EndPoint);

            // Tail is purely an annotation, so it scales with zoom/Scale
            dc.PushTransform(new RotateTransform(angle, EndPoint.X, EndPoint.Y));
            dc.PushTransform(new ScaleTransform(1, Flip ? -1 : 1, sectionTailCenter.X + TailWidth * Scale / 2, sectionTailCenter.Y));
            dc.DrawRectangle(
                Stroke,
                pen,
                new Rect(sectionTailCenter, new Size(TailWidth * Scale, TailLength * Scale))
            );
            dc.Pop();
            dc.Pop();

            dc.PushTransform(new RotateTransform(headAngle, sectionHeadCenter.X, sectionHeadCenter.Y));
            SectionTriangleVisual sectionTriangle = new()
            {
                Center = sectionHeadCenter,
                Scale = Scale
            };
            sectionTriangle.Draw(dc);
            dc.Pop();

            CalloutHeadVisual calloutHead = new()
            {
                Center = sectionHeadCenter,
                Scale = Scale,
                Radius = Radius,
                SheetNumber = SheetNumber,
                DetailNumber = DetailNumber,
            };
            calloutHead.Draw(dc);
        }
    }
}
