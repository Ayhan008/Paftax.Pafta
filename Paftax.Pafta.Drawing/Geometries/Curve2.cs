using Paftax.Pafta.Drawing.Structs;

namespace Paftax.Pafta.Drawing.Geometries
{
    public abstract class Curve2
    {
        public abstract Point2 GetEndPoint(int index);
        public abstract double Length { get; }
    }
}
