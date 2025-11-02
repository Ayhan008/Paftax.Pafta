using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.UI.Test.Visuals
{
    internal class ElevationMarkerVisual : GeometryVisual
    {
        public Point Center { get; set; } = new Point(0, 0);
        public double Radius { get; set; } = 30;
        public Brush Stroke { get; set; } = Brushes.Black;
        public double Thickness { get; set; } = 2;
        public double Angle { get; set; } = 0;
        public double EmSize { get; set; } = 12;
        public bool[] TriangleVisible { get; } = [true, false, false, false];
        public override void DrawGeometry(DrawingContext dc)
        {
            CalloutHeadVisual calloutHead = new()
            {
                Center = Center,
                Radius = Radius,
                Stroke = Stroke,
                Thickness = Thickness,
                EmSize = EmSize,
            };
            calloutHead.DrawGeometry(dc);

            dc.PushTransform(new RotateTransform(Angle, Center.X, Center.Y));
            ElevationTriangleVisual elevationTriangle = new()
            {
                Center = Center,
                Radius = Radius,
                Stroke = Stroke,
                Thickness = Thickness
            };

            if (TriangleVisible[0])
                RotateAndDrawTriangle(dc, elevationTriangle, 0);
            if (TriangleVisible[1])
                RotateAndDrawTriangle(dc, elevationTriangle, 90);
            if (TriangleVisible[2])
                RotateAndDrawTriangle(dc, elevationTriangle, 180);
            if (TriangleVisible[3])
                RotateAndDrawTriangle(dc, elevationTriangle, 270);

            dc.Pop();
        }

        private static void RotateAndDrawTriangle(DrawingContext dc, ElevationTriangleVisual triangle, double angle)
        {
            dc.PushTransform(new RotateTransform(angle, triangle.Center.X, triangle.Center.Y));
            triangle.DrawGeometry(dc);
            dc.Pop();
        }
    }
}
