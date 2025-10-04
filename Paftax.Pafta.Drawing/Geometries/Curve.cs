using Paftax.Pafta.Drawing.Structs;

namespace Paftax.Pafta.Drawing.Geometries
{
    public abstract class Curve
    {
        public abstract XY GetEndPoint(int index);
        public abstract double Length { get; }
    }
}
