using Paftax.Pafta.Drawing.Geometries;
using Paftax.Pafta.Drawing.Structs;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Annotations
{
    public abstract class Annotation
    {
        public long ElementId { get; set; }
        public DrawingVisual DrawingVisual { get; set; } = new();
        public List<Curve> Geometry { get; } = [];
        public abstract void Draw(DrawingContext drawingContext, Brush brush);
    }
}
