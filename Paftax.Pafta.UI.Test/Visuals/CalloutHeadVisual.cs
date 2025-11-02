using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.UI.Test.Visuals
{
    internal class CalloutHeadVisual : GeometryVisual
    {
        public Point Center { get; set; } = new Point(0, 0);
        public double Radius { get; set; } = 80;
        public Brush Stroke { get; set; } = Brushes.Black;
        public double Thickness { get; set; } = 6;
        public double EmSize { get; set; } = 45;
        public string SheetNumber { get; set; } = "A101";
        public string DetailNumber { get; set; } = "1";

        public override void DrawGeometry(DrawingContext dc)
        {
            Pen pen = new(Stroke, Thickness);

            dc.DrawEllipse(null, pen, Center, Radius, Radius);
            dc.DrawLine(pen,
                new Point(Center.X - Radius, Center.Y),
                new Point(Center.X + Radius, Center.Y));

            Typeface typeface = new("Arial");

            FormattedText sheetText = new(
                SheetNumber,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                typeface,
                EmSize,
                Stroke,
                1.0);

            FormattedText detailText = new(
                DetailNumber,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                typeface,
                EmSize,
                Stroke,
                1.0);

            Point sheetPoint = new(
                Center.X - sheetText.Width / 2,
                Center.Y - sheetText.Height - 5);

            Point detailPoint = new(
                Center.X - detailText.Width / 2,
                Center.Y + 5);

            dc.PushTransform(new ScaleTransform(1, -1, sheetPoint.X + sheetText.Width / 2, sheetPoint.Y + sheetText.Height / 2));
            dc.DrawText(sheetText, sheetPoint);
            dc.Pop();

            dc.PushTransform(new ScaleTransform(1, -1, detailPoint.X + detailText.Width / 2, detailPoint.Y + detailText.Height / 2));
            dc.DrawText(detailText, detailPoint);
            dc.Pop();
        }
    }
}
