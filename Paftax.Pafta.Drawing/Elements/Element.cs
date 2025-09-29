using Paftax.Pafta.Drawing.Geometries;
using Paftax.Pafta.Drawing.Geometries.Primitives;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Elements
{
    public abstract class Element
    {
        public long ElementId { get; set; }
        public DrawingVisual DrawingVisual { get; set; } = new();
        public List<Curve> Geometry { get; } = [];
        public BoundingXY BoundingXY { get; private set; } = new BoundingXY();
    }
}
