using Paftax.Pafta.Drawing.Geometries;

namespace Paftax.Pafta.Drawing.Entities.Abstracts
{
    public abstract class SpatialElement : Entity
    {
        public double Area { get; set; }
        public List<Curve> BoundarySegments { get; } = [];
    }
}
