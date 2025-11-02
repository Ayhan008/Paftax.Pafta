using Paftax.Pafta.Drawing.Structs;
using Paftax.Pafta.Drawing.Utilities;
using Paftax.Pafta.Drawing.Visuals.Abstracts;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Visuals
{
    public class ElevationMarkerVisual : GeometryVisual
    {
        public double Angle { get; set; } = 0;
        public bool[] TriangleVisible { get; set; } = [true, false, false, false];
        public string SheetNumber { get; set; } = "A101";
        public string DetailNumber { get; set; } = "1";

        public override void Draw(DrawingContext dc)
        {
            CalloutHeadVisual calloutHead = new()
            {
                Center = Center,
                SheetNumber = SheetNumber,
                DetailNumber = DetailNumber,
                Scale = Scale,
                Stroke = Stroke,
                StrokeThickness = StrokeThickness
            };
            calloutHead.Draw(dc);

            dc.PushTransform(new RotateTransform(Angle, Center.X, Center.Y));

            ElevationTriangleVisual elevationTriangle = new()
            {
                Center = Center,
                Scale = Scale,
                Stroke = Stroke,
                StrokeThickness = StrokeThickness
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
            triangle.Draw(dc);
            dc.Pop();
        }
    }
}
