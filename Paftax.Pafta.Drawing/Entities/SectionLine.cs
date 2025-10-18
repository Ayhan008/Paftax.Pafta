using Paftax.Pafta.Drawing.Entities.Abstracts;
using Paftax.Pafta.Drawing.Structs;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Entities
{
    internal class SectionLine : Annotation
    {
        public PointXY Start { get; set; }
        public PointXY End { get; set; }
        public double Direction { get; set; }
        public override void Draw(DrawingContext drawingContext)
        {
            Pen pen = new(Brush, 0.5);
            drawingContext.DrawLine(pen, Start, End);

            DrawSectionHead(drawingContext);
            DrawSectionTail(drawingContext);
        }

        private void DrawSectionHead(DrawingContext drawingContext)
        {
            Pen pen = new(Brush, 0.5);

            Vector dir = new(End.X - Start.X, End.Y - Start.Y);
            dir.Normalize();

            Point center = new(Start.X - dir.X * 17.5, Start.Y - dir.Y * 17.5);
            double baseAngle = Math.Atan2(dir.Y, dir.X) * 180 / Math.PI;
            double angle = baseAngle + (Direction < 0 ? 180 : 0);

            drawingContext.PushTransform(new RotateTransform(angle, center.X, center.Y));

            StreamGeometry triangleGeometry = new();
            using (var ctx = triangleGeometry.Open())
            {
                ctx.BeginFigure(new Point(center.X + 24.75, center.Y), true, true);
                ctx.LineTo(new Point(center.X, center.Y + 24.75), true, false);
                ctx.LineTo(new Point(center.X - 24.75, center.Y), true, false);
            }

            EllipseGeometry ellipseGeometry = new(center, 17.5, 17.5);
            CombinedGeometry elevationGeometry = new(GeometryCombineMode.Exclude, triangleGeometry, ellipseGeometry);

            drawingContext.DrawGeometry(Brush, pen, elevationGeometry);
            drawingContext.Pop();

            drawingContext.PushTransform(new ScaleTransform(1, -1, center.X, center.Y));
            drawingContext.DrawEllipse(null, pen, center, 17.5, 17.5);
            drawingContext.DrawLine(pen, new Point(center.X - 17.5, center.Y), new Point(center.X + 17.5, center.Y));

            FormattedText sheetNumber = new(
                SheetNumber,
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"),
                8,
                Brush,
                1.0
            );
            drawingContext.DrawText(sheetNumber, new Point(center.X - sheetNumber.Width / 2, center.Y - sheetNumber.Height / 2 + 7));

            FormattedText detailNumber = new(
                DetailNumber,
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"),
                8,
                Brush,
                1.0
            );
            drawingContext.DrawText(detailNumber, new Point(center.X - detailNumber.Width / 2, center.Y - detailNumber.Height / 2 - 7));
            drawingContext.Pop();
        }

        private void DrawSectionTail(DrawingContext drawingContext)
        {
            Pen pen = new(Brush, 0.5);

            Vector dir = new(End.X - Start.X, End.Y - Start.Y);
            dir.Normalize();

            Point center = new(
                End.X - dir.X * Direction,
                End.Y - dir.Y * Direction
            );

            double baseAngle = Math.Atan2(dir.Y, dir.X) * 180 / Math.PI;
            double angle = baseAngle + (Direction < 0 ? 180 : 0);

            drawingContext.PushTransform(new RotateTransform(angle, center.X, center.Y));
            Rect rect = new(center.X, center.Y, 8, 24.75);
            drawingContext.DrawRectangle(Brush, pen, rect);

            drawingContext.Pop();
        }
    }
}
