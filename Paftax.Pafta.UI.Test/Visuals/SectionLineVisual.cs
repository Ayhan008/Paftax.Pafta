using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.UI.Test.Visuals
{
    internal class SectionLineVisual : GeometryVisual
    {
        public Point StartPoint { get; set; } = new Point(300, 500);
        public Point EndPoint { get; set; } = new Point(0, 200);
        public double HeadRadius { get; set; } = 50;
        public double TailLength { get; set; } = 60;
        public double TailWidth { get; set; } = 15;
        public Point Center => new((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2);
        public Brush Stroke { get; set; } = Brushes.Black;
        public double EmSize { get; set; } = 24;
        public double Thickness { get; set; } = 4;
        public bool Flip { get; set; } = false;

        public override void DrawGeometry(DrawingContext dc)
        {
            Pen pen = new(Stroke, Thickness);

            double dx = EndPoint.X - StartPoint.X;
            double dy = EndPoint.Y - StartPoint.Y;
            double angle = Math.Atan2(dy, dx) * 180 / Math.PI;

            Vector dir = EndPoint - StartPoint;
            dir.Normalize();

            double headAngle = Flip ? angle + 180 : angle;

            Point sectionHeadCenter = StartPoint - dir * (HeadRadius * Math.Sqrt(2));
            Point sectionTailCenter = EndPoint - dir;

            dc.DrawLine(pen, StartPoint, EndPoint + dir * TailWidth);

            dc.PushTransform(new RotateTransform(angle, EndPoint.X, EndPoint.Y));
            dc.PushTransform(new ScaleTransform(1, Flip ? -1 : 1, sectionTailCenter.X + TailWidth / 2, sectionTailCenter.Y));
            dc.DrawRectangle(Stroke, pen, new Rect(sectionTailCenter, new Size(TailWidth, TailLength)));
            dc.Pop();
            dc.Pop();

            dc.PushTransform(new RotateTransform(headAngle, sectionHeadCenter.X, sectionHeadCenter.Y));

            SectionTriangleVisual sectionTriangle = new()
            {
                Center = sectionHeadCenter,
                Radius = HeadRadius,
                Stroke = Stroke,
                Thickness = Thickness
            };

            sectionTriangle.DrawGeometry(dc);
            dc.Pop();

            CalloutHeadVisual calloutHead = new()
            {
                Center = sectionHeadCenter,
                Radius = HeadRadius,
                Stroke = Stroke,
                Thickness = Thickness,
                EmSize = EmSize
            };
            calloutHead.DrawGeometry(dc);
        }
    }
}
