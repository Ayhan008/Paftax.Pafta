using Paftax.Pafta.Drawing.Test.Visuals.Abstracts;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Test.Visuals
{
    internal class ViewBoxRectVisual : GeometryVisual
    {
        public double Width { get; set; }
        public double Depth { get; set; }

        public override void Draw(DrawingContext drawingContext)
        {
        }
    }
}
