using Paftax.Pafta.Drawing.Structs;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Annotations
{
    public class Elevation : Annotation
    {
        public XY Center { get; set; } = new(0, 0);
        public double Rotation { get; set; } = 0;
        public string SheetNumber { get; set; } = "A101";
        public string DetailNumber { get; set; } = "1";
        public double Start { get; set; } = 10;
        public double Width { get; set; } = 300;
        public double Height { get; set; } = 35;
        public double Depth { get; set; } = 150;
        public bool IsViewBoxVisible { get; set; } = false;
        public bool HitCircle(XY xy)
        {
            EllipseGeometry ellipse = new(Center, 17.5, 17.5);
            return ellipse.FillContains(xy);
        }
        public bool HitTriangle(XY xy)
        {
            StreamGeometry triangleGeometry = new();
            using (StreamGeometryContext ctx = triangleGeometry.Open())
            {
                ctx.BeginFigure(new Point(Center.X + 24.75, Center.Y), true, true);
                ctx.LineTo(new Point(Center.X, Center.Y + 24.75), true, false);
                ctx.LineTo(new Point(Center.X - 24.75, Center.Y), true, false);
            }
            CombinedGeometry combinedGeometry = new(GeometryCombineMode.Exclude, triangleGeometry, new EllipseGeometry(Center, 17.5, 17.5));
            RotateTransform rotateTransform = new(Rotation, Center.X, Center.Y);
            combinedGeometry.Transform = rotateTransform;

            return combinedGeometry.FillContains(xy);
        }

        public override void Draw(DrawingContext drawingContext, Brush brush)
        {
            Pen pen = new(brush, 0.5);
            

            drawingContext.PushTransform(new ScaleTransform(1, -1, Center.X, Center.Y));

            EllipseGeometry ellipseGeometry = new(Center, 17.5, 17.5);

            StreamGeometry triangleGeometry = new();
            using (StreamGeometryContext ctx = triangleGeometry.Open())
            {
                ctx.BeginFigure(new Point(Center.X + 24.75, Center.Y), true, true);
                ctx.LineTo(new Point(Center.X, Center.Y + 24.75), true, false);
                ctx.LineTo(new Point(Center.X - 24.75, Center.Y), true, false);
            }

            CombinedGeometry elevationGeometry = new(GeometryCombineMode.Exclude, triangleGeometry, ellipseGeometry);

            drawingContext.PushTransform(new RotateTransform(Rotation, Center.X, Center.Y));

            if (IsViewBoxVisible)
            {
                DrawViewBox(drawingContext, brush);
            }

            drawingContext.DrawGeometry(brush, pen, elevationGeometry);
            drawingContext.Pop();

            drawingContext.DrawEllipse(Brushes.Transparent, pen, Center, 17.5, 17.5);
            drawingContext.DrawLine(pen, new Point(Center.X - 17.5, Center.Y), new Point(Center.X + 17.5, Center.Y));

            FormattedText sheetNumber = new(SheetNumber,
                                   CultureInfo.InvariantCulture,
                                   FlowDirection.LeftToRight,
                                   new Typeface("Arial"),
                                   8,
                                   brush,
                                   1.0
            );
            drawingContext.DrawText(sheetNumber, new Point(Center.X - sheetNumber.Width / 2, Center.Y - sheetNumber.Height / 2 + 7));

            FormattedText detailNumber = new(DetailNumber,
                                   CultureInfo.InvariantCulture,
                                   FlowDirection.LeftToRight,
                                   new Typeface("Arial"),
                                   8,
                                   brush,
                                   1.0
            );
            drawingContext.DrawText(detailNumber, new Point(Center.X - detailNumber.Width / 2, Center.Y - detailNumber.Height / 2 - 7));

            drawingContext.Pop();
        }

        private void DrawViewBox(DrawingContext drawingContext, Brush brush)
        {
            Pen pen = new(brush, 0.5);

            Pen dashedPen = new(brush, 0.5)
            {
                DashStyle = new DashStyle([4, 2], 0),
                LineJoin = PenLineJoin.Miter
            };

            drawingContext.DrawLine(pen, new XY(-Width/2, Start), new XY(Width/2, Start));

            drawingContext.DrawLine(dashedPen, new XY(-Width / 2, Start), new XY(-Width / 2, Depth));
            drawingContext.DrawLine(dashedPen, new XY(Width / 2, Start), new XY(Width / 2, Depth));

            drawingContext.DrawLine(dashedPen, new XY(-Width / 2, Depth), new XY(Width / 2, Depth));
        }
    }
}