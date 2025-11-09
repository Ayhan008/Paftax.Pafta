using Paftax.Pafta.Shared.Geometries;

namespace Paftax.Pafta.Drawings
{
    public abstract class Curve
    {
        public abstract Point2 GetEndPoint(int index);
        public abstract double Length { get; }
    }
}
