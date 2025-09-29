using Paftax.Pafta.Drawing.Geometries;
using Paftax.Pafta.Drawing.Geometries.Primitives;

namespace Paftax.Pafta.Drawing.Elements
{
    public class Room : Element
    {
        public string Name { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public double Area { get; set; }
        public List<Curve> BoundarySegments { get; } = [];
    }
}
