using Paftax.Pafta.Drawing.Utilities;
using Paftax.Pafta.Drawing.Visuals.Abstracts;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Visuals
{
    internal class CalloutHeadVisual : GeometryVisual
    {
        public double Radius { get; set; } = UnitConverter.MmToPoint(6.25);
        public double FontSize { get; set; } = UnitConverter.MmToPoint(2.5);
        public string SheetNumber { get; set; } = string.Empty;
        public string DetailNumber { get; set; } = string.Empty;
        public override void Draw(DrawingContext dc)
        {
            double scaledRadius = Radius * Scale;

            double scaledFont = Math.Max(FontSize * Scale, 0.1);

            Pen pen = new(Stroke, StrokeThickness * Scale)
            {
                LineJoin = PenLineJoin.Round
            };

            dc.DrawEllipse(null, pen, Center, scaledRadius, scaledRadius);

            dc.DrawLine(pen,
                new Point(Center.X - scaledRadius, Center.Y),
                new Point(Center.X + scaledRadius, Center.Y));

            Typeface typeface = new("Arial");

            FormattedText sheetText = new(
                SheetNumber,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                typeface,
                scaledFont,
                Stroke,
                1.0);

            FormattedText detailText = new(
                DetailNumber,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                typeface,
                scaledFont,
                Stroke,
                1.0);

            Point sheetPoint = new(
                Center.X - sheetText.Width / 2,
                Center.Y - sheetText.Height - (2 * Scale));

            Point detailPoint = new(
                Center.X - detailText.Width / 2,
                Center.Y + (2 * Scale));

            dc.PushTransform(new ScaleTransform(1, -1, sheetPoint.X + sheetText.Width / 2, sheetPoint.Y + sheetText.Height / 2));
            dc.DrawText(sheetText, sheetPoint);
            dc.Pop();

            dc.PushTransform(new ScaleTransform(1, -1, detailPoint.X + detailText.Width / 2, detailPoint.Y + detailText.Height / 2));
            dc.DrawText(detailText, detailPoint);
            dc.Pop();
        }
    }
}
