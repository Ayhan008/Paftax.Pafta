using System.Windows.Media;

namespace Paftax.Pafta.Drawings.Visuals
{
    internal class ViewBoxRectVisual : GeometryVisual
    {
        public double Width { get; set; }
        public double Depth { get; set; }

        public override void Draw(DrawingContext drawingContext)
        {
            Pen pen = new(Stroke, StrokeThickness * Scale)
            {
                DashStyle = new DashStyle([4, 2], 0)
            };

            drawingContext.DrawGeometry(null, pen, GetGeometry());
        }

        public override Geometry GetGeometry()
        {
            Geometry geometry = new RectangleGeometry(new System.Windows.Rect(0, 0, Width, Depth));
            return geometry;
        }
    }
}
