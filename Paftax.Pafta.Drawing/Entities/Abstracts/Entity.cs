using Paftax.Pafta.Drawing.Geometries;
using Paftax.Pafta.Drawing.Structs;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Entities.Abstracts
{
    public abstract class Entity
    {
        public ElementId Id { get; set; }
        public Brush Brush { get; set; } = Brushes.Black;
        public double LineWeight { get; set; } = 1;
        public BoundingBoxXYZ Bounding { get; set; }
        public List<Curve> Geometry { get; } = [];
        public DrawingVisual DrawingVisual { get; set; } = new();
        public abstract void Draw(DrawingContext drawingContext);
    }
}
