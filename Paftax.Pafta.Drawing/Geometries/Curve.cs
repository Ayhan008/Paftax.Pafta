using Paftax.Pafta.Drawing.Structs;

namespace Paftax.Pafta.Drawing.Geometries
{
    public abstract class Curve
    {
        public abstract PointXY GetEndPoint(int index);
        public abstract double Length { get; }
    }
}
