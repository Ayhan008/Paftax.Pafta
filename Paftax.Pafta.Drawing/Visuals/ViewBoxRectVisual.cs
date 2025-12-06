using System.Windows.Media;

namespace Paftax.Pafta.Drawings.Visuals
{
    internal class ViewBoxRectVisual : GeometryVisual
    {
        public double Width { get; set; }
        public double Depth { get; set; }
        protected override Geometry CreateGeometry()
        {
            Geometry geometry = new RectangleGeometry(new System.Windows.Rect(0, 0, Width, Depth));
            return geometry;
        }
    }
}
