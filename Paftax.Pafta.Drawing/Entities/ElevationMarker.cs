using Paftax.Pafta.Drawing.Entities.Abstracts;
using Paftax.Pafta.Drawing.Structs;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Entities
{
    public class ElevationMarker : Annotation
    {
        public PointXY Center { get; set; } = new(0, 0);

        public override void Draw(DrawingContext drawingContext)
        {
            Pen pen = new(Brush, 0.5);
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
            drawingContext.DrawGeometry(Brush, pen, elevationGeometry);
            drawingContext.Pop();

            drawingContext.DrawEllipse(null, pen, Center, 17.5, 17.5);
            drawingContext.DrawLine(pen, new Point(Center.X - 17.5, Center.Y), new Point(Center.X + 17.5, Center.Y));

            FormattedText sheetNumber = new(SheetNumber,
                                   CultureInfo.InvariantCulture,
                                   FlowDirection.LeftToRight,
                                   new Typeface("Arial"),
                                   8,
                                   Brush,
                                   1.0
            );
            drawingContext.DrawText(sheetNumber, new Point(Center.X - sheetNumber.Width / 2, Center.Y - sheetNumber.Height / 2 + 7));

            FormattedText detailNumber = new(DetailNumber,
                                   CultureInfo.InvariantCulture,
                                   FlowDirection.LeftToRight,
                                   new Typeface("Arial"),
                                   8,
                                   Brush,
                                   1.0
            );
            drawingContext.DrawText(detailNumber, new Point(Center.X - detailNumber.Width / 2, Center.Y - detailNumber.Height / 2 - 7));

            drawingContext.Pop();
        }
    }
}