namespace Paftax.Pafta.Drawing.Geometries.Primitives
{
    public abstract class Curve
    {
        public abstract PointXY GetEndPoint(int index);
        public abstract double Length { get; }
    }
}
