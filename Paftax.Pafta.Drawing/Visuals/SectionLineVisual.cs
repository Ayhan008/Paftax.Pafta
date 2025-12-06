using Paftax.Pafta.Shared.Geometries;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawings.Visuals
{
    public class SectionLineVisual : GeometryVisual
    {
        public Point StartPoint { get; set; } = new(300, 500);
        public Point EndPoint { get; set; } = new(0, 200);
        public double Radius { get; set; } = UnitConverter.MmToPoint(6.25);
        public double TailLength { get; set; } = UnitConverter.MmToPoint(7);
        public double TailWidth { get; set; } = UnitConverter.MmToPoint(2);
        public string SheetNumber { get; set; } = "A101";
        public string DetailNumber { get; set; } = "1";
        public bool Flip { get; set; } = false;
        public double ViewBoxDepth { get; set; } = UnitConverter.MmToPoint(1000);
        public double ViewBoxWidth { get; set; } = UnitConverter.MmToPoint(1000);

        public override void Draw(DrawingContext dc)
        {
            if (StartPoint == EndPoint) return;
            Pen pen = new(Stroke, StrokeThickness * Scale);

            Vector dir = EndPoint - StartPoint;
            double len = dir.Length;

            if (len == 0) return;
            dir /= len;

            double angle = Math.Atan2(dir.Y, dir.X) * 180 / Math.PI;
            double headAngle = Flip ? angle + 180 : angle;

            Point sectionHeadCenter = StartPoint - dir * (Radius * Math.Sqrt(2) * Scale);

            Point sectionTailCenter = EndPoint - dir;

            dc.DrawLine(pen, StartPoint, EndPoint);

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

        protected override Geometry CreateGeometry()
        {
            throw new NotImplementedException();
        }
    }
}
