using System.Windows.Media;

namespace Paftax.Pafta.Drawings.Visuals
{
    public class ElevationMarkerVisual : GeometryVisual
    {
        public double Angle { get; set; } = 0;
        public bool[] TriangleVisible { get; set; } = [true, false, false, false];
        public string SheetNumber { get; set; } = "A101";
        public string DetailNumber { get; set; } = "1";

        private CalloutHeadVisual _calloutHeadVisual = new();
        private ElevationTriangleVisual _elevationTriangleVisual = new();

        public override void Draw(DrawingContext dc)
        {
            CreateGeometry();

            _calloutHeadVisual.Draw(dc);

            dc.PushTransform(new RotateTransform(Angle, Center.X, Center.Y));

            if (TriangleVisible[0])
                RotateAndDrawTriangle(dc, _elevationTriangleVisual, 0);
            if (TriangleVisible[1])
                RotateAndDrawTriangle(dc, _elevationTriangleVisual, 90);
            if (TriangleVisible[2])
                RotateAndDrawTriangle(dc, _elevationTriangleVisual, 180);
            if (TriangleVisible[3])
                RotateAndDrawTriangle(dc, _elevationTriangleVisual, 270);

            dc.Pop();
        }

        private static void RotateAndDrawTriangle(DrawingContext dc, ElevationTriangleVisual triangle, double angle)
        {
            dc.PushTransform(new RotateTransform(angle, triangle.Center.X, triangle.Center.Y));
            triangle.Draw(dc);
            dc.Pop();
        }

        public override Geometry GetGeometry()
        {
            return CreateGeometry();
        }

        public GeometryVisual GetCalloutHeadVisual()
        {
            return _calloutHeadVisual;
        }
        public GeometryVisual GetElevationTriangleVisual()
        {
            return _elevationTriangleVisual;
        }

        private GeometryGroup CreateGeometry()
        {
            GeometryGroup group = new();

            CalloutHeadVisual calloutHead = new()
            {
                Center = Center,
                SheetNumber = SheetNumber,
                DetailNumber = DetailNumber,
                Scale = Scale,
            };
            _calloutHeadVisual = calloutHead;
            group.Children.Add(calloutHead.GetGeometry());

            ElevationTriangleVisual elevationTriangle = new()
            {
                Center = Center,
                Scale = Scale,
            };
            _elevationTriangleVisual = elevationTriangle;
            group.Children.Add(elevationTriangle.GetGeometry());

            return group;
        }
    }
}
